using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_System_Core_Script : MonoBehaviour
{
    [Header("Accessible Variables")]
    public Vector3 direction_left_controller;
    public Vector3 direction_right_controller;

    public Vector3 origin_left_controller;
    public Vector3 origin_right_controller;

    public GameObject hovered_object;

    //List of subscribed objects that are informed whenever a controller hovers over a new object.
    public List<GameObject> hover_listeners = new List<GameObject>();

    public List<GameObject> button_trigger_listeners = new List<GameObject>();
    public List<GameObject> button_primary_listeners = new List<GameObject>();

    public InputActionReference triggerInputActionReference;

    public bool getTrigger()
    {
        return triggerInputActionReference.action.ReadValue<bool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
