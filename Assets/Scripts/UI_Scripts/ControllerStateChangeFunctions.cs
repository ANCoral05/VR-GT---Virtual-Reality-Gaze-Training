using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStateChangeFunctions : MonoBehaviour
{
    [SerializeField]
    private InputSystemCoreScript inputSystemCoreScript;

    void Start()
    {
        if(inputSystemCoreScript == null)
        {
            inputSystemCoreScript = FindObjectOfType<InputSystemCoreScript>();
        }
    }
    
    public void ChangeControllerStateBoth()
    {
        inputSystemCoreScript.SetControllerActiveStatus(ActiveControllerSelection.Both);
    }

    public void ChangeControllerStateLeft()
    {
        inputSystemCoreScript.SetControllerActiveStatus(ActiveControllerSelection.Left);
    }

    public void ChangeControllerStateRight()
    {
        inputSystemCoreScript.SetControllerActiveStatus(ActiveControllerSelection.Right);
    }

    public void ChangeControllerStateLastPressed()
    {
        inputSystemCoreScript.SetControllerActiveStatus(ActiveControllerSelection.LastPressed);
    }
}
