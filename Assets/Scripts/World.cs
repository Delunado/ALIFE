using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    int width;
    public int Width { get => width; set => width = value; }

    int height;
    public int Height { get => height; set => height = value; }

    private List<List<Tile>> worldTileMatrix;
    private List<Tile> walkableTileList;

    public World(int width, int height)
    {
        Width = width;
        Height = height;

        //Here we initialize the worldTileMatrix list
        worldTileMatrix = new List<List<Tile>>();

        walkableTileList = new List<Tile>();
    }

    public void CreateWorld()
    {
        InitializeWorldTileMatrix();

        GameObject worldTilesObject = new GameObject("WorldTiles");

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Random.Range(0, 2) == 0)
                    CreateTile(i, j, TileType.GROUND, worldTilesObject);
                else
                    CreateTile(i, j, TileType.WALL, worldTilesObject);
            }
        }

        SetWalkableTileList();
        SetTileNeighbours();
    }

    private void SetWalkableTileList()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (worldTileMatrix[i][j].CanWalk)
                {
                    walkableTileList.Add(worldTileMatrix[i][j]);
                }
            }
        }
    }

    /// <summary>
    /// Initialize the World Tile Matrix using the actual Width
    /// </summary>
    private void InitializeWorldTileMatrix()
    {
        worldTileMatrix.Clear();

        for (int i = 0; i < Width; i++)
        {
            worldTileMatrix.Add(new List<Tile>());
        }
    }

    public void EraseWorld()
    {
        GameObject.Destroy(GameObject.Find("WorldTiles"));

        walkableTileList.Clear();
        InitializeWorldTileMatrix();
    }

    public void SetWorldSize(int newWidth, int newHeight)
    {
        Width = newWidth;
        Height = newHeight;

        InitializeWorldTileMatrix();
    }

    /// <summary>
    /// Returns a Random Not-Occupied Walkable Tile
    /// </summary>
    /// <returns></returns>
    public Tile GetRandomWalkableTile()
    {
        Tile tileToReturn = null;
        int random;
        int timesToTry = walkableTileList.Count * 2;

        do
        {
            random = Random.Range(0, walkableTileList.Count);
            timesToTry--;
        } while ((walkableTileList[random].IsOccupied || walkableTileList[random].HasFood) && timesToTry > 0);

        if (!(walkableTileList[random].IsOccupied))
        {
            tileToReturn = walkableTileList[random];
        }

        return tileToReturn;
    }

    public Tile GetTile(int x, int y)
    {
        return worldTileMatrix[x][y];
    }

    private void SetTileNeighbours()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Tile actualTile = worldTileMatrix[i][j];

                //If there is North Neigbour
                if (j - 1 >= 0)
                    actualTile.Neighbours[(int)TileNeighbours.NORTH] = GetTile(i, j - 1);

                //If there is South Neigbour
                if (j + 1 < Height)
                    actualTile.Neighbours[(int)TileNeighbours.SOUTH] = GetTile(i, j + 1);

                //If there is West Neigbour
                if (i - 1 >= 0)
                    actualTile.Neighbours[(int)TileNeighbours.WEST] = GetTile(i - 1, j);

                //If there is East Neigbour
                if (i + 1 < Width)
                    actualTile.Neighbours[(int)TileNeighbours.EAST] = GetTile(i + 1, j);
            }
        }
    }

    /*---------------------TILE CONFIGURATION-------------------------------*/

    /// <summary>
    /// Creates a tile of the given type in the given coordinates.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="tileType"></param>
    private void CreateTile(int posX, int posY, TileType tileType, GameObject parent)
    {
        //We create the GO and set it in the world space
        GameObject tileObject = new GameObject("Tile" + posX + "," + posY);
        tileObject.transform.parent = parent.transform;
        tileObject.transform.position = new Vector2(posX, posY);      

        //We add the Tile to the World Tile Matrix and set the Type and Sprite
        switch (tileType)
        {
            case TileType.GROUND:
                worldTileMatrix[posX].Add(tileObject.AddComponent<GroundTile>());
                ConfigTileSpriteRenderer("Sprites/Ground", tileObject);
                break;
            case TileType.WALL:
                worldTileMatrix[posX].Add(tileObject.AddComponent<WallTile>());
                ConfigTileSpriteRenderer("Sprites/Wall", tileObject);
                break;
        }
    }

    /// <summary>
    /// Config the sprite renderer for a specific tile.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="tileToApplyAt"></param>
    private void ConfigTileSpriteRenderer(string path, GameObject tileToApplyAt)
    {
        SpriteRenderer spriteRenderer = new SpriteRenderer();
        tileToApplyAt.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }
}
