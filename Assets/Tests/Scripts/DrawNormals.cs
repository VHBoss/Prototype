using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNormals : MonoBehaviour
{
    public MeshFilter m_MeshFilter;

    void Start()
    {
        Mesh mesh = m_MeshFilter.mesh;
        //mesh.sc
        foreach (var item in mesh.normals)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale = Vector3.one * 0.3f;
            go.transform.position = item;
            go.transform.rotation = transform.rotation;
        }
    }
}
