using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Script : MonoBehaviour
{
    [SerializeField]
    private Texture2D hover_texture;

    [SerializeField]
    private Texture2D idle_texture;

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

    }

    void onHover()
    {
        this.GetComponentInChildren<Renderer>().material.mainTexture = hover_texture;
    }

    void onHoverEnd()
    {
        this.GetComponentInChildren<Renderer>().material.mainTexture = idle_texture;
    }
}
