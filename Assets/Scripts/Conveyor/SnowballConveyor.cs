using DG.Tweening;
using UnityEngine;

public class SnowballConveyor : MonoBehaviour
{
    [SerializeField] private Transform m_IceSpawnPoint;
    [SerializeField] private Ice m_IcePrefab;
    [SerializeField] private ConveyorArea m_Area;
    [SerializeField] private float m_ConvertionTime = 2f;
    [SerializeField] private float m_NewIceEveryScale = 0.5f;

    private SnowballController m_Snowball;

    public void ProcessSnowball(SnowballController snowball)
    {
        m_Snowball = snowball;

        ProccessIce();
    }

    private void ProccessIce()
    {
        Transform slot = m_Area.GetEmptySlot();
        if (slot == null)
        {
            return;
        }

        Transform snowballTransform = m_Snowball.transform;
        float nextScale = snowballTransform.localScale.x - m_NewIceEveryScale;
        if (nextScale > 0) {
            Vector3 endScale = Vector3.one * nextScale;
            snowballTransform.DOScale(endScale, m_ConvertionTime).OnComplete(() => CreateIce(slot));
        }
        else
        {
            m_Snowball.gameObject.SetActive(false);
            m_Snowball = null;
        }
    }

    private void CreateIce(Transform slot)
    {
        Ice ice = Instantiate(m_IcePrefab, m_IceSpawnPoint.position, m_IceSpawnPoint.rotation);
        ice.Init(slot);

        slot.gameObject.SetActive(false);

        ProccessIce();
    }
}
