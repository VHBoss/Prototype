using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField]
    private float m_GrowSpeed = 1;
    [SerializeField]
    private float m_MaxSize = 10;
    [SerializeField]
    private float m_BallOffset = 1.5f;

    private float m_Distance;
    private bool m_IsTargered;
    private Vector3 m_PlayerPrevPosition;
    private Transform m_RootTransform;
    //private Transform m_PlayerTransform;
    private Vector3 m_StartScale;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GetComponent<SphereCollider>().isTrigger = false;

            Character character = other.GetComponent<Character>();
            other.enabled = false;

            transform.SetParent(other.transform);
            transform.localPosition = new Vector3(0, transform.localScale.x * 0.5f, transform.localScale.x * 0.5f + m_BallOffset);

            m_StartScale = transform.localScale;
            m_RootTransform = character.transform;
            m_PlayerPrevPosition = m_RootTransform.position;
            //m_PlayerTransform = character.CharacterAnimator.transform;
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
        }

        transform.localPosition = new Vector3(0, transform.localScale.x * 0.5f, transform.localScale.x * 0.5f + m_BallOffset);
    }
}
