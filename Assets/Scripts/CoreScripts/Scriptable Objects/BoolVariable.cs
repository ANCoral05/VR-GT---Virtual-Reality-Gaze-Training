using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Variables/Bool Variable")]
    public class BoolVariable : ScriptableObject
    {
        [SerializeField]
        private bool Value;

        public bool value
        {
            get => Value;
            set
            {
                Value = value;

                HasValueChanged(Value, "");
                previousValue = Value;
            }
        }

        private bool previousValue;

        private enum AutoReset
        {
            None,
            OnSceneLoad,
            OnGameEnd
        }

        [SerializeField, Tooltip("Set when to reset the value to its initial value. Initial value will be the last value set outside of runtime.")]
        private AutoReset automaticReset = AutoReset.None;

        [SerializeField, Tooltip("If checked, any runtime change to the value will be shown in the Debug.Log.")]
        private bool showInDebugLog;

        public delegate void ValueChangedHandler(bool previousValue, bool currentValue, string tag);

        public event ValueChangedHandler OnValueChanged;

        #region backupValues

        private bool initialValue;

        #endregion


        public void SetValueWithTag(bool newValue, string tag = "")
        {
            value = newValue;
            if (value != previousValue)
            {
                HasValueChanged(value, tag);
                
            }
        }

        private void HasValueChanged(bool newValue, string tag)
        {
            if (previousValue != newValue && Application.isPlaying)
            {
                OnValueChanged?.Invoke(previousValue, newValue, tag);

                if (tag != "")
                {
                    Debug.Log($"BoolVariable {name} changed from {previousValue} to {newValue} with tag {tag}");
                }
                else
                {
                    Debug.Log($"BoolVariable {name} changed from {previousValue} to {newValue}");
                }
            }

            previousValue = newValue;
        }

        private void SaveInitialState()
        {
            if (Application.isPlaying)
                return;

            initialValue = value;
        }

        private void Reset()
        {
            value = initialValue;
        }

        private void OnEnable()
        {
            SaveInitialState();
            SetAutoResetMode();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Application.quitting -= Reset;
        }

        private void SetAutoResetMode()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Application.quitting -= Reset;

            if (automaticReset == AutoReset.OnSceneLoad)
                SceneManager.sceneLoaded += OnSceneLoaded;
            else if (automaticReset == AutoReset.OnGameEnd)
                Application.quitting += Reset;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Reset();
        }

        private void OnValidate()
        {
            SaveInitialState();
            SetAutoResetMode();
            HasValueChanged(value, "");
        }
    }
}
