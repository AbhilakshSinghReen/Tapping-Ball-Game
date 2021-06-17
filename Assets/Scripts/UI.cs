using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;


public class UI : MonoBehaviour
{
    public GameObject InfoMenu;

    public Player player;

    public LevelDescription CurrentLevel;

    public Slider SensitivitySlider;
    
    public GameObject PauseMenu;

    public TMP_Text SensitivityDisplayerText;

    public TMP_Text CompletionText;

    public bool IsGamePaused;

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

        IsGamePaused = false;

        UnPauseGame();

        PlayerData OldData = SaveSystem.Load();

        MusicOn = OldData.MusicOn;
        SoundOn = OldData.SoundOn;

        if (!MusicOn)
        {
            MusicOnButton.image.color = FadedButtonColor;
            audioManager.MuteMusic(true);
        }
        if (!SoundOn)
        {
            SoundOnButton.image.color = FadedButtonColor;
        }

        SensitivitySlider.value = OldData.Sensitivity;

        //  Debug.Log("Old sens = " + OldData.Sensitivity.ToString());

        OnSensitivityChanged(OldData.Sensitivity);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("MainGame");
        UnPauseGame();
    }

    public void PauseButton()
    {
        if (IsGamePaused)
        {
            UnPauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;

        CurrentLevel = player.CurrentLevel;

        CompletionText.text = "LEVEL " + CurrentLevel.LevelNumber.ToString() + "\n" + CompletionText.text;
    }
    void UnPauseGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;

        CompletionText.text = Mathf.RoundToInt(player.CompletionPercentage).ToString() + "%";
    }

    public void OnSensitivityChanged(float NewSensitivity)
    {
        ModeClass.Sensitivity = NewSensitivity;

        player.OnNewSensitivity(NewSensitivity);

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

    public void QuitToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        UnPauseGame();
    }

    public void RetryButton()
    {
        UnPauseGame();

        player.Retry();
    }

    public void ContinueButton()
    {

    }

    public void NextButton()
    {
        SceneManager.LoadScene("MainGame");
        player.PlayInterstitialAdOnRandomBasis();
    }

    public void DisplayInfo()
    {
        PauseGame();
        InfoMenu.SetActive(true);
    }

}
