using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ResetMethod
{
    NoneOrCustom,
    OnSceneChange,
    OnGameEnd
}

public interface IScriptableObjectVariable<T>
{
    T Value { get; set; }
    event Action<T, T, string> OnValueChanged;
}

public abstract class ScriptableObjectVariable<T> : ScriptableObject, IScriptableObjectVariable<T>
{
    [SerializeField] private T value;
    [SerializeField] private ResetMethod resetMethod = ResetMethod.NoneOrCustom;
    [SerializeField] private bool showChangesInDebugLog;

    public event Action<T, T, string> OnValueChanged;

    private T resetValue;

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

    public void SetValueWithTag(T newValue, string tag)
    {
        ValueChanged(newValue, value, tag);
    }

    public void Reset()
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

        if (resetMethod == ResetMethod.OnGameEnd)
        {
            Application.quitting += Reset;
        }
        else if (resetMethod == ResetMethod.OnSceneChange)
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
    }

    private void OnDisable()
    {
        Application.quitting -= Reset;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnValidate()
    {
        if (!Equals(value, resetValue))
        {
            resetValue = value;
        }
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        Reset();
    }
}