using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager
{
    private GameObject foodGO;

    private List<GameObject> foodList;

    public FoodManager(GameObject _foodGO)
    {
        foodList = new List<GameObject>();
        foodGO = _foodGO;
    }

    public void CreateFood(Tile creationTile)
    {
        if (creationTile == null)
        {
            Debug.Log("ERROR: CAN'T CREATE FOOD. NULL CREATION TILE.");
            return;
        }

        GameObject foodGOAux = GameObject.Instantiate(foodGO, creationTile.transform.position, Quaternion.identity);
        foodGOAux.GetComponent<Food>().ActualTile = creationTile;
        foodGOAux.GetComponent<Food>().FoodManager = this;

        creationTile.HasFood = true;

        foodList.Add(foodGOAux);
    }

    public int TotalFoodNumber()
    {
        return foodList.Count;
    }

    public void EraseFood()
    {
        foreach (GameObject f in foodList)
        {
            GameObject.Destroy(f);
        }

        foodList.Clear();
    }

    public void EraseFood(GameObject foodGO)
    {
        foodList.Remove(foodGO);
    }
}
