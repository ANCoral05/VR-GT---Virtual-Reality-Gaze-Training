using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeQuestUtils
{
    public static class GazeQuestUtilityFunctions
    {
        public static List<GameObject> GetDescendants(GameObject rootObject)
        {
            List<GameObject> list = new List<GameObject>();

            GetDescendantCycle(rootObject, list);

            return list;
        }

        private static void GetDescendantCycle(GameObject rootObject, List<GameObject> list)
        {
            foreach (Transform child in rootObject.transform)
            {
                list.Add(child.gameObject);
                GetDescendantCycle(child.gameObject, list);
            }
        }

        public static List<string> GetDescendantsRelativePaths(GameObject rootObject)
        {
            List<string> relativePathsList = new List<string>();

            GetRelativePathCycle(rootObject, rootObject.name, relativePathsList);

            return relativePathsList;
        }

        private static void GetRelativePathCycle(GameObject rootObject, string path, List<string> relativePaths)
        {
            for (int i = 0; i < rootObject.transform.childCount; i++)
            {
                Transform child = rootObject.transform.GetChild(i);
                string currentPath = $"{path}[{i}]_{child.gameObject.name}";
                relativePaths.Add(currentPath);
                GetRelativePathCycle(child.gameObject, currentPath, relativePaths);
            }
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
