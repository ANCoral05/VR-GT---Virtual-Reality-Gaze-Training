using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class TargetTrackingMain : MonoBehaviour
{
    [Header("Input variables")]

    [Range(1,30), Tooltip("Set the number of targets that move around on the screen.")]
    public int totalTargets = 10;

    [Range(1, 10), Tooltip("Set the number of targets that have to be tracked and identified by the user.")]
    public int markedTargets = 2;

    [Range(1, 10), Tooltip("Set the speed (in deg/s) at which the targets move across the scene.")]
    public float targetMovementSpeed = 2;

    [Range(1, 20), Tooltip("Sets the number of seconds of a trial before the targets stop moving and have to be identified.")]
    public float duration;

    [Header("Editor input")]

    [Header("Output variables")]

    [Header("Debug info")]

    [SerializeField, Tooltip("Displays the current step")]
    private CurrentStep currentStep;

    [Header("Private variables")]
    private float timer;
    public enum CurrentStep { menu, initiate, track, findTargets, restart };

    [Range(0, 100)]
    public int difficultyLevel = 0;

    public int progress = 0;

    public LayerMask targetLayer;

    public GameObject targetPrefab;

    public GameObject[] targets;

    public Texture2D markedImage;

    public Texture2D unmarkedImage;

    public GameObject infoCanvas;

    public GameObject nextButton;

    public GameObject backButton;

    public GameObject failButton;

    public GameObject[] controllerRays;

    bool failSelected;

    bool firstTrialDone;

    public TextMeshPro currentTrialText;

    public TextMeshPro resultText;

    public TextMeshPro difficultyText;

    public Image progressBar;

    public int correctTargetCounter;

    public int errors;

    public float timeForSelection;

    public float timeDuringTracking;

    public Material screenSizeMaterial;

    public Vector2 fieldDimensions;

    public AudioSource audioSource;

    public AudioClip successClip;

    public AudioClip errorClip;

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
}
