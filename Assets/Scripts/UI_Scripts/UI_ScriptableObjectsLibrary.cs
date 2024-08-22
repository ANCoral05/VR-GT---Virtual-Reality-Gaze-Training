using UnityEngine;

[CreateAssetMenu(fileName = "Controller settings", menuName = "ScriptableObjects/UI and Input", order = 1)]
public class ControllerSettings_SO : ScriptableObject
{
    public enum ActiveControllerSelection
    {
        Both,
        LastPressed,
        Left,
        Right,
        None
    }

    public ActiveControllerSelection actionControllerSelection = ActiveControllerSelection.Both;
}

public class UI_ScriptableObjectsLibrary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
