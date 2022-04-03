using DG.Tweening;
using UnityEngine;

public class CharacterBag : MonoBehaviour
{
    [SerializeField] private int m_MaxCount;
    private float m_AnimationTime = 0.2f;

    public bool TryAddItem(Ice ice)
    {
        if (transform.childCount < m_MaxCount)
        {
            Vector3 slotPosition = new Vector3(0, transform.childCount * 0.5f, 0);

            Transform iceTransform = ice.transform;
            iceTransform.SetParent(transform);
            iceTransform.DOLocalMove(slotPosition, m_AnimationTime);
            iceTransform.DOLocalRotate(new Vector3(-90, 0, 0), m_AnimationTime);

            return true;
        }

        return false;
    }

    public Transform GetIce()
    {
        if (transform.childCount > 0)
        {
            return transform.GetChild(transform.childCount - 1);
        }
        else
        {
            return null;
        }
    }
}
