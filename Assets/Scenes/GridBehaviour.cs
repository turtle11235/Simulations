using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public GameObject tilePrefab;
    public int xLength = 10;
    public int yLength = 10;
    public float tileX = 1f;
    public float tileY = 1f;
    public float animationSpeed = .5f;
    public bool run = false;

    private float elapsedTime = 0f;
    private GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[xLength, yLength];
        float xOffset = xLength / 2f - tileX / 2;
        float yOffset = yLength / 2f - tileY / 2;
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                grid[x, y] = Instantiate(tilePrefab, new Vector3(x - xOffset, y - yOffset, 0), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= animationSpeed)
            {
                updateCells();
                elapsedTime = 0;
            }
        }
        
    }

    void updateCells()
    {
        bool[,] nextValues = new bool[xLength, yLength];
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                nextValues[x, y] = willLive(x, y);
            }
        }

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                grid[x, y].GetComponent<TileBehaviour>().isAlive = nextValues[x, y];
            }
        }
    }

    bool willLive(int x, int y)
    {
        bool isAlive = grid[x, y].GetComponent<TileBehaviour>().isAlive;
        int liveNeighborCount = 0;
        foreach(GameObject neighbor in getNeighbors(x, y))
        {
            if (neighbor != null && neighbor.GetComponent<TileBehaviour>().isAlive)
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

    GameObject[] getNeighbors(int x, int y)
    {
        GameObject[] neighbors = new GameObject[9];
        int index = 0;
        for (int i = Math.Max(0, x-1); i <= Math.Min(xLength-1, x+1); i++)
        {
            for (int j = Math.Max(0, y-1); j <= Math.Min(yLength-1, y+1); j++)
            {
                Debug.Log(index);
                if (i != x && j != y)
                {
                    GameObject neighbor = grid[i, j];
                    neighbors[index] = neighbor;
                }
                index++;
            }
        }
        return neighbors;
    }
}
