using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float OrthographicWidth = 9f;

    public Vector3 DesiredCameraPosition;

    public float SmoothSpeed = 0.125f;

    public Vector3 CameraPole = new Vector3(0f, 3f, -10f);
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DesiredCameraPosition = new Vector3(0f, 0f, 0f);

        ResetCameraOrthographicSize(OrthographicWidth);
    }

    void FixedUpdate()
    {
        Vector3 DesiredPos = DesiredCameraPosition + CameraPole;
        Vector3 SmoothedPos = Vector3.Lerp(transform.position, DesiredPos, SmoothSpeed);
        transform.position = SmoothedPos;
    }

    public void ResetCameraOrthographicSize(float VisibleWidth)
    {
        float OrthoSize = 0.5f * VisibleWidth * Screen.height / Screen.width;

        Camera.main.orthographicSize = OrthoSize;
    }

}
