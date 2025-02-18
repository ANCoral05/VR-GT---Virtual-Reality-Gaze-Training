using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GazeQuestUtils;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

public class ActionEventScript : MonoBehaviour
{
    [Header("Input variables")]
    [Tooltip("Choose the script and function to run when this button is activated/toggled on.")]
    public UnityEvent onClick;

    public UnityEvent onClickRelease;

    public UnityEvent onHoverStart;

    public UnityEvent onHoverEnd;

    public UnityEvent onToggleOn;

    [Tooltip("Choose the script and function to run when this button is toggled off.")]
    public UnityEvent onToggleOff;

    [Tooltip("Choose the controller key (or multiple keys) that will activate this button's function if pressed while hovering over the button.")]
    public List<ControllerKey> directInteractionKey = new List<ControllerKey>();

    [Tooltip("Choose the controller key (or multiple keys) that will activate this button's function even if the button is not directly targeted (like a shortcut).")]
    public List<ControllerKey> shortcutKey = new List<ControllerKey>();

    [Range(0, 5), Tooltip("Delay in seconds until the function is called after the button is pressed.")]
    public float delayTime;

    [Range(0, 1), Tooltip("Set the strength of haptic feedback (vibration) when the selection ray starts hovers over the button.")]
    public float hoverHapticFeedback;

    [Range(0, 1), Tooltip("Set the strength of haptic feedback (vibration) when the button is selected.")]
    public float activateHapticFeedback;

    [Tooltip("Check this if the button should remain toggled on until pressed again.")]
    public bool isToggle;

    [Header("Editor Inputs")]
    [Tooltip("Add the Input Manager Core Script.")]
    public InputSystemCoreScript inputManagerScript;

    [SerializeField, Tooltip("Choose the default visible object of the button.")]
    private GameObject defaultItem;

    [SerializeField, Tooltip("Choose the visible object to display when a selection ray hovers over the button.")]
    private GameObject hoverItem;

    [SerializeField, Tooltip("Choose the visible object to display when the button is clicked.")]
    private GameObject isPressedItem;

    [SerializeField, Tooltip("Choose the visible object to display while the button is toggled on.")]
    private GameObject isToggledOnItem;

    [SerializeField, Tooltip("Choose the visible object to display when the button is disabled/not clickable.")]
    private GameObject isDisabledItem;

    [SerializeField, Tooltip("Choose the sound clip that is played when the button is activated.")]
    private AudioClip buttonActivateSoundClip;

    [SerializeField, Tooltip("Choose the sound clip that is played when the button is hovered over.")]
    private AudioClip buttonHoverSoundClip;

    [Header("Public variables")]
    [Tooltip("States whether the button is currently hovered.")]
    public int hoverRayCount;



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

    [ContextMenu("Activate Button")]
    public void ContextMenuClick()
    {
        onClick.Invoke();
    }

    public void OnPressed(ControllerKey inputKey)
    {
        if(hoverRayCount >= 1)
        {
            foreach (ControllerKey testKey in directInteractionKey)
            {
                if(inputKey == testKey)
                {
                    onClick.Invoke();
                }
            }
        }

        foreach (ControllerKey testKey in shortcutKey)
        {
            if (inputKey == testKey)
            {
                onClick.Invoke();
            }
        }
    }

    public void OnToggledOff()
    {
        DeactivateAllVisuals();

        GazeQuest_Methods.ActivateObject(hoverItem);
    }

    public void OnHover()
    {
        onHoverStart.Invoke();

        hoverRayCount += 1;

        if(hoverRayCount == 1)
        {
            onHoverStart.Invoke();
        }
    }

    public void OnHoverEnd()
    {
        hoverRayCount -= 1;

        if (hoverRayCount == 0)
        {
            onHoverEnd.Invoke();
        }
    }

    private void DeactivateAllVisuals()
    {
        GazeQuest_Methods.DeactivateObject(defaultItem);

        GazeQuest_Methods.DeactivateObject(hoverItem);

        GazeQuest_Methods.DeactivateObject(isPressedItem);

        GazeQuest_Methods.DeactivateObject(isToggledOnItem);

        GazeQuest_Methods.DeactivateObject(isDisabledItem);
    }

    private void OnEnable()
    {
        inputManagerScript.inputListeners.Add(this);
    }

    private void OnDisable()
    {
        inputManagerScript.inputListeners.Remove(this);
    }

    public void ChangeObjectOnHover()
    {
        DeactivateAllVisuals();

        GazeQuest_Methods.ActivateObject(hoverItem);
    }

    public void ChangeObjectOnHoverBack()
    {
        DeactivateAllVisuals();

        GazeQuest_Methods.ActivateObject(defaultItem);
    }
}
