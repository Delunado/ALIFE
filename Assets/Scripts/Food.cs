using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Tile actualTile;
    public Tile ActualTile { get => actualTile; set => actualTile = value; }

    private FoodManager foodManager;
    public FoodManager FoodManager { get => foodManager; set => foodManager = value; }

    private float nutrient = 2.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Individual>())
        {
            collision.GetComponent<Individual>().Eat();

            actualTile.HasFood = false;

            foodManager.EraseFood(gameObject);
            Destroy(gameObject);
        }
    }
}
