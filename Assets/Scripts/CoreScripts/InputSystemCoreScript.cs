using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GazeQuestUtils;
using TMPro;
using VRK_BuildingBlocks;

public class XRController
{
    public XRControllerData xrControllerData;

    public GameObject controllerObject;
    public GameObject raycastVisualizer;
    public GameObject raycastTarget;

    public Vector3 direction;
    public Vector3 position;
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
    public PooledGameObjectVariable pooledGameObject;
    public ArrayVariable<string> testArray2;
    public StringVariable testString;
    public Vector4Variable testVector4;

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
    [Tooltip("Input the Scriptable Object that stores the data for the left controller.")]
    public XRControllerData leftControllerData;

    [Tooltip("Input the Scriptable Object that stores the data for the right controller.")]
    public XRControllerData rightControllerData;

    [Tooltip("Input the Scriptable Object that stores the controller state.")]
    public ActiveControllerSelectionData activeControllerSelectionData;

    [Tooltip("Input the GameObject of the left controller.")]
    public GameObject leftControllerObject;

    [Tooltip("Input the GameObject of the right controller.")]
    public GameObject rightControllerObject;

    [Header("Test parameters")]
    public GameObject testCube;
    public GameObject leftTestCube;
    public GameObject rightTestCube;

    [Header("Public variables")]
    [HideInInspector, Tooltip("List of subscribed listeners that are informed whenever a controller hovers over a new object.")]
    public List<ActionEventScript> hoverListeners = new List<ActionEventScript>();

    [Tooltip("List of subscribed listeners that are triggered by a shortcut without the requirement to hover over a visually displayed button.")]
    public List<ActionEventScript> inputListeners = new List<ActionEventScript>();

    [HideInInspector]
    public List<ControllerKey> pressedKeys = new List<ControllerKey>();

    [HideInInspector, Tooltip("The controller entity of the left controller that is referenced by this and other scripts.")]
    public XRController leftController = new XRController();

    [HideInInspector, Tooltip("The controller entity of the right controller that is referenced by this and other scripts.")]
    public XRController rightController = new XRController();

    [Header("Private variables")]
    private List<XRController> activeControllers = new List<XRController>();

    private GameObject hoveredTargetLeft;
    private GameObject hoveredTargetRight;

    private void Awake()
    {
        //Function to make the following assigning of functions to input actions more readable.
        void AssignFunctionToAction(InputActionReference inputActionReference, System.Action<InputAction.CallbackContext> assignedFunction)
        {
            if(inputActionReference != null)
            {
                inputActionReference.action.started += assignedFunction;
            }
        }

        //Assign which function to play when a button is pressed.
        AssignFunctionToAction(triggerActionLeft, TriggerFunctionLeft);
        AssignFunctionToAction(triggerActionRight, TriggerFunctionRight);

        AssignFunctionToAction(primaryActionLeft, PrimaryFunctionLeft);
        AssignFunctionToAction(primaryActionRight, PrimaryFunctionRight);

        AssignFunctionToAction(secondaryActionLeft, SecondaryFunctionLeft);
        AssignFunctionToAction(secondaryActionRight, SecondaryFunctionRight);

        AssignFunctionToAction(gripActionLeft, GripFunctionLeft);
        AssignFunctionToAction(gripActionRight, GripFunctionRight);

        AssignFunctionToAction(thumbstickPressActionLeft, ThumbstickPressFunctionLeft);
        AssignFunctionToAction(thumbstickPressActionRight, ThumbstickPressFunctionRight);
    }

    void Start()
    {
        InitializeControllers();
    }


    void Update()
    {
        TrackControllerVariables(leftController);

        TrackControllerVariables(rightController);
    }

    #region Controller Functions

    private void InitializeControllers()
    {
        leftController.xrControllerData = leftControllerData;
        leftController.xrControllerData.controllerHand = ControllerHand.Left;
        leftController.controllerObject = leftControllerObject;
        leftController.raycastVisualizer = leftController.controllerObject.transform.Find("RayCastVisualizer").gameObject;

        rightController.xrControllerData = rightControllerData;
        rightController.xrControllerData.controllerHand = ControllerHand.Right;
        rightController.controllerObject = rightControllerObject;
        rightController.raycastVisualizer = rightController.controllerObject.transform.Find("RayCastVisualizer").gameObject;

        SetControllerActiveStatus();
    }

