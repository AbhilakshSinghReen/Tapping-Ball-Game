using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlatformManager : MonoBehaviour
{
    [Range(0,100)]
    public int MenuPlatformCount;

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public GameObject PlatformPrefab;

    List<GameObject> ActivePlatforms;

    // Start is called before the first frame update
    void Start()
    {
        ActivePlatforms = new List<GameObject>();

        for(int i = 0; i < MenuPlatformCount; i++)
        {
            SpawnRandomPlatform(xMin, xMax, yMin, yMax);
            Debug.Log("Added platform");
        }
    }

    void SpawnRandomPlatform(float xMin,float xMax,float yMin,float yMax)
    {
        GameObject NewPlatform = Instantiate(PlatformPrefab, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0f), Quaternion.Euler(0, 0, Random.Range(-90f, 90f)));


        
        NewPlatform.GetComponent<Platform>().IsControllable = false;
        int r = Random.Range(0, 4);
        if (r == 0)
        {
            NewPlatform.GetComponent<Platform>().InitialisePlatform("Green");

        }
        else if (r == 1)
        {
            NewPlatform.GetComponent<Platform>().InitialisePlatform("Orange");

        }
        else if (r == 2)
        {
            NewPlatform.GetComponent<Platform>().InitialisePlatform("White");
        }
        else
        {
            NewPlatform.GetComponent<Platform>().InitialisePlatform("Yellow");
        }

        ActivePlatforms.Add(NewPlatform);        
    }
}
