using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : Tile
{
    public GroundTile()
    {
        IsOccupied = false;
        CanWalk = true;
        tileType = TileType.GROUND;
        HasFood = false;
    }
}
