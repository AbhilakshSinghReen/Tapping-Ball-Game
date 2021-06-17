using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraSlightlyScript : MonoBehaviour
{
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public float SmoothSpeed = 0.125f;

    public float MinResetDistance;

    [HideInInspector]
    public Vector3 Destination;

    void Start()
    {
        Destination = new Vector3(0f, 0f, -10f);
    }

    void FixedUpdate()
    {
        MoveCameraSlightly();
    }

    void MoveCameraSlightly()
    {
        Vector3 SmoothedPos = Vector3.Lerp(transform.position, Destination, SmoothSpeed);
        SmoothedPos = Vector3.Lerp(transform.position, SmoothedPos, SmoothSpeed);
        SmoothedPos = Vector3.Lerp(transform.position, SmoothedPos, SmoothSpeed);

        transform.position = SmoothedPos;

        if(Vector3.Distance(transform.position,Destination) < MinResetDistance)
        {
            GetNewDestination();
        }
    }
    void GetNewDestination()
    {
        Destination = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -10f);
    }
}
