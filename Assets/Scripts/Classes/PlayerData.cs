using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int Sensitivity;
    public bool SoundOn;
    public bool MusicOn;

    public int CurrentLevel;
    public int NumberOfKeys;

    public int InfiniteHighScore;


    public PlayerData(int sens,bool sound,bool music,int CurrLvl,int NoOfKeys,int InfHighScore)
    {
        Sensitivity = sens;
        SoundOn = sound;
        MusicOn = music;
        CurrentLevel = CurrLvl;
        NumberOfKeys = NoOfKeys;
        InfiniteHighScore = InfHighScore;
    }

}
