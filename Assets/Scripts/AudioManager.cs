using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    public static AudioManager instance;

    public bool SoundOn, MusicOn;

    void Start()
    {
        PlayerData OldData = SaveSystem.Load();

        MusicOn = true;
        SoundOn = true;

        PlaySound("Music1");
        Debug.Log("Playing Music1");
               
        MusicOn = OldData.MusicOn;
        SoundOn = OldData.SoundOn;
        
        if (!MusicOn)
        {
            MuteMusic(true);
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in Sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
            sound.Source.playOnAwake = false;
            sound.Source.spatialBlend = 1f;

            if (sound.Name == "Music1" || sound.Name == "Music2" || sound.Name == "Music3" || sound.Name == "Music4" || sound.Name == "Music5" || sound.Name == "Music6" || sound.Name == "Music7" || sound.Name == "Music8")
            {
                sound.Source.spatialBlend = 0f;
            }
        }
    }

    public void PlaySound(string SoundName)
    {
        if(SoundName == "Music1" || SoundName == "Music2" || SoundName == "Music3" || SoundName == "Music4" || SoundName == "Music5" || SoundName == "Music6" || SoundName == "Music7" || SoundName == "Music8")
        {
            if (MusicOn)
            {
                Debug.Log("PlayingMusic");
                Sound TargetSound = Array.Find(Sounds, sound => sound.Name == SoundName);
                if (TargetSound == null)
                {
                    Debug.LogWarning("Sound: " + SoundName + " not found");
                    return;
                }
                TargetSound.Source.Play();
            }
        }
        else
        {
            if (SoundOn)
            {
                Sound TargetSound = Array.Find(Sounds, sound => sound.Name == SoundName);
                if (TargetSound == null)
                {
                    Debug.LogWarning("Sound: " + SoundName + " not found");
                    return;
                }
                TargetSound.Source.Play();
            }
        }

        
    }

    public void MuteMusic(bool Mute)
    {
        foreach(Sound sound in Sounds)
        {
            if (sound.Name == "Music1" || sound.Name == "Music2" || sound.Name == "Music3" || sound.Name == "Music4" || sound.Name == "Music5" || sound.Name == "Music6" || sound.Name == "Music7" || sound.Name == "Music8")
            {
                sound.Source.mute = Mute;
            }
        }
    }
}
