using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDescription 
{
    public int LevelNumber;
    
    public float LevelMinY;
    public float LevelWinY;

    public float LevelOrthoWidth;

    public List<PlatformDescription> LevelPlatforms;

    public LevelDescription(int LvlNum,float MinY,float WinY,float OrthoWidth)
    {
        LevelNumber = LvlNum;
        LevelMinY = MinY;
        LevelWinY = WinY;
        LevelOrthoWidth = OrthoWidth;
        LevelPlatforms = new List<PlatformDescription>();
    }
}
