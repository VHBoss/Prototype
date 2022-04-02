using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500f;
    public Animator CharacterAnimator;
    public CapsuleCollider CharacterCollider;
    public VariableJoystick variableJoystick;

    private Vector3 m_PlayerVelocity;
    private CapsuleCollider m_Collider;
    private Rigidbody m_Rigidbody;

    private readonly float gravityValue = -9.81f;
    private readonly int RunAnimation = Animator.StringToHash("Run");

    private void Start()
    {
        m_Collider = GetComponent<CapsuleCollider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider.radius = 0.5f;
    }

    private void FixedUpdate()
    {
        Vector3 move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif

        if (move != Vector3.zero)
        {
            CharacterAnimator.SetBool(RunAnimation, true);
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            Quaternion rotation = Quaternion.RotateTowards(m_Rigidbody.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation(rotation);
        }
        else
        {
            CharacterAnimator.SetBool(RunAnimation, false);
        }

        m_Rigidbody.MovePosition(transform.position + move * Time.fixedDeltaTime * moveSpeed);
    }

    public void AttachSnowball(SnowballController snowball)
    {
        Vector3 newPosition = transform.position + transform.forward * snowball.GetAxis();
        Vector3 deltaPosition = transform.position - newPosition;

        transform.position = newPosition;
        CharacterCollider.transform.position += deltaPosition;

        snowball.transform.SetParent(transform);
        snowball.transform.localPosition = new Vector3(0, snowball.transform.localPosition.y, 0);

        CharacterCollider.enabled = true;
    }
}
