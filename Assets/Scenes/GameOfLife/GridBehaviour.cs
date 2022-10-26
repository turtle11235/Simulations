using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Debug = UnityEngine.Debug;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GridBehaviour : MonoBehaviour
{
    public GameObject cellPrefab;
    public int length = 20;
    
    public float animationSpeed = .5f;
    public bool running = false;
    public bool random = false;

    private float elapsedTime = 0f;
    private GameObject[,] grid;
    private const int maxLength = 10;

    // Start is called before the first frame update
    void Start()
    {
        initializeGrid(random);
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

    void initializeGrid(bool random=false)
    {
        float cellLength = maxLength / (float)length;
        float startingPos = -(maxLength / 2f - cellLength / 2);
        grid = new GameObject[length, length];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(x*cellLength + startingPos, y*cellLength + startingPos, 0), Quaternion.identity);
                if (random)
                {
                    cell.GetComponent<CellBehaviour>().isAlive = UnityEngine.Random.Range(0f, 1f) > .5;
                }
                cell.transform.localScale = new Vector3(cellLength, cellLength, 1);
                grid[x, y] = cell;
            }
        }
    }

    void updateCells()
    {
        bool[,] nextValues = new bool[length, length];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                nextValues[x, y] = willLive(x, y);
            }
        }

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                grid[x, y].GetComponent<CellBehaviour>().isAlive = nextValues[x, y];
            }
        }
    }

    bool willLive(int x, int y)
    {
        bool isAlive = grid[x, y].GetComponent<CellBehaviour>().isAlive;
        int liveNeighborCount = 0;
        GameObject[] neighbors = getNeighbors(x, y);
        foreach(GameObject neighbor in neighbors)
        {
            if (neighbor != null && neighbor.GetComponent<CellBehaviour>().isAlive)
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
        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                int row = (i % length + length) % length;
                int col = (j % length + length) % length;
                if (!(i == x && j == y))
                {
                    GameObject neighbor = grid[row, col];
                    neighbors[index] = neighbor;
                }
                index++;
            }
        }
        return neighbors;
    }
}
