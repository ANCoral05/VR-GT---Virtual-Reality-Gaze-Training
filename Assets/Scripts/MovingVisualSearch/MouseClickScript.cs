using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickScript : MonoBehaviour
{
    // unity input system action for the mouse click
    public InputActionReference clickAction;

    // A function that does a cursor to world point raycast and checks if the colliding object has a "TargetScript" component
    void Update()
    {
        if (clickAction.action.triggered)
        {
            // Get the mouse position on the screen
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<TargetSelectionScript>() != null)
                {
                    hit.collider.gameObject.GetComponent<TargetSelectionScript>().OnClick();
                }
            }
        }
    }
}
