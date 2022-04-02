using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    private Vector3 m_TargetOffset;

    private void Start()
    {
        m_TargetOffset = transform.position;
    }

    void LateUpdate()
    {
        transform.position = m_Target.position + m_TargetOffset;
    }
}
