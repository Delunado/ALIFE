using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    GROUND = 0,
    WALL = 1
}

public enum TileNeighbours
{
    NORTH = 0,
    SOUTH = 1,
    WEST = 2,
    EAST = 3
}

abstract public class Tile : MonoBehaviour
{
    [SerializeField] private bool seeNeighbours = false;

    protected bool canWalk;
    public bool CanWalk { get => canWalk; set => canWalk = value; }

    protected bool isOccupied;
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }

    private bool hasFood;
    public bool HasFood { get => hasFood; set => hasFood = value; }

    protected TileType tileType;

    Tile[] neighbours = { null, null, null, null };
    public Tile[] Neighbours { get => neighbours; set => neighbours = value; }

    public Tile GetRandomNeighbour()
    {
        Tile randomNeighbour = this;
        int random;
        int timesToTry = 10; 

        do
        {
            random = Random.Range(0, neighbours.Length);
            timesToTry--;
        } while ((neighbours[random] == null || neighbours[random].IsOccupied) && timesToTry > 0);

        if ((neighbours[random] != null && !neighbours[random].IsOccupied))
        {
            randomNeighbour = neighbours[random];
        }

        return randomNeighbour;
    }

    private void SeeNeighbours()
    {
        foreach (Tile t in Neighbours)
        {
            if (t != null)
            {
                t.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void Update()
    {
        if (seeNeighbours)
        {
            seeNeighbours = false;
            SeeNeighbours();
        }
    }
}


