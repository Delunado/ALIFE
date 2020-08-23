using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Individual : MonoBehaviour
{
    private Tile actualTile;
    public Tile ActualTile { get => actualTile; set => actualTile = value; }

    private Tile nextTile;
    public Tile NextTile { get => nextTile; set => nextTile = value; }

    private Poblation poblation;
    public Poblation Poblation { get => poblation; set => poblation = value; }

    private DNA dna;

    private float actualHunger;
    public float ActualHunger { get => actualHunger; set => actualHunger = value; }

    private UnityAction listenerIncreaseHunger;

    //This Matrix indicates how many times the individual has moved to each tile 
    int[,] worldMovementMatrix;
    public int[,] WorldMovementMatrix { get => worldMovementMatrix; set => worldMovementMatrix = value; }

    private void Awake()
    {
        listenerIncreaseHunger = new UnityAction(IncreaseHunger);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventsNames.increaseHunger, listenerIncreaseHunger);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsNames.increaseHunger, listenerIncreaseHunger);
    }

    private void Start()
    {
        dna = new DNA(Random.Range(1f, 5f), Random.Range(5f, 20f), Random.Range(0.1f, 1.0f));
        ActualHunger = dna.Hunger;

        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    public void GenerateWorldMovementMatrix(int width, int height)
    {
        WorldMovementMatrix = new int[width, height]; 

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                WorldMovementMatrix[i, j] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.Equals(NextTile.transform.position))
        {
            ActualTile.IsOccupied = false;
            NextTile.IsOccupied = true;
            ActualTile = NextTile;
            WorldMovementMatrix[(int)ActualTile.transform.position.x, (int)ActualTile.transform.position.y]++;
            /*RANDOM MODE
             * NextTile = ActualTile.GetRandomNeighbour();
             * */

            /*EXPLORATION MODE*/
            List<Tile> lessExploredTiles = new List<Tile>();

            //Here we get the less explored neighbour
            int index;
            Tile lessExploredTile = null;
            for (int i = 0; i < ActualTile.Neighbours.Length; i++)
            {
                if (ActualTile.Neighbours[i] != null && ActualTile.Neighbours[i].CanWalk)
                {
                    lessExploredTile = ActualTile.Neighbours[i];
                    index = i;
                    break;
                }
            }

            if (lessExploredTile != null)
            {

                for (int i = 0; i < ActualTile.Neighbours.Length; i++)
                {
                    if (ActualTile.Neighbours[i] != null && ActualTile.Neighbours[i].CanWalk)
                    {
                        if (WorldMovementMatrix[(int)ActualTile.Neighbours[i].transform.position.x,
                            (int)ActualTile.Neighbours[i].transform.position.y]
                            <
                            WorldMovementMatrix[(int)lessExploredTile.transform.position.x,
                            (int)lessExploredTile.transform.position.y])
                        {
                            index = i;
                            lessExploredTile = ActualTile.Neighbours[i];
                        }
                    }
                }

                //Here we get all the similar explored neighbours
                for (int i = 0; i < ActualTile.Neighbours.Length; i++)
                {
                    if (ActualTile.Neighbours[i] != null && ActualTile.Neighbours[i].CanWalk)
                    {
                        if (WorldMovementMatrix[(int)ActualTile.Neighbours[i].transform.position.x,
                                (int)ActualTile.Neighbours[i].transform.position.y]
                                ==
                                WorldMovementMatrix[(int)lessExploredTile.transform.position.x,
                                (int)lessExploredTile.transform.position.y])
                        {
                            lessExploredTiles.Add(ActualTile.Neighbours[i]);
                        }
                    }
                }

                NextTile = lessExploredTiles[Random.Range(0, lessExploredTiles.Count)];
            } else
            {
                NextTile = ActualTile;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, NextTile.transform.position, dna.Speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Restore the individual hunger.
    /// </summary>
    public void Eat()
    {
        actualHunger = dna.Hunger;
        transform.localScale = new Vector2(1.0f, 1.0f);
    }

    private void IncreaseHunger()
    {
        actualHunger -= dna.Metabolism;

        float hungerRatio = actualHunger / dna.Hunger;
        if (hungerRatio > 0.2f)
            transform.localScale = new Vector2(hungerRatio, hungerRatio);
        else
            transform.localScale = new Vector2(0.2f, 0.2f);

        //Kill individual if hunger is 0
        if (Mathf.Approximately(ActualHunger, 0f) || ActualHunger < 0f)
        {
            poblation.ErasePoblation(this);
            Destroy(gameObject);
        }
    }
}
