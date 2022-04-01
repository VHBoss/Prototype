using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerTest : MonoBehaviour
{
    public CollideTest collideTest;

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.name);
        collideTest.Stop();
    }

    private void OnTriggerExit(Collider other)
    {
        print(other.transform.name);
        collideTest.Resume();
    }
}
