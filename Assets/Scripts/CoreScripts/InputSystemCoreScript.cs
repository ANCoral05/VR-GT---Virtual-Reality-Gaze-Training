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

    public GameObject controllerObject { get; set; }
    public GameObject raycastVisualizer { get; set; }
    public Vector3 direction { get; set; }
    public Vector3 position { get; set; }
    public GameObject raycastTarget { get; set; }
}

public enum ActiveControllerSelection
{
    Both,
    LastPressed,
    Left,
    Right,
    None
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
    public GameObject leftTestCube;
    public GameObject rightTestCube;

    [Header("Public variables")]
    [Tooltip("Decide which controller is 'active', i.e. is used for selecting and interacting.")]
    public ActiveControllerSelection activeControllerSelection = ActiveControllerSelection.Both;

    public XRController leftController;
    public XRController rightController;

    [HideInInspector, Tooltip("List of subscribed listeners that are informed whenever a controller hovers over a new object.")]
    public List<ActionEventScript> hoverListeners = new List<ActionEventScript>();

    [Tooltip("List of subscribed listeners that are triggered by a shortcut without the requirement to hover over a visually displayed button.")]
    public List<ActionEventScript> inputListeners = new List<ActionEventScript>();

    [HideInInspector]
    public List<ControllerKey> pressedKeys = new List<ControllerKey>();

    [Header("Private variables")]
    private List<XRController> activeControllers = new List<XRController>();

    private XRController lastPressedController;

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
        TrackControllerVariables(leftController, leftControllerObject);

        TrackControllerVariables(rightController, rightControllerObject);
    }

    #region Controller Functions

    private void InitializeControllers()
    {
        leftController.controllerHand = ControllerHand.Left;
        leftController.controllerObject = leftControllerObject;

        rightController.controllerHand = ControllerHand.Right;
        rightController.controllerObject = rightControllerObject;

        leftController.raycastVisualizer = leftController.controllerObject.transform.Find("Left Controller Visual").gameObject;
        rightController.raycastVisualizer = rightController.controllerObject.transform.Find("Right Controller Visual").gameObject;

        lastPressedController = leftController;

        SetControllerActiveStatus();
    }

    private void TrackControllerVariables(XRController controller, GameObject controllerObject)
    {
        controller.direction = (controllerObject.transform.rotation * Vector3.forward).normalized;
        controller.position = controllerObject.transform.position;

        Ray ray = new Ray(controller.position, controller.direction);
        RaycastHit hit;

        if (controller.rayActive = true && Physics.Raycast(ray, out hit))
        {
            controller.raycastTarget = hit.transform.gameObject;
        }

        HoverEventCheck();
    }

    private void SetControllerActiveStatus(ActiveControllerSelection? changeControllerSelection = null)
    {
        if(changeControllerSelection.HasValue)
        {
            activeControllerSelection = changeControllerSelection.Value;
        }

        if (activeControllerSelection == ActiveControllerSelection.Both)
        {
            ControllerStateChange(leftController, true);
            ControllerStateChange(rightController, true);
        }

        if(activeControllerSelection == ActiveControllerSelection.LastPressed)
        {
            ControllerStateChange(leftController, false);
            ControllerStateChange(rightController, false);

            ControllerStateChange(lastPressedController, true);
        }

        if(activeControllerSelection == ActiveControllerSelection.Left)
        {
            ControllerStateChange(leftController, true);
            ControllerStateChange(rightController, false);
        }

        if (activeControllerSelection == ActiveControllerSelection.Right)
        {
            ControllerStateChange(leftController, false);
            ControllerStateChange(rightController, true);
        }

        if (activeControllerSelection == ActiveControllerSelection.None)
        {
            ControllerStateChange(leftController, false);
            ControllerStateChange(rightController, false);
        }
    }

    private void ControllerStateChange(XRController controller, bool state)
    {
        controller.isActive = state;
        controller.raycastVisualizer.SetActive(state);
        controller.controllerObject.SetActive(state);

        if (controller == leftController)
            leftTestCube.SetActive(!leftTestCube.activeSelf);

        if (controller == rightController)
            rightTestCube.SetActive(!rightTestCube.activeSelf);
    }
    #endregion

    #region Input functions
    //This function is played whenever a controller key is pressed and informs all input listeners about it.
    public void ControllerKeyEvent(ControllerKey inputKey, XRController controllerPressed)
    {
        foreach (ActionEventScript listener in inputListeners)
        {
            listener.OnPressed(inputKey);
        }

        lastPressedController = controllerPressed;

        SetControllerActiveStatus();
    }

    private void TriggerFunctionLeft(InputAction.CallbackContext context)
    {
        ControllerKeyEvent(ControllerKey.Trigger_Left, leftController);
    }

    private void TriggerFunctionRight(InputAction.CallbackContext context)
    {
        ControllerKeyEvent(ControllerKey.Trigger_Right, rightController);
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
    #endregion

    #region Button Functions
    public void HoverEventCheck()
    {
        if (hoveredTargetLeft != leftController.raycastTarget)
        {
            hoveredTargetLeft.SendMessage("OnHoverEnd");

            hoveredTargetLeft = leftController.raycastTarget;

            hoveredTargetLeft.SendMessage("OnHover");
        }
    }


    #endregion


    public void ToggleCube()
    {
        testCube.SetActive(!testCube.activeSelf);
    }
}
