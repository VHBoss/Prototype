using DG.Tweening;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private Collider m_Collider;
    private Transform m_Slot;
    private float m_AnimationSpeed = 0.5f;

    private void Start()
    {
        m_Collider = GetComponent<Collider>();
    }

    public void Init(Transform slot)
    {
        m_Slot = slot;
        DOTween.Sequence()
            .AppendCallback(() => { m_Collider.enabled = false; })
            .Append(transform.DOMove(slot.position, m_AnimationSpeed)).SetEase(Ease.InCirc)
            .Insert(0, transform.DORotateQuaternion(Quaternion.identity, m_AnimationSpeed).OnComplete(() => { m_Collider.enabled = true; }));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();

            if (characterController.Bag.TryAddItem(this))
            {
                m_Collider.enabled = false;

                if (m_Slot)
                {
                    m_Slot.gameObject.SetActive(true);
                    m_Slot.GetComponentInParent<ConveyorArea>().OnSlotRestored();
                }
            }
        }
    }
}
