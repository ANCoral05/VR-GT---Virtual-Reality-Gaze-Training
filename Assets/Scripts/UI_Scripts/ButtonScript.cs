using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GazeQuestUtils;

public class ButtonScript : MonoBehaviour
{
    [Header("Input variables")]
    [Range(0, 1), Tooltip("Set the strength of haptic feedback (vibration) when the selection ray starts hovers over the button.")]
    public float hoverHapticFeedback;

    [Range(0, 1), Tooltip("Set the strength of haptic feedback (vibration) when the button is selected.")]
    public float activateHapticFeedback;

    [Header("Editor Inputs")]
    [Tooltip("Choose the controller key (or multiple keys) that will activate this button's function if pressed while hovering over the button.")]
    public List<controllerKeys> directInteractionButton = new List<controllerKeys>();

    [Tooltip("Choose the controller key (or multiple keys) that will activate this button's function even if the button is not directly targeted (like a shortcut).")]
    public List<controllerKeys> shortcutButton = new List<controllerKeys>();

    [Tooltip("Add the Input Manager Core Script.")]
    public InputSystemCoreScript inputManagerScript;

    [Tooltip("Choose the script and function to run when this button is activated.")]
    public UnityEvent triggeredFunction;

    [SerializeField, Tooltip("Choose the default texture of the button.")]
    private Texture2D defaultTexture;

    [SerializeField, Tooltip("Choose the texture to display when a selection ray hovers over the button.")]
    private Texture2D hoverTexture;

    [SerializeField, Tooltip("Choose the sound clip that is played when the button is activated.")]
    private AudioClip buttonActivateSoundClip;

    [Header("Public variables")]
    [Tooltip("States whether the button is currently hovered.")]
    public bool hoverState;



    void Start()
    {
        if(inputManagerScript = null)
        {
            inputManagerScript = FindObjectOfType<InputSystemCoreScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onPressed()
    {
        triggeredFunction.Invoke();
    }

    // DO NOT RENAME FUNCTION! This function is called as string in the InputSystemCoreScript.cs
    void OnHover()
    {
        this.GetComponentInChildren<Renderer>().material.mainTexture = hoverTexture;
    }

    void onHoverEnd()
    {
        this.GetComponentInChildren<Renderer>().material.mainTexture = defaultTexture;
    }
}
