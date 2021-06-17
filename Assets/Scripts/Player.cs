using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float DistanceToSwitchCameraToPlayer = 40f;

    bool IsPlayerOnScreen;

    public AdMob adMob;

    public float InfiniteModeLosingDistance = 15;

    public bool ModeLevels = true;
    public int Score = 0;

    public GameObject GameOverMenu;

    public GameObject LevelCompletedMenu;

    public GameObject PauseButton;
    public GameObject ResetButton;

    float TiltAngle = 0f;

    public float LoosingPercentage = 10f;

    int CurrentDirection = 0;
    /*
    1 = Case A
    2 = Case B
    3 = Case C
    4 = Case D
     */

    float DirectionSwitchingHeadingMargin = 15; //DEGREES

    

    public CameraScript MainCameraScript;

    public LevelDescription CurrentLevel;


    [HideInInspector]
    public float CompletionPercentage;

    public TMP_Text CompletionPercentageDisplayerText;


    public float Sensitivity = 1f;

    public int PlatformLayer = 8;
    public float RayCastDetectionDistance = 10f;

    public float UpwardsForce = 5f;

    [HideInInspector]
    public GameObject CurrentPlatform;

    public Text DebuggerText;

    public Rigidbody2D PlayerRigidbody;

    public ParticleSystem ForceParticleSystem;

    Quaternion NewBaseRotation;
    float Heading;

    public Vector2 ScreenBounds;
    float PlayerWidth, PlayerHeight;

    bool addUpwardsForce;
    int TouchCount;

    float StartY = 0f;

    bool HasWon;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        //SaveSystem.SaveCurrentLevel(1);

        audioManager = FindObjectOfType<AudioManager>();

        PlayerRigidbody = GetComponent<Rigidbody2D>();

        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        PlayerWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;
        PlayerHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;


        Debug.Log(ScreenBounds);

        EnableGyroAndCompass();
        NewBaseRotation = new Quaternion(1f, 1f, 0f, 0f);

        LevelCompletedMenu.SetActive(false);

        HasWon = false;
        IsPlayerOnScreen = true;
    }

    void GetNewCurrentDirection(float heading)
    {
        if (heading >= 45 && heading <= 135) //A
        {
            CurrentDirection = 1;
        }
        else if (heading > 135 && heading <= 225) //B
        {
            CurrentDirection = 2;
        }
        else if (heading > 225 && heading <= 315) //C
        {
            CurrentDirection = 3;
        }
        else if (heading > 315 || heading < 45) //D
        {
            CurrentDirection = 4;
        }

    }

    float ClampAngleWhileMaintainingRotation(float Angle)
    {
        if(Angle>180)
        {
            Angle -= 360;
        }
        if(Angle<=(-180))
        {
            Angle += 360;
        }

        return Angle;
    }

    // Update is called once per frame
    void Update()
    {
        TouchCount = Input.touchCount;

        if (Input.GetMouseButton(0))
        {
            TouchCount = 1;
        }

        if (Input.gyro.enabled)
        {
            Heading = Input.compass.magneticHeading;
            Quaternion GyroRotation = Input.gyro.attitude;
            GyroRotation = GyroRotation * NewBaseRotation;

            switch(CurrentDirection)
            {
                case 1:
                    {
                        TiltAngle = GyroRotation.eulerAngles.x * Sensitivity;
                        TiltAngle = ClampAngleWhileMaintainingRotation(TiltAngle);
                        
                        if(!(Heading >= 45 && Heading <= 135))
                        {
                            if(Heading<(45- DirectionSwitchingHeadingMargin))
                            {
                                GetNewCurrentDirection(Heading);
                            }
                            else if (Heading > (135 + DirectionSwitchingHeadingMargin))
                            {
                                GetNewCurrentDirection(Heading);
                            }
                        }                        
                        break;
                    }
                case 2:
                    {
                        TiltAngle = GyroRotation.eulerAngles.y * Sensitivity;
                        TiltAngle += 180;
                        TiltAngle = ClampAngleWhileMaintainingRotation(TiltAngle);
                        
                        
                        if (Heading < (135 - DirectionSwitchingHeadingMargin))
                        {
                            GetNewCurrentDirection(Heading);
                        }
                        else if (Heading > (225 + DirectionSwitchingHeadingMargin))
                        {
                            GetNewCurrentDirection(Heading);
                        }
                        break;
                    }
                case 3:
                    {
                        TiltAngle = -GyroRotation.eulerAngles.x * Sensitivity;
                        TiltAngle = ClampAngleWhileMaintainingRotation(TiltAngle);

                        
                        if (Heading < (225 - DirectionSwitchingHeadingMargin))
                        {
                            GetNewCurrentDirection(Heading);
                        }
                        else if (Heading > (315 + DirectionSwitchingHeadingMargin))
                        {
                            GetNewCurrentDirection(Heading);
                        }
                        break;
                    }
                case 4:
                    {
                        TiltAngle = -GyroRotation.eulerAngles.y * Sensitivity;
                        TiltAngle += 180;
                        TiltAngle = ClampAngleWhileMaintainingRotation(TiltAngle);
                        

                        if (Heading < (315 - DirectionSwitchingHeadingMargin))
                        {
                            GetNewCurrentDirection(Heading);
                        }
                        else if (Heading > (45 + DirectionSwitchingHeadingMargin))
                        {
                            GetNewCurrentDirection(Heading);
                        }
                        break;
                    }
                default:
                    {
                        GetNewCurrentDirection(Heading);
                        break;
                    }
            }

            CurrentPlatform.transform.rotation = Quaternion.Euler(0f, 0f, TiltAngle);

            //CompletionPercentageDisplayerText.text = "Head= " + Mathf.RoundToInt(Heading).ToString() + "\nTilt= " + TiltAngle.ToString("F2");

        }
        else
        {
            if (SystemInfo.supportsGyroscope)
            {
                EnableGyroAndCompass();
            }
            else
            {
                DebuggerText.text = "No Gyro available";
            }
        }
        CheckForCompleteOrLoss();
    }
    void LateUpdate()
    {
        Vector3 ViewPos = transform.position;
        if (ScreenBounds.x < 0)
        {
            ViewPos.x = Mathf.Clamp(ViewPos.x, ScreenBounds.x + PlayerWidth / 2, -ScreenBounds.x - PlayerWidth / 2);
        }
        else
        {
            ViewPos.x = Mathf.Clamp(ViewPos.x, -ScreenBounds.x + PlayerWidth / 2, ScreenBounds.x - PlayerWidth / 2);
        }
        if (ScreenBounds.y < 0)
        {
            ViewPos.y = Mathf.Clamp(ViewPos.y, ScreenBounds.y + PlayerHeight / 2, -ScreenBounds.y - PlayerHeight / 2);
        }
        else
        {
            ViewPos.y = Mathf.Clamp(ViewPos.y, -ScreenBounds.y + PlayerHeight / 2, ScreenBounds.y - PlayerHeight / 2);
        }
    }
    void FixedUpdate()
    {
        SetCurrentPlatform();
        AddUpwardsForce();
        SetCameraPosition();       
        if((int)transform.position.y>Score)
        {
            Score = (int)transform.position.y;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        audioManager.PlaySound("BounceSound");
    }

    void SetCameraPosition()
    {
        if (!HasWon)
        {
            float DistanceFromCurrentPlatform = Mathf.Abs(transform.position.y - CurrentPlatform.transform.position.y);

            if (DistanceFromCurrentPlatform > DistanceToSwitchCameraToPlayer)
            {
                MainCameraScript.DesiredCameraPosition = new Vector3(MainCameraScript.DesiredCameraPosition.x, transform.position.y, MainCameraScript.DesiredCameraPosition.z);
            }
            else
            {
                MainCameraScript.DesiredCameraPosition = new Vector3(MainCameraScript.DesiredCameraPosition.x, CurrentPlatform.transform.position.y, MainCameraScript.DesiredCameraPosition.z);
            }
        }
        else
        {
            MainCameraScript.DesiredCameraPosition = new Vector3(transform.position.x, transform.position.y, MainCameraScript.DesiredCameraPosition.z);
        }
    }

    void AddUpwardsForce()
    {
        PlayerRigidbody.AddForce(Vector2.up * TouchCount * UpwardsForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        if (TouchCount > 0)
        {
            ForceParticleSystem.Play();
        }
        else
        {
            ForceParticleSystem.Pause();
            ForceParticleSystem.Clear();
        }
        DebuggerText.text += "Adding force";
    }

    void EnableGyroAndCompass()
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;
    }

    public void SetCurrentPlatform()
    {
        if(!HasWon)
        {
            RaycastHit2D Hit;

            Hit = Physics2D.Raycast(transform.position, Vector2.down, RayCastDetectionDistance, 1 << LayerMask.NameToLayer("Platform"));
            Debug.DrawRay(transform.position, Vector2.down, Color.green);

            if (Hit.collider != null)
            {
                if (CurrentPlatform != Hit.transform.gameObject)
                {
                    if (Hit.transform.gameObject.GetComponent<Platform>().IsControllable)
                    {
                        if (CurrentPlatform != null)
                        {
                            CurrentPlatform.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                        CurrentPlatform = Hit.transform.gameObject;
                        OnNewPlatform();
                    }
                }
            }
        }             
    }

    void OnNewPlatform()
    {
        MainCameraScript.DesiredCameraPosition = new Vector3(MainCameraScript.DesiredCameraPosition.x, CurrentPlatform.transform.position.y, MainCameraScript.DesiredCameraPosition.z);
    }


    public void OnNewSensitivity(float NewSensitivity)
    {
        Sensitivity = NewSensitivity;
    }

    void InitialiseLevel()
    {

    }

    void CheckForCompleteOrLoss()
    {
        if (ModeLevels)
        {
            if (HasWon)
            {
                CompletionPercentageDisplayerText.text = "LEVEL COMPLETED";
            }
            else
            {
                float cP = (transform.position.y - StartY) * 100 / (CurrentLevel.LevelWinY - StartY);
                if (cP > CompletionPercentage && !HasWon)
                {
                    CompletionPercentage = cP;
                    CompletionPercentageDisplayerText.text = Mathf.RoundToInt(CompletionPercentage).ToString() + "%";
                }

                if (CompletionPercentage >= 100)
                {
                    OnVictory();
                    return;
                }

                if (CompletionPercentage - cP > LoosingPercentage)
                {
                    OnDefeat();
                }
            }
        }
        else
        {
            CompletionPercentageDisplayerText.text = "Score: " + Score.ToString();


            if (Score - transform.position.y > InfiniteModeLosingDistance)
            {
                OnDefeat();
            }
        }
    }

    void OnDefeat()
    {
        CompletionPercentageDisplayerText.text = "YOU LOST";
        Time.timeScale = 0f;
        GameOverMenu.SetActive(true);
    }

    void OnVictory()
    {
        if (!HasWon && ModeLevels)
        {
            CompletionPercentageDisplayerText.text = "LEVEL COMPLETED";

            PlayerRigidbody.gravityScale = 0f;
            ForceParticleSystem.Play();

            CurrentLevel.LevelNumber += 1;

            SaveSystem.SaveCurrentLevel(CurrentLevel.LevelNumber);

            ResetButton.SetActive(false);
            PauseButton.SetActive(false);
            LevelCompletedMenu.SetActive(true);
            HasWon = true;
        }        
    }

    public void Retry()
    {
        GameOverMenu.SetActive(false);
        transform.position = new Vector2(0f, 2f);
        PlayerRigidbody.velocity = new Vector2(0f, 0f);
        CompletionPercentage = 0f;
    }

    public void PlayInterstitialAdOnRandomBasis()
    {
        PlayInterstitialAd();        
    }

    public void PlayInterstitialAd()
    {
        adMob.ShowInterstitialAd();
    }
}