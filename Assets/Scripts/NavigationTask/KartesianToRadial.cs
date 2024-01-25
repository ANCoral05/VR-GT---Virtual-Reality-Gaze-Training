using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartesianToRadial : MonoBehaviour
{
    public Vector2 XY_angles;
    public float zDistance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = -(XY_angles.y-90) / 180 * Mathf.PI;
        float y = -(XY_angles.x-90)/180*Mathf.PI;
        float xPos = zDistance * Mathf.Sin(x) * Mathf.Cos(y);
        float yPos = zDistance * Mathf.Cos(x);
        float zPos = zDistance * Mathf.Sin(x) * Mathf.Sin(y);
        transform.localPosition = new Vector3(xPos, yPos, zPos);
    }
}
