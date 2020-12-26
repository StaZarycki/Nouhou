using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Destination
{
    [Range(0.1f, 2f)] public float Speed = 0.5f;
    public Vector3 Position;
    public bool AttackOnFinish = false;
    private float tolerance;

    private float map(float value, float min, float max, float minMap, float maxMap)
    {
        return (value - min)/(max-min)*(maxMap-minMap)+minMap;
    }

    public float Tolerance
    {
        get
        {
            tolerance = map(Speed, 0.1f, 2f, 0.25f, 0.01f);
            return tolerance;
        }
    }
}
