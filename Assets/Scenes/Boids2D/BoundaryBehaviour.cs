using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBehaviour : MonoBehaviour
{
    public bool removeExistingColliders = true;

    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] temp = mesh.triangles;
        Array.Reverse(temp);
        mesh.triangles = temp;
    }
}
