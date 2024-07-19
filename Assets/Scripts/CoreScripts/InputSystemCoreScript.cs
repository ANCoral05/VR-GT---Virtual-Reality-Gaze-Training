using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GazeQuestUtils;

public class XRController
{
    public bool isActive { get; set; }
    public bool rayActive { get; set; } = true;

    public enum ControllerLeftRight
    {
        Left,
        Right
    }

    public ControllerLeftRight controllerLeftRight { get; set; }

    public Vector3 direction { get; set; }
    public Vector3 position { get; set; }
    public GameObject raycastTarget { get; set; }
}

public class InputSystemCoreScript : MonoBehaviour
{
    [Header("Action references - Left")]
    public InputActionReference triggerActionLeft;
    public InputActionReference primaryActionLeft;
    public InputActionReference secondaryActionLeft;
    public InputActionReference gripActionLeft;
    public InputActionReference thumbstickPressActionLeft;

    [Header("Action references - Right")]
    public InputActionReference triggerActionRight;
    public InputActionReference primaryActionRight;
    public InputActionReference secondaryActionRight;
    public InputActionReference gripActionRight;
    public InputActionReference thumbstickPressActionRight;

    [Header("Editor inputs")]
    public GameObject leftControllerObject;
    public GameObject rightControllerObject;

    public GameObject testCube;

    [Header("Public variables")]
    public XRController leftController;
    public XRController rightController;

    //List of subscribed objects that are informed whenever a controller hovers over a new object.
    public List<GameObject> hoverListeners = new List<GameObject>();

    public List<GameObject> buttonTriggerListeners = new List<GameObject>();

    public List<GameObject> buttonPrimaryListeners = new List<GameObject>();

    public List<GameObject> joystickListeners = new List<GameObject>();

    public List<controllerKeys> pressedKeys = new List<controllerKeys>();

    [Header("Private variables")]
    private GameObject hoveredTargetLeft;
    private GameObject hoveredTargetRight;

    private void Awake()
    {
        //Assign which function to play when a button is pressed.
        triggerActionLeft.action.started += TriggerFunctionLeft;
        triggerActionRight.action.started += TriggerFunctionRight;

        primaryActionLeft.action.started += PrimaryFunctionLeft;
        primaryActionRight.action.started += PrimaryFunctionRight;

        secondaryActionLeft.action.started += SecondaryFunctionLeft;
        secondaryActionRight.action.started += SecondaryFunctionRight;

        gripActionLeft.action.started += GripFunctionLeft;
        gripActionRight.action.started += GripFunctionRight;

        thumbstickPressActionLeft.action.started += ThumbstickPressFunctionLeft;
        thumbstickPressActionRight.action.started += ThumbstickPressFunctionRight;
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
        leftController.controllerLeftRight = XRController.ControllerLeftRight.Left;

        rightController.controllerLeftRight = XRController.ControllerLeftRight.Right;
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

    private void TriggerFunctionLeft(InputAction.CallbackContext context)
    {
        //event
    }

    private void TriggerFunctionRight(InputAction.CallbackContext context)
    {
        //event
    }

    private void PrimaryFunctionLeft(InputAction.CallbackContext context)
    {
        //event
    }

    private void PrimaryFunctionRight(InputAction.CallbackContext context)
    {
        //event
    }

    private void SecondaryFunctionLeft(InputAction.CallbackContext context)
    {
        //event
    }

    private void SecondaryFunctionRight(InputAction.CallbackContext context)
    {
        //event
    }

    private void GripFunctionLeft(InputAction.CallbackContext context)
    {
        //event
    }

    private void GripFunctionRight(InputAction.CallbackContext context)
    {
        //event
    }

    private void ThumbstickPressFunctionLeft(InputAction.CallbackContext context)
    {
        //event
    }

    private void ThumbstickPressFunctionRight(InputAction.CallbackContext context)
    {
        //event
    }
}
