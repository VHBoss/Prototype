using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private Vector3 m_TargetOffset;

    void LateUpdate()
    {
        transform.position = m_Target.position + m_TargetOffset;
    }
}
