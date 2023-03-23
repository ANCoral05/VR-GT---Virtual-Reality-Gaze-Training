using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Input_Manager : MonoBehaviour
{
    [HideInInspector] //Is true if the primary button (A or X) is pressed on the controller.
    public bool GetPrimaryButtonDown;

    [HideInInspector] //Is true if the trigger of the currently active controller is pressed.
    public bool GetMainTriggerDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
