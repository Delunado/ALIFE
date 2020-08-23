using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private World world;
    private Poblation poblation;
    private FoodManager food;

    [SerializeField] private int worldWidth;
    [SerializeField] private int worldHeight;

    [SerializeField] private GameObject individualGO;
    [SerializeField] private GameObject foodGO;

    void Start()
    {
        world = new World(worldWidth, worldHeight);
        world.CreateWorld();

        poblation = new Poblation(individualGO);
        for (int i = 0; i < 1; i++)
        {
            poblation.CreateIndividual(world.GetRandomWalkableTile(), worldWidth, worldHeight);
        }

        food = new FoodManager(foodGO);

        ConfigCamera();

        InvokeRepeating("IncreaseHunger", 0.5f, 1.0f);
    }  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetWorld();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            food.CreateFood(world.GetRandomWalkableTile());
        }

        if (food.TotalFoodNumber() < 5)
        {
            food.CreateFood(world.GetRandomWalkableTile());
        }

        
    }

    private void ResetWorld()
    {
        //We erase the world
        world.EraseWorld();
        poblation.ErasePoblation();

        //We re-create the world
        world.SetWorldSize(Random.Range(3,18), Random.Range(3, 18));
        world.CreateWorld();

        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            poblation.CreateIndividual(world.GetRandomWalkableTile(), world.Width, world.Height);
        }

        food.EraseFood();

        ConfigCamera();

        CancelInvoke();
        InvokeRepeating("IncreaseHunger", 0.5f, 1.0f);
    }

    private void ConfigCamera()
    {
        Camera.main.transform.position = new Vector3(world.Width / 2, world.Height / 2, Camera.main.transform.position.z);
        //Camera.main.orthographicSize = (world.Height * world.Height) / 5;
    }

    private void IncreaseHunger()
    {
        EventManager.TriggerEvent(EventsNames.increaseHunger);
    }


}
