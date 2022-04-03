using DG.Tweening;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private Collider m_Collider;
    private Transform m_Slot;

    private void Start()
    {
        m_Collider = GetComponent<Collider>();
    }

    public void Init(Transform slot)
    {
        m_Slot = slot;
        DOTween.Sequence()
            .AppendCallback(() => { m_Collider.enabled = false; })
            .Append(transform.DOMove(slot.position, 1))
            .Insert(0, transform.DORotateQuaternion(Quaternion.identity, 1).OnComplete(() => { m_Collider.enabled = true; }));
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
                }
            }
        }
    }
}
