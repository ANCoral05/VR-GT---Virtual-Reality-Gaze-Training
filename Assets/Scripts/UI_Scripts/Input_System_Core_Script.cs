using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_System_Core_Script : MonoBehaviour
{
    public InputActionReference triggerInputActionReference;

    public bool getTrigger()
    {
        return triggerInputActionReference.action.ReadValue<bool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
