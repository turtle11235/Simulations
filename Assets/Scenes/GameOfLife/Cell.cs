using System.Collections;
using UnityEngine;

public class Cell
{
    public readonly int x;
    public readonly int y;
    public bool isAlive = false;
    public readonly int index;

    public Cell(int x, int y, int index, bool isAlive = false)
    {
        this.x = x;
        this.y = y;
        this.isAlive = isAlive;
        this.index = index;
    }
}
