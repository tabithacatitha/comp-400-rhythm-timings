using System;
using UnityEngine;

[Serializable]
public class Beat
{
    public float judgementTime;
    public float spawnTime;

    public Beat(float judgementTime, float spawnTime)
    {
        this.judgementTime = judgementTime;
        this.spawnTime = spawnTime;
    }
}