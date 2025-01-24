using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleDebug : MonoBehaviour
{
    [SerializeField]
    private GameObject debugCanvas;


    public void ToggleDebugFunction()
    {
        debugCanvas.SetActive(!debugCanvas.activeSelf);
    }
}

