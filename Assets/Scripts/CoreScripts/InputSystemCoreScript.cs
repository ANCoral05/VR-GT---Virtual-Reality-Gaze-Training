using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GazeQuestUtils;

public class XRController
{
    public bool isActive { get; set; }
    public bool rayActive { get; set; } = true;

    public ControllerHand controllerHand { get; set; }

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

    [HideInInspector, Tooltip("List of subscribed listeners that are informed whenever a controller hovers over a new object.")]
    public List<ButtonScript> hoverListeners = new List<ButtonScript>();

    [HideInInspector, Tooltip("List of subscribed listeners that are triggered by a shortcut without the requirement to hover over a visually displayed button.")]
    public List<ButtonScript> inputListeners = new List<ButtonScript>();

    [HideInInspector]
    public List<ControllerKey> pressedKeys = new List<ControllerKey>();

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
        leftController.controllerHand = ControllerHand.Left;

        rightController.controllerHand = ControllerHand.Right;
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

    //This function is played whenever a controller key is pressed and informs all input listeners about it.
    public void ControllerKeyEvent()
    {
        foreach (ButtonScript listener in inputListeners)
        {
            listener.OnPressed(pressedKeys);
        }

        pressedKeys = new List<ControllerKey>();
    }

    private void TriggerFunctionLeft(InputAction.CallbackContext context)
    {
        pressedKeys.Add(new ControllerKey(GazeQuestKey.Trigger, ControllerHand.Left));
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
