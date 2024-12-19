using GazeQuestUtils;
using UnityEngine;


namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(fileName = "ActiveControllerSelectionData", menuName = "ScriptableObjects/ControlsAndInterfaces/ActiveControllerSelectionData")]
    public class ActiveControllerSelectionData : ScriptableObject
    {
        [Tooltip("Decide which controller is 'active', i.e. is used for selecting and interacting.")]
        public ActiveControllerSelection activeControllerSelection = ActiveControllerSelection.LastPressed;

        [Tooltip("Defines the last controller on which a key was pressed.")]
        public ControllerHand lastPressedControllerHand = ControllerHand.Right;
    }
}
