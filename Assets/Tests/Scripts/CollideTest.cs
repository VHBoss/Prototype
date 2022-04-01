using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideTest : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500f;

    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity;
    bool stop;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();

        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        m_EulerAngleVelocity = new Vector3(0, 100, 0);
    }

    public void Stop()
    {
        stop = true;
    }

    public void Resume()
    {
        stop = false;
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //transform.Rotate(m_EulerAngleVelocity * Time.fixedDeltaTime);
        //Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        //m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);

        if (move != Vector3.zero && !stop)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            Quaternion r = Quaternion.RotateTowards(m_Rigidbody.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation(r);
            //m_Rigidbody.AddTorque(0, 10, 0, ForceMode.Impulse);
        }

        m_Rigidbody.MovePosition(transform.position + move * Time.fixedDeltaTime * moveSpeed);
    }
}
