using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorArea : MonoBehaviour
{
    //private Transform[] m_Places;

    void Start()
    {
        //m_Places = GetComponentsInChildren<Transform>();
    }

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
}
