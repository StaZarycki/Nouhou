using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Destination
{
    public Vector3 position;
    public float speed;
    private float tolerance;

    private float map(float value, float min, float max, float minMap, float maxMap)
    {
        return (value - min)/(max-min)*(maxMap-minMap)+minMap;
    }

    public float Tolerance
    {
        get
        {
            tolerance = map(speed, 0.01f, 0.2f, 0.25f, 0.01f);
            return tolerance;
        }
    }
}
