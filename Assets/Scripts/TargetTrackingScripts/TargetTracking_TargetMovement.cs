using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Description: 
// This script manages the movement of the targets that are to be tracked in the Target Tracking task.
// It also contains the indicators and markers for correct or wrong targets which are used in the TargetTrackingMain script.
// This scripts has to be added to the target prefab.

public class TargetTracking_TargetMovement : MonoBehaviour
{
    // ### Input variables ###
    [Header("Input variables")]

    [Tooltip("The distance at which a target will turn around to avoid collisions.")]
    public float bounceDistance = 0.15f;

    [Tooltip("The movement speed of the target.")]
    public float speed;

    // ### Editor input ###
    [Header("Editor input")]

    [Tooltip("Enter the ImageGameObject of the target prefab showing the indicator when a wrong target was selected.")]
    public GameObject errorMarker;

    [Tooltip("Enter the ImageGameObject of the target prefab that marks a correct target during the \"tracking\" stage.")]
    public GameObject correctTargetIndicator;

    //Scripts
    [Tooltip("Enter the 'TargetTrackingMain' script from the script manager.")]
    public TargetTrackingMain targetTrackingMain;

    // ### Debug info ###
    [Header("Debug info")]



    // ### Private variables ###
    [Header("Private variables")]

    [HideInInspector]
    public bool allowedToMove;

    private Vector2 xy_angles;

    [HideInInspector] //TODO: Rename and potentially change to private; The Vector2 by which the target moves over one frame
    public Vector2 shiftingAngles;

    //The shifting angle of the previous frame.
    private Vector2 previousAngles;

    //Position of the point the target moves towards.
    private Vector3 destinationPosition;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SetNewTargetPosition()
    {


        float azimuth = Random.Range(-targetTrackingMain.fieldDimensions.x, targetTrackingMain.fieldDimensions.x);

        float elevation = Random.Range(-targetTrackingMain.fieldDimensions.y, targetTrackingMain.fieldDimensions.y);

        float distance = Random.Range(targetTrackingMain.fieldDistance * 0.8f, targetTrackingMain.fieldDistance * 1.25f);

        destinationPosition = new Vector3(azimuth, elevation, distance);
    }
}
