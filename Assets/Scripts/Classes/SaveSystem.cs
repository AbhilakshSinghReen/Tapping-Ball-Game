using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{ 
    public static void Save(PlayerData Data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string SavePath = Application.persistentDataPath + "/player.bat";
        FileStream stream = new FileStream(SavePath, FileMode.Create);

        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public static PlayerData Load()
    {
        string SavePath = Application.persistentDataPath + "/player.bat";

        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Open);

            PlayerData Data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return Data;
        }
        else
        {
            Debug.LogError("Save file not found");
            PlayerData Data = new PlayerData(1, true, true, 1, 0, 0);
            Save(Data);
            return Data;
        }
    }

    public static void SaveSensitivity(int NewSensitivity)
    {
        PlayerData OldSave = Load();
        OldSave.Sensitivity = NewSensitivity;
        Save(OldSave);
    }
    public static void SaveSoundOn(bool SoundOn)
    {
        PlayerData OldSave = Load();
        OldSave.SoundOn = SoundOn;
        Save(OldSave);
    }
    public static void SaveMusicOn(bool MusicOn)
    {
        PlayerData OldSave = Load();
        OldSave.MusicOn = MusicOn;
        Save(OldSave);
    }
    public static void SaveCurrentLevel(int NewCurrentlevel)
    {
        PlayerData OldSave = Load();
        OldSave.CurrentLevel = NewCurrentlevel;
        Save(OldSave);
    }
    public static void SaveNumberOfKeys(int NewNumberOfKeys)
    {
        PlayerData OldSave = Load();
        OldSave.NumberOfKeys = NewNumberOfKeys;
        Save(OldSave);
    }
    public static void SaveInfiniteHighScore(int NewInfHighScore)
    {
        PlayerData OldSave = Load();
        OldSave.InfiniteHighScore = NewInfHighScore;
        Save(OldSave);
    }
}
