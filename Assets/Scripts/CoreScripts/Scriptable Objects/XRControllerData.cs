using GazeQuestUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XRControllerData", menuName = "ScriptableObjects/UI_and_Controls/XRControllerData")]
public class XRControllerData : ScriptableObject
{
    public bool isActive;
    public bool rayActive;

    public ControllerHand controllerHand;
}
