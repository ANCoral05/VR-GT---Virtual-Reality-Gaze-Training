using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRK_BuildingBlocks
{
    public enum ResetMethod
    {
        NoneOrCustom,
        OnSceneEnd,
        OnGameEnd
    }

    public abstract class ScriptableObjectVariable<T> : ScriptableObject
    {
        [SerializeField] private T value;
        [SerializeField] private ResetMethod resetMethod = ResetMethod.OnSceneEnd;
        [SerializeField] protected bool showChangesInDebugLog;

        public event Action<T, T, string> OnValueChanged;

        protected T resetValue;

        public T Value
        {
            get => value;
            set
            {
                if (!Equals(this.value, value))
                {
                    ValueChanged(value, this.value, "");
                }
            }
        }

        private void ValueChanged(T newValue, T previousValue, string tag)
        {
            this.value = newValue;

            if (Application.isPlaying)
            {
                OnValueChanged?.Invoke(newValue, previousValue, tag);

                if (!showChangesInDebugLog)
                {
                    return;
                }

                if (string.IsNullOrEmpty(tag))
                {
                    Debug.Log($"<{typeof(T)}> variable '{name}' changed from {previousValue} to {newValue}.");
                }
                else
                {
                    Debug.Log($"<{typeof(T)}> variable '{name}' changed from {previousValue} to {newValue} with tag {tag}.");
                }
            }
        }

        private void SetValueWithTag(T newValue, string tag)
        {
            ValueChanged(newValue, value, tag);
        }

        protected virtual void Reset()
        {
            Value = resetValue;

            if (showChangesInDebugLog)
            {
                Debug.Log($"<{typeof(T)}> variable '{name}' reset to {resetValue}.");
            }
        }

        private void OnEnable()
        {
            resetValue = value;

            SubscribeResetMethod();
        }

        private void OnDisable()
        {
            UnsubscribeResetMethod();
        }

        private void SubscribeResetMethod()
        {
            UnsubscribeResetMethod();

            if(resetMethod == ResetMethod.NoneOrCustom)
            {
                Application.quitting += SetResetValue;
            }
            else if (resetMethod == ResetMethod.OnSceneEnd)
            {
                SceneManager.sceneUnloaded += OnSceneChanged;
                Application.quitting += Reset;
            }
            else if (resetMethod == ResetMethod.OnGameEnd)
            {
                Application.quitting += Reset;
            }
        }

        private void UnsubscribeResetMethod()
        {
            Application.quitting -= SetResetValue;
            SceneManager.sceneUnloaded -= OnSceneChanged;
            Application.quitting -= Reset;
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                SetResetValue();
            }

            SubscribeResetMethod();
        }

        protected virtual void SetResetValue()
        {
            if (!Equals(Value, resetValue))
            {
                resetValue = Value;
            }
        }

        private void OnSceneChanged(Scene scene)
        {
            Reset();
        }
    }
}