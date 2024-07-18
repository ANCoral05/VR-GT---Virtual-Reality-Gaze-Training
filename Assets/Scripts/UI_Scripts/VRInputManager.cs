using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

public class VRInputManager : MonoBehaviour
{
    [HideInInspector] //Is true if the primary button (A or X) is pressed on the controller.
    public bool GetPrimaryButtonDown;

    [HideInInspector] //Is true if the trigger of the currently active controller is pressed.
    public bool GetMainTriggerDown;

    //Left and right input controller
    private InputDevice leftDevice;
    private InputDevice rightDevice;

    //The currently active controller, defined by which controller was last used
    private InputDevice activeDevice;

    //Is true if the trigger button of the left or right controller is pressed
    private bool leftTriggerPressed;
    private bool rightTriggerPressed;

    //Child object of left or right controller that determines direction of the pointer ray
    public GameObject leftPointer;
    public GameObject rightPointer;

    //The currently active pointer, determined by which controller was last used
    private GameObject activePointer;

    //List of devices found
    private List<InputDevice> devices = new List<InputDevice>();

    //XR input nodes required to define left and right controller device
    private XRNode xrNodeLeft = XRNode.LeftHand;
    private XRNode xrNodeRight = XRNode.RightHand;

    //TODO: Description
    public bool controllerTracking;

    //Bools to store key and trigger states
    public bool GetMainTrigger;

    public RaycastHit controllerClickRaycast(LayerMask UILayer)
    {
        RaycastHit hitPosition = new RaycastHit();

        if (leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPressed) || rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPressed))
        {
            if (leftTriggerPressed || rightTriggerPressed || controllerTracking)
            {
                Ray pointRay = new Ray(activePointer.transform.position, activePointer.transform.rotation * Vector3.forward);

                Physics.Raycast(pointRay, out hitPosition, 100, UILayer);
            }
        }
        return hitPosition;
    }

    private void OnEnable()
    {
        if (!leftDevice.isValid || !rightDevice.isValid)
        {
            GetDevices();
        }
    }

    private void GetMainTriggerData()
    {
        CheckTrigger(leftDevice, leftPointer, rightPointer);
        CheckTrigger(rightDevice, rightPointer, leftPointer);
    }

    private void CheckTrigger(InputDevice device, GameObject pointerToActivate, GameObject pointerToDeactivate)
    {
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed)
            {
                if (!GetMainTrigger)
                {
                    activePointer = pointerToActivate;
                    activeDevice = device;

                    pointerToActivate.SetActive(true);
                    pointerToDeactivate.SetActive(false);

                    GetMainTriggerDown = true;
                }
                else
                {
                    GetMainTriggerDown = false;
                }

                GetMainTrigger = true;
            }
        }
    }


    void GetDevices()
    {
        InputDevices.GetDevicesAtXRNode(xrNodeLeft, devices);
        leftDevice = devices.FirstOrDefault();

        InputDevices.GetDevicesAtXRNode(xrNodeRight, devices);
        rightDevice = devices.FirstOrDefault();
    }
}
