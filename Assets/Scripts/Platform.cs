using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Sprite[] PlatformImages;

    [HideInInspector]
    public string PlatformColor;

    [HideInInspector]
    public bool IsControllable = true;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialisePlatform(string PlatColor)
    {
        foreach(Sprite sprite in PlatformImages)
        {
            if(PlatColor== sprite.name)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprite;
                PlatformColor = PlatColor;
            }
        }
    }
}
