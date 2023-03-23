using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartesianToRadialTransform : MonoBehaviour
{
    [HideInInspector] //Azimuth (x) and elevation (y) angles between forward axis and kartesian Vector3 coordinate.
    public Vector2 XY_angles;

    [HideInInspector] //Distance (z) to reference plane.
    public float zDistance;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
