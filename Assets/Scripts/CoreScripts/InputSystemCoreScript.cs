using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GazeQuestUtils;

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

    public bool rayActive = true;

    public GameObject raycastTarget;
}

public class InputSystemCoreScript : MonoBehaviour
{
    [Header("Editor Input")]
    public InputActionReference triggerReference = null;
    
    public GameObject leftControllerObject;
    public GameObject rightControllerObject;

    public GameObject testCube;

    [Header("Accessible Variables")]
    public XRController leftController;
    public XRController rightController;

    //List of subscribed objects that are informed whenever a controller hovers over a new object.
    public List<GameObject> hoverListeners = new List<GameObject>();

    public List<GameObject> buttonTriggerListeners = new List<GameObject>();

    public List<GameObject> buttonPrimaryListeners = new List<GameObject>();

    public List<GameObject> joystickListeners = new List<GameObject>();

    public List<controllerKeys> pressedKeys = new List<controllerKeys>();

    [Header("Private Variables")]
    private GameObject hoveredTargetLeft;
    private GameObject hoveredTargetRight;

    private void Awake()
    {
        triggerReference.action.started += Toggle;
    }


    void Start()
    {
        InitializeControllers();
    }


    void Update()
    {
        
    }

    private void InitializeControllers()
    {
        leftController.isLeftRight = XRController.IsLeftRight.Left;

        rightController.isLeftRight = XRController.IsLeftRight.Right;
    }

    private void TrackControllerVariables(XRController controller, GameObject controllerObject)
    {
        controller.direction = (controllerObject.transform.rotation * Vector3.forward).normalized;
        controller.position = controllerObject.transform.position;

        Ray ray = new Ray(controller.position, controller.direction);
        RaycastHit hit;

        // Perform the raycast
        if (controller.rayActive = true && Physics.Raycast(ray, out hit))
        {
            controller.raycastTarget = hit.transform.gameObject;
        }
    }

    public void HoverEventCheck()
    {
        if(hoveredTargetLeft != leftController.raycastTarget)
        {
            hoveredTargetLeft.SendMessage("OnHoverEnd");

            hoveredTargetLeft = leftController.raycastTarget;

            hoveredTargetLeft.SendMessage("OnHover");
        }
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        testCube.SetActive(!testCube.activeSelf);
    }
    
    
}
