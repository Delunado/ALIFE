using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poblation
{
    private List<Individual> individualList;

    GameObject individualGO;

    public Poblation(GameObject _individualGO)
    {
        individualGO = _individualGO;
        individualList = new List<Individual>();
    }

    public Individual CreateIndividual(Tile creationTile, int worldWidth, int worldHeight)
    {
        if (creationTile == null)
        {
            Debug.LogError("ERROR: CAN'T CREATE INDIVIDUAL. NULL CREATION TILE.");
            return null;
        }

        GameObject individualGOAux = GameObject.Instantiate(individualGO, creationTile.transform.position, Quaternion.identity);
        Individual individual = individualGOAux.GetComponent<Individual>();

        individual.ActualTile = creationTile;
        individual.ActualTile.IsOccupied = true;

        individual.NextTile = creationTile;

        individual.Poblation = this;

        individual.GenerateWorldMovementMatrix(worldWidth, worldHeight);

        individual.WorldMovementMatrix[(int)creationTile.transform.position.x, (int)creationTile.transform.position.y]++;

        individualList.Add(individual); //We add it to the individuals list

        return individual;
    }

    public void ErasePoblation()
    {
        foreach (Individual i in individualList)
        {
            GameObject.Destroy(i.gameObject);
        }

        individualList.Clear();
    }

    public void ErasePoblation(Individual individual)
    {
        individualList.Remove(individual);
    }

}
