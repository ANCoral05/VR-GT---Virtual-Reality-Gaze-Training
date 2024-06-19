using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    public Input_System_Core_Script inputScript;

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inputScript.getTrigger())
        {
            cube.SetActive(!cube.activeSelf);
        }
    }
}
