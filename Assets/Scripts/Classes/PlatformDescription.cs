using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDescription
{
    public Vector3 SpawnPosition;
    public Vector3 SpawnScale;
    public string Color;
    public bool IsControllable;

    public PlatformDescription(Vector3 spawnPos, Vector3 spawnScale, string color,bool IsPlatformControllable)
    {
        SpawnPosition = spawnPos;
        SpawnScale = spawnScale;
        Color = color;
        IsControllable = IsPlatformControllable;
    }
}
