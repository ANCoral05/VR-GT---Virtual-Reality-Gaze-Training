using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Variables/String Variable")]
    public class StringVariable : ScriptableObject
    {
        [Tooltip("Use this variable to update and modify at runtime.")]
        public string value;

        [Tooltip("Check to reset the variable to its starting state at the end of a scene.")]
        public bool resetOnSceneLoad;

        private string defaultValue;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            defaultValue = value;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (resetOnSceneLoad)
            {
                value = defaultValue;
            }
        }
    }
}
