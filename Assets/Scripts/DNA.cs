using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    float speed;
    float hunger;
    float metabolism;

    public float Speed { get => speed; set => speed = value; }
    public float Hunger { get => hunger; set => hunger = value; }
    public float Metabolism { get => metabolism; set => metabolism = value; }

    public DNA(float speed, float hunger, float metabolism)
    {
        Speed = speed;
        Hunger = hunger;
        Metabolism = metabolism;
    }
}
