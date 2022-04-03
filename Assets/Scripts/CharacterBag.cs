using DG.Tweening;
using UnityEngine;

public class CharacterBag : MonoBehaviour
{
    [SerializeField] private int m_MaxCount;
    private float m_AnimationTime = 0.2f;

    private int m_CurrentCount;

    public bool TryAddItem(Ice ice)
    {
        if (m_CurrentCount < m_MaxCount)
        {
            Vector3 slotPosition = new Vector3(0, m_CurrentCount * 0.5f, 0);
            m_CurrentCount++;

            Transform iceTransform = ice.transform;
            iceTransform.SetParent(transform);
            iceTransform.DOLocalMove(slotPosition, m_AnimationTime);
            iceTransform.DOLocalRotate(new Vector3(-90, 0, 0), m_AnimationTime);

            return true;
        }

        return false;
    }
}
