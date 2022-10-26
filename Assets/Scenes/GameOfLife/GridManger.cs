using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridManger : MonoBehaviour
{

    public int length = 20;
    private bool[,] cells;
    private Vector3[] vertices;
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        initializeGrid();
    }

    void initializeGrid()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        vertices = new Vector3[(length + 1) * (length + 1)];
        int i = 0;
        for (int y = 0; y <= length; y++)
        {
            for (int x = 0; x <= length; x++)
            {
                vertices[i] = new Vector3(x, y);
                i++;
            }
        }

        mesh.vertices = vertices;

        int[] triangles = new int[length * length * 6];
        for (int ti = 0, vi = 0, y = 0; y < length; y++, vi++)
        {
            for (int x = 0; x < length; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + length + 1;
                triangles[ti + 5] = vi + length + 2;
            }
        }
        mesh.triangles = triangles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
