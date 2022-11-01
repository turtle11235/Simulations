using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridManger : MonoBehaviour
{
    public Camera mainCamera;

    public int length = 20;
    public MeshFilter meshFilter;
    public Color aliveColor;
    public Color deadColor;
    public float animationSpeed = .5f;
    public bool running = false;
    public bool random = false;

    private Mesh mesh;
    private Cell[,] cells;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        initializeGrid();
        mainCamera.transform.position = new Vector3(length / 2f, length / 2f, -1);
        mainCamera.GetComponent<Camera>().orthographicSize = length / 2f;
    }

    void initializeGrid()
    {
        meshFilter.mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        cells = new Cell[length, length];
        Vector3[] vertices = new Vector3[length * length * 4];
        int[] triangles = new int[length * length * 6];
        Color[] colors = new Color[length * length * 4];

        int vertexIndex = 0;
        int triangleIndex = 0;
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {

                cells[x, y] = new Cell(x, y, vertexIndex, Random.Range(0f, 1f) < .5);

                vertices[vertexIndex] = new Vector3(x, y);
                vertices[vertexIndex + 1] = new Vector3(x+1, y);
                vertices[vertexIndex + 2] = new Vector3(x, y+1);
                vertices[vertexIndex + 3] = new Vector3(x + 1, y + 1);

                Color color = cells[x,y].isAlive ? aliveColor : deadColor;
                colors[vertexIndex] = color;
                colors[vertexIndex + 1] = color;
                colors[vertexIndex + 2] = color;
                colors[vertexIndex + 3] = color;

                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 3] = triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 4] = triangles[triangleIndex + 1] = vertexIndex + 2;
                triangles[triangleIndex + 5] = vertexIndex + 3;

                vertexIndex += 4;
                triangleIndex += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= animationSpeed)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                updateCells();
                elapsedTime = 0;
                timer.Stop();
                Debug.Log("Updated " + (length * length) + " cells in " + timer.ElapsedMilliseconds / 1000f + " seconds");
            }
        }

    }

    void updateCells()
    {
        Color[] newColors = mesh.colors;
        bool[,] nextValues = new bool[length, length];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                nextValues[x, y] = willLive(cells[x, y]);
            }
        }

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                cells[x, y].isAlive = nextValues[x, y];
                int i = cells[x, y].index;
                Color color = cells[x, y].isAlive ? aliveColor : deadColor;
                newColors[i] = newColors[i + 1] = newColors[i + 2] = newColors[i + 3] = color;
            }
        }

        mesh.colors = newColors;
    }

    bool willLive(Cell c)
    {
        bool isAlive = c.isAlive;
        int liveNeighborCount = 0;
        Cell[] neighbors = getNeighbors(c);
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor != null && neighbor.isAlive)
            {
                liveNeighborCount += 1;
            }
        }

        if (isAlive)
        {
            if (liveNeighborCount == 2 || liveNeighborCount == 3)
            {
                return true;
            }
        }
        else
        {
            if (liveNeighborCount == 3)
            {
                return true;
            }
        }
        return false;
    }

    Cell[] getNeighbors(Cell c)
    {
        Cell[] neighbors = new Cell[9];
        int index = 0;
        for (int i = c.x - 1; i <= c.x + 1; i++)
        {
            for (int j = c.y - 1; j <= c.y + 1; j++)
            {
                int row = (i % length + length) % length;
                int col = (j % length + length) % length;
                if (!(i == c.x && j == c.y))
                {
                    Cell neighbor = cells[row, col];
                    neighbors[index] = neighbor;
                }
                index++;
            }
        }
        return neighbors;
    }

}