    private void TrackControllerVariables(XRController controller)
    {
        if (!controller.xrControllerData.isActive)
            return;

        controller.direction = (controller.controllerObject.transform.rotation * Vector3.forward).normalized;
        controller.position = controller.controllerObject.transform.position;

        Ray ray = new Ray(controller.position, controller.direction);
        RaycastHit hit;

        GameObject newRaycastTarget = null;

        if (controller.xrControllerData.rayActive == true && Physics.Raycast(ray, out hit))
        {
            newRaycastTarget = hit.transform.gameObject;
        }

        HoverEventCheck(controller, newRaycastTarget);
    }

    public void SetControllerActiveStatus(ActiveControllerSelection? changeControllerSelection = null)
    {
        if(changeControllerSelection.HasValue)
        {
            activeControllerSelectionData.activeControllerSelection = changeControllerSelection.Value;
        }

        if (activeControllerSelectionData.activeControllerSelection == ActiveControllerSelection.Both)
        {
            ControllerStateChange(leftController, true);
            ControllerStateChange(rightController, true);
        }

        if(activeControllerSelectionData.activeControllerSelection == ActiveControllerSelection.LastPressed)
        {
            ControllerStateChange(leftController, false);
            ControllerStateChange(rightController, false);

            ControllerStateChange(HandToController(activeControllerSelectionData.lastPressedControllerHand), true);
        }

        if(activeControllerSelectionData.activeControllerSelection == ActiveControllerSelection.Left)
        {
            ControllerStateChange(leftController, true);
            ControllerStateChange(rightController, false);
        }

        if (activeControllerSelectionData.activeControllerSelection == ActiveControllerSelection.Right)
        {
            ControllerStateChange(leftController, false);
            ControllerStateChange(rightController, true);
        }

        if (activeControllerSelectionData.activeControllerSelection == ActiveControllerSelection.None)
        {
            ControllerStateChange(leftController, false);
            ControllerStateChange(rightController, false);
        }
    }

    private void ControllerStateChange(XRController controller, bool state)
    {
        controller.xrControllerData.isActive = state;
        controller.raycastVisualizer.SetActive(state);

        if(state == false)
        {
            HoverEventCheck(controller, null);

            controller.raycastTarget = null;
        }
    }

    private XRController HandToController(ControllerHand controllerHand)
    {
        XRController controller = null;

        if (controllerHand == ControllerHand.Left)
            controller = leftController;

        if (controllerHand == ControllerHand.Right)
            controller = rightController;

            return controller;
    }

    #endregion

    #region Input functions
    //This function is played whenever a controller key is pressed and informs all input listeners about it, given that the controller is currently active.
    public void ControllerKeyEvent(ControllerKey inputKey, XRController controllerPressed)
    {
        if (controllerPressed.xrControllerData.isActive)
        {
            foreach (ActionEventScript listener in inputListeners)
            {
                listener.OnPressed(inputKey);
            }
        }

        activeControllerSelectionData.lastPressedControllerHand = controllerPressed.xrControllerData.controllerHand;

        SetControllerActiveStatus();
    }

    private void TriggerFunctionLeft(InputAction.CallbackContext context)
    {       
        ControllerKeyEvent(ControllerKey.Trigger_Left, leftController);
    }

    private void TriggerFunctionRight(InputAction.CallbackContext context)
    {
        ControllerKeyEvent(ControllerKey.Trigger_Right, rightController);

        ToggleCube();
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
    public void HoverEventCheck(XRController controller, GameObject newRaycastTarget)
    {
        if (newRaycastTarget == controller.raycastTarget)
        {
            return;
        }

        ActionEventScript previousActionEventScript = controller.raycastTarget?.GetComponent<ActionEventScript>();

        ActionEventScript newActionEventScript = newRaycastTarget?.GetComponent<ActionEventScript>();

        previousActionEventScript?.OnHoverEnd();

        newActionEventScript?.OnHover();

        controller.raycastTarget = newRaycastTarget;
    }


    #endregion


    public void ToggleCube()
    {
        testCube.SetActive(!testCube.activeSelf);
    }
}
