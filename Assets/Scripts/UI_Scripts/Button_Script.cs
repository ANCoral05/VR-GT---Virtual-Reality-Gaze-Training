using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button_Script : MonoBehaviour
{
    public UnityEvent triggeredFunction;

    public bool state;

    [SerializeField]
    private Texture2D hoverTexture;

    [SerializeField]
    private Texture2D idleTexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onPress()
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
        this.GetComponentInChildren<Renderer>().material.mainTexture = idleTexture;
    }
}
