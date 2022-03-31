using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500f;
    public Transform CharacterRoot;
    public Animator CharacterAnimator;
    public VariableJoystick variableJoystick;
    public bool UseRigidbody;

    private Vector3 m_PlayerVelocity;
    private CharacterController m_Controller;
    private Rigidbody m_Rigidbody;

    private readonly float gravityValue = -9.81f;
    private readonly int RunAnimation = Animator.StringToHash("Run");

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //if (m_Controller.isGrounded && m_PlayerVelocity.y < 0)
        //{
        //    m_PlayerVelocity.y = 0f;
        //}

        Vector3 move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif
        if (UseRigidbody)
        {
            //m_Rigidbody.MovePosition(transform.position + move * Time.deltaTime * moveSpeed);
        }
        else
        {
            m_Controller.Move(move * Time.deltaTime * moveSpeed);
        }

        if (move != Vector3.zero)
        {
            CharacterAnimator.SetBool(RunAnimation, true);
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            CharacterRoot.rotation = Quaternion.RotateTowards(CharacterRoot.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            CharacterAnimator.SetBool(RunAnimation, false);
        }

        //m_PlayerVelocity.y += gravityValue * Time.deltaTime;
        //m_Controller.Move(m_PlayerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif
        m_Rigidbody.MovePosition(transform.position + move * Time.fixedDeltaTime * moveSpeed);

        //if (move != Vector3.zero)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
        //    Quaternion r = Quaternion.RotateTowards(m_Rigidbody.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        //    m_Rigidbody.MoveRotation(r);
        //}
    }
}
