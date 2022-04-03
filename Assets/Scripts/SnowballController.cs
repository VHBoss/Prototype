using DG.Tweening;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    [SerializeField]
    private IndentDraw m_IndentDraw;
    [SerializeField]
    private float m_GrowSpeed = 1;
    [SerializeField]
    private float m_MaxSize = 10;
    [SerializeField]
    private float m_BallOffset = 0.7f;
    [SerializeField]
    private float m_RespawnTime = 5f;

    [Header("Debug")]
    [SerializeField]
    private TMPro.TMP_Text m_SliderValue;

    public SphereCollider Collider => m_SphereCollider;

    private float m_Distance;
    private float AxisOffset = 0f;
    private bool m_IsTargered;
    private Vector3 m_PlayerPrevPosition;
    private Transform m_RootTransform;
    private Transform m_PlayerTransform;
    private Vector3 m_StartScale;
    private CapsuleCollider m_BaseCollider;
    private CapsuleCollider m_PlayerCollider;
    private SphereCollider m_SphereCollider;
    private Vector3 m_StartPosition;

    private void Start()
    {
        m_SphereCollider = GetComponent<SphereCollider>();
        m_StartPosition = transform.position;
        m_StartScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CharacterController character = other.GetComponent<CharacterController>();
            character.AttachSnowball(this);

            m_RootTransform = character.transform;
            m_PlayerTransform = character.CharacterCollider.transform;
            m_PlayerPrevPosition = m_RootTransform.position;
            m_BaseCollider = character.GetComponent<CapsuleCollider>();
            m_PlayerCollider = character.CharacterCollider;

            UpdateCollidersState();

            m_Distance = 0;
            m_IsTargered = true;
        }
    }

    void Update()
    {
        if (m_IsTargered)
        {
            UpdateDistance();
            DrawTrail();
        }
    }

    private void UpdateDistance()
    {
        if (transform.localScale.x < m_MaxSize)
        {

            m_Distance += (m_RootTransform.position - m_PlayerPrevPosition).sqrMagnitude;
            m_PlayerPrevPosition = m_RootTransform.position;
            transform.localScale = m_StartScale + Vector3.one * m_Distance * m_GrowSpeed;
            //m_PlayerCollider.radius = transform.localScale.x * 0.5f;
        }

        m_BaseCollider.radius = Mathf.Lerp(m_PlayerCollider.radius, transform.localScale.x * 0.5f, AxisOffset);
        transform.localPosition = new Vector3(0, transform.localScale.x * 0.5f, (transform.localScale.x * 0.5f + m_BallOffset) * (1 - AxisOffset));
        m_PlayerTransform.localPosition = new Vector3(0, 0, -GetAxis());
    }

    public float GetAxis()
    {
        return (transform.localScale.x * 0.5f + m_BallOffset) * AxisOffset;
    }

    public void OnAxisChanged(float value)
    {
        AxisOffset = value;
        m_SliderValue.text = value.ToString("0.0");

        if (m_IsTargered)
        {
            UpdateCollidersState();
        }
    }

    public void ResetTarget()
    {
        m_IsTargered = false;
    }

    private void UpdateCollidersState()
    {
        m_PlayerCollider.enabled = AxisOffset > 0;
        m_BaseCollider.enabled = AxisOffset == 1;
        m_SphereCollider.isTrigger = AxisOffset == 1;
    }

    private void DrawTrail()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100, 1 << 4))
        {
            m_IndentDraw.IndentAt(hit, transform.localScale.x*0.5f);
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(false);

        DOTween.Sequence()
            .AppendInterval(m_RespawnTime)
            .AppendCallback(() =>
            {
                transform.position = m_StartPosition;
                gameObject.SetActive(true);
            })
            .Append(transform.DOScale(Vector3.one * 0.5f, 0.5f)).OnComplete(() =>
            {
                m_Distance = 0;
                m_SphereCollider.enabled = true;
                m_SphereCollider.isTrigger = true;
            });
    }
}
