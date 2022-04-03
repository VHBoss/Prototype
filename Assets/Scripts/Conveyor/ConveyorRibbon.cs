using DG.Tweening;
using UnityEngine;

public class ConveyorRibbon : MonoBehaviour
{
    [SerializeField] private SnowballConveyor m_SnowballConveyor;
    [SerializeField] private Transform m_StartPoint;
    [SerializeField] private Transform m_EndPoint;

    private Transform m_Target;

    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController controller = collision.gameObject.GetComponent<CharacterController>();
            SnowballController snowball = controller.Snowball;
            if (snowball != null)
            {
                controller.DetachSnowball();
                MoveToConveyor(snowball);
            }
        }
    }

    private void MoveToConveyor(SnowballController snowball)
    {
        snowball.Collider.enabled = false;
        snowball.ResetTarget();

        Transform snoballTransform = snowball.transform;

        Vector3 startMove = m_StartPoint.position;
        startMove.y = snoballTransform.position.y;

        Vector3 endMove = m_EndPoint.position;
        endMove.y = snoballTransform.position.y;

        DOTween.Sequence()
            .Append(snoballTransform.DOMove(startMove, 1))
            .Append(snoballTransform.DOMove(endMove, 2))
            .OnComplete(() => m_SnowballConveyor.ProcessSnowball(snowball));
    }
}
