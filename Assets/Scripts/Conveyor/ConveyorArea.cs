using UnityEngine;

public class ConveyorArea : MonoBehaviour
{
    [SerializeField] SnowballConveyor m_SnowballConveyor;

    public Transform GetEmptySlot()
    {
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeInHierarchy)
            {
                return item;
            }
        }
        return null;
    }

    public void OnSlotRestored()
    {
        m_SnowballConveyor.ProccessIce();
    }
}
