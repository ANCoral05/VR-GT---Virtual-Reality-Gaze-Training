using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRController
{
    public bool isActive;

    public enum IsLeftRight
    {
        Left,
        Right
    };

    public IsLeftRight isLeftRight;

    public Vector3 direction;

    public Vector3 position;

    public bool rayActive;

    public GameObject raycastTarget;
}

public class Input_System_Core_Script : MonoBehaviour
{
    [Header("Editor Input")]
    public InputDevice rightControllerInput;
    public InputDevice leftControllerInput;

    [Header("Accessible Variables")]
    public XRController leftController;
    public XRController rightController;

    //List of subscribed objects that are informed whenever a controller hovers over a new object.
    public List<GameObject> hoverListeners = new List<GameObject>();

    public List<GameObject> buttonTriggerListeners = new List<GameObject>();

    public List<GameObject> buttonPrimaryListeners = new List<GameObject>();

    public List<GameObject> joystickListeners = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {


        leftController.isLeftRight = XRController.IsLeftRight.Left;

        rightController.isLeftRight = XRController.IsLeftRight.Right;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
}
