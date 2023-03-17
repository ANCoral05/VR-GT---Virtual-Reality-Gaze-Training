using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class TargetTrackingMain : MonoBehaviour
{
    // ### Input variables ###
    [Header("Input variables")]

    [Range(1,30), Tooltip("Set the number of targets that move around on the screen.")]
    public int totalTargets = 10;

    [Range(1, 10), Tooltip("Set the number of targets that have to be tracked and identified by the user.")]
    public int markedTargets = 2;

    [Range(1, 10), Tooltip("Set the speed (in deg/s) at which the targets move across the scene.")]
    public float targetMovementSpeed = 2;

    [Range(1, 20), Tooltip("Set the number of seconds of a trial before the targets stop moving and have to be identified.")]
    public float duration;

    [Tooltip("Set the maximum field dimensions of the field in which the targets will randomly move around.")]
    public Vector2 fieldDimensions;

    [Range(1, 100), Tooltip("Set the level of difficulty for this task. Higher difficulty increases field size, movement speed, number of targets and number of marked targets gradually.")]
    public int difficultyLevel = 1;

    // ### Editor input ###
    [Header("Editor input")]

    //Targets
    [Tooltip("Enter the object layer of the targets.")]
    public LayerMask targetLayer;

    [Tooltip("Enter the prefab for the targets.")]
    public GameObject targetPrefab;

    [Tooltip("Enter the sprite for the marked targets.")]
    public Texture2D markedImage;

    [Tooltip("Enter the sprite for the unmarked targets.")]
    public Texture2D unmarkedImage;

    //UI and canvas
    [Tooltip("Enter the canvas for the UI interface.")]
    public GameObject infoCanvas;

    [Tooltip("Enter the button that starts the next trial.")]
    public GameObject nextButton;

    [Tooltip("Enter the button that brings you back to the menu.")]
    public GameObject backButton;

    [Tooltip("Enter the button that deletes the last trial.")]
    public GameObject failButton;

    [Tooltip("Enter the TextObject that displays the number of the current trial.")]
    public TextMeshPro currentTrialText;

    [Tooltip("Enter the TextObject that displays the results of the last trial.")]
    public TextMeshPro resultText;

    [Tooltip("Enter the TextObject that displays the current difficulty level.")]
    public TextMeshPro difficultyText;

    [Tooltip("Enter the Image for the progress bar. that fills with progress.")]
    public Image progressBar;

    //Sounds
    [Tooltip("Enter the audioSource from the ScriptManager.")]
    public AudioSource audioSource;

    [Tooltip("Enter the audioClip for selecting a correct target.")]
    public AudioClip successClip;

    [Tooltip("Enter the audioClip for selecting a wrong target.")]
    public AudioClip errorClip;

    //Others
    [Tooltip("Enter both visual pointer rays of the controllers that are disabled during trials.")]
    public GameObject[] controllerRays = new GameObject[2];

    [Tooltip("Enter the material of the background screen.")]
    public Material screenSizeMaterial;

    // ### Output variables ###
    [Header("Output variables")]

    [Tooltip("Shows the number of correctly identified targets.")]
    public int correctTargetCounter;

    [Tooltip("Shows the number of wrongly selected targets.")]
    public int errors;

    [Tooltip("Shows the current time spent in the findTargets stage.")]
    public float timeForSelection;

    [Tooltip("Shows the total duration during which the gaze rested on a marked target.")]
    public float timeDuringTracking;

    // ### Debug info ###
    [Header("Debug info")]

    [SerializeField, Tooltip("Displays the current stage of this script.")]
    private CurrentStep currentStep;

    enum CurrentStep { menu, initiate, track, findTargets, restart };

    // ### Private variables ###
    [Header("Private variables")]
    
    //Counts the time until
    private float timer;

    //Should be true if the last trial was selected as failed trial.
    bool failSelected;

    //Should be true if at least one trial was executed this session
    bool firstTrialDone;

    //List of targets.
    [HideInInspector]
    public GameObject[] targets;

    //Progress until the next difficulty level is reached.
    [HideInInspector]
    public int progress = 0;


    public Camera cam;

    //public ControllerClickManager vrInput;

    public LayerMask targetTrackingLayer;

    public LayerMask UILayer;

    public TextMeshPro text;

    bool hasPressedTrigger;

    float dfov;

    //no scripts yet

    //public StoreTrialNumbers trialNumberScript;

    //public MeasurementTracking measurementScript;

    //public DFoV_Measurement dfovScript;

    //[SerializeField] ForwardRendererData forwardRendererData;

    public float blurAmount;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Function to adjust screensize according to set maximum field size and difficulty modifier.
    private void AdjustScreensize()
    {
        float screenSize = 0.4f + 1.6f / (1f + 0.15f * difficultyLevel);

        fieldDimensions = new Vector2(180, 135) * 0.38f / screenSize;

        screenSizeMaterial.mainTextureScale = screenSize * new Vector2(1, 1);

        screenSizeMaterial.mainTextureOffset = new Vector2(1, 1) * 0.5f * (1 - screenSize);
    }
}
