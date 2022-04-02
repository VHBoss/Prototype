using UnityEngine;

public class SnowballController : MonoBehaviour
{    
    [SerializeField]
    private float m_GrowSpeed = 1;
    [SerializeField]
    private float m_MaxSize = 10;
    [SerializeField]
    private float m_BallOffset = 0.7f;

    [Header("Debug")]
    [SerializeField]
    private TMPro.TMP_Text m_SliderValue;

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

    private void Start()
    {
        m_SphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CharacterController character = other.GetComponent<CharacterController>();
            character.AttachSnowball(this);

            m_StartScale = transform.localScale;
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

    void UpdateCollidersState()
    {
        m_PlayerCollider.enabled = AxisOffset > 0;
        m_BaseCollider.enabled = AxisOffset == 1;
        m_SphereCollider.isTrigger = AxisOffset == 1;
    }
}
