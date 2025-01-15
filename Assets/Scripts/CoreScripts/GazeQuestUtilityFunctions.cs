using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeQuestUtils
{
    public static class GazeQuestUtilityFunctions
    {
        public static List<GameObject> GetDescendents(GameObject parentObject)
        {
            List<GameObject> list = new List<GameObject>();

            foreach (Transform child in parentObject.transform)
            {
                list.Add(child.gameObject);
                GetDescendents(child.gameObject);
            }

            return list;
        }
    }



    // Enum for controller keys
    public enum ControllerKey
    {
        Trigger_Left,
        Trigger_Right,
        Primary_Left,
        Primary_Right,
        Secondary_Left,
        Secondary_Right,
        Handle_Left,
        Handle_Right,
        ThumbstickPress_Left,
        ThumbstickPress_Right
    }

    public enum ControllerHand
    {
        Left,
        Right
    }

    public static class GazeQuest_Methods
    {
        public static void DeactivateObject(GameObject _object)
        {
            if (_object != null)
                _object.SetActive(false);
        }

        public static void ActivateObject(GameObject _object)
        {
            if (_object != null)
                _object.SetActive(true);
        }

        public static void ToggleObject(GameObject _object)
        {
            if (_object != null)
                _object.SetActive(!_object.activeSelf);
        }
    }
}
