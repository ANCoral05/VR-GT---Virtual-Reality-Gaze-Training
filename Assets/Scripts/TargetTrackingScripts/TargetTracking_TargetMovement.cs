using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Description: 
// This script manages the movement of the targets that are to be tracked in the Target Tracking task.
// It also contains the indicators and markers for correct or wrong targets which are used in the TargetTrackingMain script.
// This scripts has to be added to the target prefab.

public class TargetTracking_TargetMovement : MonoBehaviour
{
    [Tooltip("Enter the ImageGameObject of the target prefab showing the indicator when a wrong target was selected.")]
    public GameObject errorMarker;

    [Tooltip("Enter the ImageGameObject of the target prefab that marks a correct target during the \"tracking\" stage.")]
    public GameObject correctTargetIndicator;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
