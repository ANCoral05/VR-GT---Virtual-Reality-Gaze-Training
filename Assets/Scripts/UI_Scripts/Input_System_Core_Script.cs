using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVariables
{
    public enum IsLeftRight
    {
        Left,
        Right
    };

    public IsLeftRight isLeftRight;

    public Vector3 direction;

    public Vector3 position;

    public bool rayActive;
}

public class Input_System_Core_Script : MonoBehaviour
{
    [Header("Accessible Variables")]
    public Vector3 directionLeftController;
    public Vector3 directionRightController;

    public Vector3 originLeftController;
    public Vector3 originRightController;

    private ControllerVariables controller;

    public GameObject hoveredObject;

    //List of subscribed objects that are informed whenever a controller hovers over a new object.
    public List<GameObject> hoverListeners = new List<GameObject>();

    public List<GameObject> buttonTriggerListeners = new List<GameObject>();
    public List<GameObject> buttonPrimaryListeners = new List<GameObject>();

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
        controller.isLeftRight = ControllerVariables.IsLeftRight.Left;
    }

    
    
}
