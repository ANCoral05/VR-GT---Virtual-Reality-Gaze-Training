using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GazeQuestUtils;

public class ActionEventScript : MonoBehaviour
{
    [Header("Input variables")]
    [Tooltip("Choose the script and function to run when this button is activated.")]
    public UnityEvent triggeredFunction;

    [Range(0, 1), Tooltip("Set the strength of haptic feedback (vibration) when the selection ray starts hovers over the button.")]
    public float hoverHapticFeedback;

    [Range(0, 1), Tooltip("Set the strength of haptic feedback (vibration) when the button is selected.")]
    public float activateHapticFeedback;

    [Header("Editor Inputs")]
    [Tooltip("Choose the controller key (or multiple keys) that will activate this button's function if pressed while hovering over the button.")]
    public List<ControllerKey> directInteractionButton = new List<ControllerKey>();

    [Tooltip("Choose the controller key (or multiple keys) that will activate this button's function even if the button is not directly targeted (like a shortcut).")]
    public List<ControllerKey> shortcutKey = new List<ControllerKey>();

    [Tooltip("Add the Input Manager Core Script.")]
    public InputSystemCoreScript inputManagerScript;

    [SerializeField, Tooltip("Choose the default texture of the button.")]
    private Texture2D defaultTexture;

    [SerializeField, Tooltip("Choose the texture to display when a selection ray hovers over the button.")]
    private Texture2D hoverTexture;

    [SerializeField, Tooltip("Choose the sound clip that is played when the button is activated.")]
    private AudioClip buttonActivateSoundClip;

    [Header("Public variables")]
    [Tooltip("States whether the button is currently hovered.")]
    public bool hoverState;



    void Awake()
    {
        if(inputManagerScript == null)
        {
            inputManagerScript = FindObjectOfType<InputSystemCoreScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        inputManagerScript.inputListeners.Add(this);
    }

    private void OnDisable()
    {
        inputManagerScript.inputListeners.Remove(this);
    }

    public void OnPressed(ControllerKey inputKey)
    {
        foreach (ControllerKey testKey in shortcutKey)
        {
            if (inputKey == testKey)
            {
                triggeredFunction.Invoke();
            }
        }
    }

    // DO NOT RENAME FUNCTION! This function is called as string in the InputSystemCoreScript.cs
    public void OnHover()
    {
        this.GetComponentInChildren<Renderer>().material.mainTexture = hoverTexture;
    }

    public void OnHoverEnd()
    {
        this.GetComponentInChildren<Renderer>().material.mainTexture = defaultTexture;
    }

    void OnActivated()
    {
        
    }

    void OnDeactivated()
    {

    }
}
