using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformManager : MonoBehaviour
{
    public UI InGameUI;

    public string Mode = "";

    public GameObject PlatformPrefab;
    public GameObject Player;

    float MaximumAllowedVerticalDistanceBetweenPlatforms = 50f;
    float LevelNumberClearanceFactor = 3.3f;

    List<GameObject> ActivePlatforms;

    Quaternion SpawnRotation = Quaternion.Euler(0f, 0f, 0f);

    public float OrthoWidth = 25f;
    public float PlatformWidthIsToHeightRatio = 8f;
    public float PlatformWidth = 8f;

    public Text YouWonText;

    [HideInInspector]
    public LevelDescription CurrentLevel;



    //VARIABLES FOR HANDLING INFINITE MODE
    float GenerateY, CleanUpY, LastGeneratedY, GenerateClearance;

    float PlayerForwardMargin;
    float PlayerRearMargin;

    List<GameObject> CleanUpPlatforms;
    List<GameObject> NewPlatforms;

    List<GameObject> SidePlatforms;



    // Start is called before the first frame update
    void Start()
    {
        PlayerData OldData = SaveSystem.Load();

        Mode = ModeClass.GameMode;

        ActivePlatforms = new List<GameObject>();

        PlatformDescription Platform1Description = new PlatformDescription(new Vector3(0f, 0f, 0f), new Vector3(1f, .5f, 1f), "Yellow", true);
        SpawnPlatform(Platform1Description);

        if (Mode == "Infinite")
        {
            Debug.Log("Infinite mode");
            Player.GetComponent<Player>().ModeLevels = false;
            
            CleanUpPlatforms = new List<GameObject>();
            NewPlatforms = new List<GameObject>();
            SidePlatforms = new List<GameObject>();
            LastGeneratedY = 0f;
            GenerateClearance = 16f;            

            GenerateInfiniteLevel();
        }
        else
        {
            Player.GetComponent<Player>().ModeLevels = true;

            GenerateLevelByNumber(OldData.CurrentLevel);        

            Player.GetComponent<Player>().CurrentPlatform = ActivePlatforms[0];
        }


        if (CurrentLevel.LevelNumber == 1 || CurrentLevel.LevelNumber <= 0)
        {
            OnFirstGame();
        }

        PlayerRearMargin = 2 * Player.GetComponent<Player>().InfiniteModeLosingDistance;
    }

    void FixedUpdate()
    {
        /*
        if(Mode == "Infinite")
        {
            int i;
            Vector3 PlayerPosition = Player.transform.position;

            GenerateY = PlayerPosition.y + PlayerForwardMargin;
            CleanUpY = PlayerPosition.y - PlayerRearMargin;

            for(i=ActivePlatforms.Count-1;i>=0;i--)
            {
                if(ActivePlatforms[i].transform.position.y<CleanUpY)
                {
                    GameObject.Destroy(ActivePlatforms[i]);
                    ActivePlatforms.RemoveAt(i);
                }
            }

            if((GenerateY-LastGeneratedY)>GenerateClearance)
            {
                //Spawn new platform here
                //Move Level side boundaries up

                float MinClearance = 10 + Mathf.Pow(PlayerPosition.y, 0.1f);
                float MaxClearance = 35 + Mathf.Pow(PlayerPosition.y, 0.33f);
                GenerateClearance = Random.Range(MinClearance, MaxClearance);
            }


        }
        */
    }

    void GenerateInfiniteLevel()
    {
        float SidePlatformHieght;
        PlatformDescription LeftSidePlatformDescription,RightSidePlatformDescription;
        GameObject LeftSidePlatform, RightSidePlatform;

        SidePlatformHieght = 200f;


        LeftSidePlatformDescription = new PlatformDescription(new Vector3(OrthoWidth / 2, 0f, 0f), new Vector3(1 / PlatformWidthIsToHeightRatio, SidePlatformHieght, 1f), "White", false);


        LeftSidePlatform = Instantiate(PlatformPrefab, LeftSidePlatformDescription.SpawnPosition, SpawnRotation);
        LeftSidePlatform.transform.localScale = LeftSidePlatformDescription.SpawnScale;

        LeftSidePlatform.GetComponent<Platform>().IsControllable = LeftSidePlatformDescription.IsControllable;
        LeftSidePlatform.GetComponent<Platform>().InitialisePlatform(LeftSidePlatformDescription.Color);
        LeftSidePlatform.layer = 8;

        SidePlatforms.Add(LeftSidePlatform);


        RightSidePlatformDescription = new PlatformDescription(new Vector3(-OrthoWidth / 2, 0f, 0f), new Vector3(1 / PlatformWidthIsToHeightRatio, SidePlatformHieght, 1f), "White", false);

        RightSidePlatform = Instantiate(PlatformPrefab, RightSidePlatformDescription.SpawnPosition, SpawnRotation);
        RightSidePlatform.transform.localScale = RightSidePlatformDescription.SpawnScale;

        RightSidePlatform.GetComponent<Platform>().IsControllable = RightSidePlatformDescription.IsControllable;
        RightSidePlatform.GetComponent<Platform>().InitialisePlatform(RightSidePlatformDescription.Color);
        RightSidePlatform.layer = 8;

        SidePlatforms.Add(RightSidePlatform);
    }

    void GenerateLevelByNumber(int LvlNum)
    {
        if (LvlNum != 0)
        {
            CurrentLevel = new LevelDescription(LvlNum, 0, 0, 0);
        }
        else
        {
            CurrentLevel = new LevelDescription(1, 0, 0, 0);
        }

        int LevelMaxY = Mathf.RoundToInt(100 + Mathf.Pow(LvlNum, 1.35f));

        float MinClearance = Mathf.RoundToInt(10 + Mathf.Pow(LevelNumberClearanceFactor*LvlNum, .5f));
        float MaxClearance = Mathf.RoundToInt(20 + Mathf.Pow(LevelNumberClearanceFactor*LvlNum, .66f));

        MinClearance = Mathf.Clamp(MinClearance, 10f, 20f);
        MaxClearance = Mathf.Clamp(MaxClearance, 20f, MaximumAllowedVerticalDistanceBetweenPlatforms);

        GenerateHeightWiseDummyLevel(LvlNum,-10, LevelMaxY, -OrthoWidth / 2, OrthoWidth / 2, MinClearance, MaxClearance);
    }
    
    void OnFirstGame()
    {
        InGameUI.DisplayInfo();
    }

    void SpawnLevel(LevelDescription levelDescription)
    {
        SpawnBaseAndSidesPlatforms(levelDescription);

        foreach(PlatformDescription platformDescription in levelDescription.LevelPlatforms)
        {
            SpawnPlatform(platformDescription);
        }
    }
    
    void SpawnBaseAndSidesPlatforms(LevelDescription levelDescription)
    {
        float SidePlatformHieght;
        PlatformDescription RightSidePlatformDescription, LeftSidePlatformDescription, BasePlatformDescription;

        BasePlatformDescription = new PlatformDescription(new Vector3(0f, levelDescription.LevelMinY, 0f), new Vector3(10f, 1f, 1f), "White", false);
        SpawnPlatform(BasePlatformDescription);

        SidePlatformHieght = (levelDescription.LevelWinY - levelDescription.LevelMinY);
        RightSidePlatformDescription = new PlatformDescription(new Vector3(levelDescription.LevelOrthoWidth / 2, SidePlatformHieght / 2 + levelDescription.LevelMinY, 0f), new Vector3(1 / PlatformWidthIsToHeightRatio,SidePlatformHieght, 1f), "White", false);
        SpawnPlatform(RightSidePlatformDescription);

        LeftSidePlatformDescription = new PlatformDescription(new Vector3(-levelDescription.LevelOrthoWidth / 2, SidePlatformHieght / 2 + levelDescription.LevelMinY, 0f), new Vector3(1 / PlatformWidthIsToHeightRatio, SidePlatformHieght, 1f), "White", false);
        SpawnPlatform(LeftSidePlatformDescription);        
    }

    void SpawnPlatform(PlatformDescription platformDescription)
    {
        GameObject NewPlatform = Instantiate(PlatformPrefab, platformDescription.SpawnPosition, SpawnRotation);
        NewPlatform.transform.localScale = platformDescription.SpawnScale;

        NewPlatform.GetComponent<Platform>().IsControllable = platformDescription.IsControllable;
        NewPlatform.GetComponent<Platform>().InitialisePlatform(platformDescription.Color);
        NewPlatform.layer = 8;

        ActivePlatforms.Add(NewPlatform);
    }

    void GenerateDummyLevel(bool random, float MinY, float MaxY, float MinX, float MaxX, float Clearance, int Count)
    {
        List<PlatformDescription> DummyLevel = new List<PlatformDescription>();

        if (random)
        {
            for (int i = 0; i < Count; i++)
            {                
                PlatformDescription pD;
                pD = new PlatformDescription(new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), 0f), new Vector3(1f, .5f, 1f), "Green", true);

                int r = Random.Range(0, 4);
                if (r == 0)
                {
                    pD.Color = "Green";

                }
                else if (r == 1)
                {
                    pD.Color = "Orange";

                }
                else if (r == 2)
                {
                    pD.Color = "White";
                }
                else
                {
                    pD.Color = "Yellow";
                }

                int c = 0;

                foreach(PlatformDescription platformDescription in DummyLevel)
                {
                    float d = Vector3.Distance(pD.SpawnPosition, platformDescription.SpawnPosition);

                    if (d > Clearance)
                    {
                        c++;
                    }
                }

                if (c == DummyLevel.Count)
                {
                    DummyLevel.Add(pD);
                }
                else
                {
                    i--;
                }
            }
        }
        else
        {
            PlatformDescription NewPD;

            NewPD = new PlatformDescription(new Vector3(-3.25f, -3.78f, 0f), new Vector3(0.3f, 0.3f, .3f), "Yellow", true);
            DummyLevel.Add(NewPD);

            NewPD = new PlatformDescription(new Vector3(0f, -4.75f, 0f), new Vector3(.3f, 0.3f, .3f), "White", true);
            DummyLevel.Add(NewPD);

            NewPD = new PlatformDescription(new Vector3(1.86f, -1, 0f), new Vector3(0.3f, 0.3f, .3f), "Orange", true);
            DummyLevel.Add(NewPD);

            NewPD = new PlatformDescription(new Vector3(-2f, 3.38f, 0f), new Vector3(0.3f, 0.3f, .3f), "Green", true);
            DummyLevel.Add(NewPD);
        }

        LevelDescription lvldes = new LevelDescription(0, -10, 100, OrthoWidth);

        lvldes.LevelPlatforms = DummyLevel;

        SpawnLevel(lvldes);

        CurrentLevel = lvldes;

    }

    PlatformDescription RandomizeColor(PlatformDescription pD)
    {
        int r = Random.Range(0, 4);
        if (r == 0)
        {
            pD.Color = "Green";

        }
        else if (r == 1)
        {
            pD.Color = "Orange";

        }
        else if (r == 2)
        {
            pD.Color = "White";
        }
        else
        {
            pD.Color = "Yellow";
        }

        return pD;
    }

    void GenerateHeightWiseDummyLevel(int LevelNumber,float MinY, float MaxY, float MinX, float MaxX, float MinClearance, float MaxClearance)
    {
        bool FirstHalf = true;

        FirstHalf = ((LevelNumber % 2) == 0);

        float MarginY = 5f;

        float x = MinY;

        x += MarginY;

        List<PlatformDescription> DummyLevel = new List<PlatformDescription>();
        PlatformDescription pD;

        while (x < MaxY)
        {
            if (FirstHalf)
            {
                pD = new PlatformDescription(new Vector3(Random.Range(MinX + PlatformWidth / 2, -PlatformWidth / 2), x, 0f), new Vector3(1f, .5f, 1f), "Green", true);
            }
            else
            {
                pD = new PlatformDescription(new Vector3(Random.Range(PlatformWidth / 2, MaxX - PlatformWidth / 2), x, 0f), new Vector3(1f, .5f, 1f), "Green", true);
            }
            

            pD = RandomizeColor(pD);

            DummyLevel.Add(pD);

            x += Random.Range(MinClearance, MaxClearance);

            FirstHalf = !FirstHalf;
        }

        LevelDescription lvldes = new LevelDescription(CurrentLevel.LevelNumber, MinY, MaxY, OrthoWidth);

        lvldes.LevelPlatforms = DummyLevel;

        SpawnLevel(lvldes);
        CurrentLevel = lvldes;
        Player.GetComponent<Player>().CurrentLevel = CurrentLevel;
    }
}
