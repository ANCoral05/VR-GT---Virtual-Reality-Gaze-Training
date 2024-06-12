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
    [Tooltip("Enter the layer mask of the UI.")]
    public LayerMask UILayer;

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

    //Scripts
    [Tooltip("Enter the TrialManager Script.")]
    public TrialManager trialManager;

    //Others
    [Tooltip("Enter both visual pointer rays of the controllers that are disabled during trials.")]
    public GameObject[] controllerRays = new GameObject[2];

    [Tooltip("Enter the material of the background screen.")]
    public Material screenSizeMaterial;

    [Tooltip("Enter the debug text output.")]
    public TextMeshPro text;

    [Tooltip("Set the amount of blur for the scene to simulate low visual acuity.")]
    public float blurAmount;



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

    //The Dynamic Field of View (Dfov) value. Defines how much visual area was covered by the limited visual field over a fixed duration.
    float dfov;

    //Should be true if the last trial was selected as failed trial.
    bool failSelected;

    //Should be true if at least one trial was executed this session
    bool firstTrialDone;

    //Checks if the trigger of one of the VR controllers was pressed.
    bool hasPressedTrigger;

    //List of targets.
    [HideInInspector]
    public GameObject[] targets;

    //Progress until the next difficulty level is reached.
    [HideInInspector]
    public int progress = 0;

    [Tooltip("Enter the main camera, found in the VR player rig.")]
    public Camera cam;

    [Tooltip("Enter the VR_Input_Script from the ScriptManager.")]
    public VR_Input_Manager vrInput;






    //no scripts yet

    //public StoreTrialNumbers trialNumberScript;

    //public MeasurementTracking measurementScript;

    //public DFoV_Measurement dfovScript;

    //[SerializeField] ForwardRendererData forwardRendererData;



    void Start()
    {
        
    }

    void Update()
    {
        InputCheck();

        if(currentStep == CurrentStep.initiate)
        {
            InitiateTrial();
        }
    }

    void InputCheck()
    {
        if (vrInput.GetPrimaryButtonDown)
        {
            SceneManager.LoadScene(0);
        }
    }

    // This function initializes the trial by placing the required number of targets, 
    // marking some targets as "correct targets" and resetting all floats and ints from 
    // the last trial.
    private void InitiateTrial()
    {
        ResetParametersAfterTrial();

        AdjustScreensize();

        //Set the number of total targets (=all targets, with or without marker)
        if (targets.Length != totalTargets)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] != null)
                    targets[i].SetActive(false);
            }

            targets = new GameObject[totalTargets];
        }
        //Set all targets to unmarked first
        for (int i = 0; i < totalTargets; i++)
        {
            GameObject target;
            if (targets[i] == null)
            {
                target = GameObject.Instantiate(targetPrefab);
                target.transform.parent = transform;
                targets[i] = target;
            }
            else
            {
                target = targets[i];
            }

            TargetTracking_TargetMovement moveTarget = targets[i].GetComponentInChildren<TargetTracking_TargetMovement>();

            moveTarget.errorMarker.SetActive(false);

            target.tag = "incorrectTarget";
            //target.GetComponent<KartesianToRadial>().XY_angles = new Vector2(Random.Range(-fieldDimensions.x / 2, fieldDimensions.x / 2), Random.Range(-fieldDimensions.y / 2, fieldDimensions.y / 2));
            target.GetComponent<KartesianToRadialTransform>().XY_angles = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            target.GetComponent<KartesianToRadialTransform>().zDistance = 1.9f;

            target.SetActive(false);
        }
        //Add markers to specified number of targets
        for (int i = 0; i < markedTargets; i++)
        {
            TargetTracking_TargetMovement moveTarget = targets[i].GetComponentInChildren<TargetTracking_TargetMovement>();

            moveTarget.correctTargetIndicator.SetActive(true);

            targets[i].tag = "correctTarget";
        }

        //Remove cheese in case of difficulty downgrade
        targets[markedTargets].GetComponentInChildren<TargetTracking_TargetMovement>().correctTargetIndicator.SetActive(false);

        vrInput.GetMainTriggerDown = false;

        currentStep = CurrentStep.track;
    }

    private void TrackingPhase()
    {
        if (vrInput.controllerClickRaycast(UILayer).collider != null && vrInput.controllerClickRaycast(UILayer).transform.gameObject == nextButton)
        {

        }

            currentStep = CurrentStep.findTargets;
    }

    private void SelectionPhase()
    {

    }

    private void ResetParametersAfterTrial()
    {
        correctTargetCounter = 0;

        timeDuringTracking = 0;

        SetBlurValue(0);

        difficultyLevel = trialManager.GetDifficulty();

        //dfovScript.tunnelVision = false; // (Random.value > 0.5f);

        timer = duration * Random.Range(0.75f, 1.33f);

        totalTargets = 5 + Mathf.RoundToInt(difficultyLevel / 2.0f);

        markedTargets = 2 + Mathf.RoundToInt(0.25f + difficultyLevel / 20.0f);

        progress = trialManager.GetProgress();

        progressBar.fillAmount = progress / 100.0f;

        //Change Text of UI interface to show the updated parameters
        difficultyText.text = "Level: " + trialManager.GetLevel().ToString() + " (aktuelle Schwierigkeit: " + difficultyLevel.ToString() + ")";

        if (firstTrialDone)
        {
            currentTrialText.text = "Runden absolviert: " + (trialManager.GetCurrentTrials() + 1).ToString() + "/20";

            resultText.text = "Fehler: " + errors.ToString() + "\nSichtfeld: " + dfov.ToString("f1") + "%";
        }
        else
        {
            currentTrialText.text = "Runden absolviert: " + (trialManager.GetCurrentTrials()).ToString() + "/20";
        }
    }

    private void ProceedToNextStage()
    {

    }

    private void SetBlurValue(float blurValue)
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
