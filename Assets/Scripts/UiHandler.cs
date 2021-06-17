using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UiHandler : MonoBehaviour
{
    public Slider SensitivitySlider;

    public TMP_Text SensitivityDisplayerText;

    public bool MusicOn, SoundOn;

    public Button MusicOnButton;
    public Button SoundOnButton;

    AudioManager audioManager;

    Color FadedButtonColor, UnfadedButtonColor;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        FadedButtonColor = new Color(60f, 60f, 60f, .5f);
        UnfadedButtonColor = new Color(255f, 255f, 255f, 1f);

        PlayerData OldData = SaveSystem.Load();

        MusicOn = OldData.MusicOn;
        SoundOn = OldData.SoundOn;

        if (!MusicOn)
        {
            MusicOnButton.image.color = FadedButtonColor;
        }
        if (!SoundOn)
        {
            SoundOnButton.image.color = FadedButtonColor;
        }

        SensitivitySlider.value = OldData.Sensitivity;

        Debug.Log("Old sens = " + OldData.Sensitivity.ToString());

        OnSensitivityChanged(OldData.Sensitivity);
    }



    public void PlayInfiniteButon()
    {
        ModeClass.GameMode = "Infinite";
        SceneManager.LoadScene("MainGame");

    }
    public void PlayLevelsButton()
    {
        ModeClass.GameMode = "Levels";
        SceneManager.LoadScene("MainGame");
    }



    public void OnSensitivityChanged(float NewSensitivity)
    {
        ModeClass.Sensitivity = NewSensitivity;

        SensitivityDisplayerText.text = NewSensitivity.ToString() + "x";

        SaveSystem.SaveSensitivity(Mathf.RoundToInt(NewSensitivity));
    }
    public void ToggleMusicButton()
    {
        MusicOn = !MusicOn;

        audioManager.MuteMusic(!MusicOn);

        audioManager.MusicOn = MusicOn;

        SaveSystem.SaveMusicOn(MusicOn);

        if (MusicOn)
        {
            MusicOnButton.image.color = UnfadedButtonColor;
        }
        else
        {
            MusicOnButton.image.color = FadedButtonColor;
        }

    }

    public void ToggleSoundButton()
    {
        SoundOn = !SoundOn;

        audioManager.SoundOn = SoundOn;

        SaveSystem.SaveSoundOn(SoundOn);

        if (SoundOn)
        {
            SoundOnButton.image.color = UnfadedButtonColor;
        }
        else
        {
            SoundOnButton.image.color = FadedButtonColor;
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
