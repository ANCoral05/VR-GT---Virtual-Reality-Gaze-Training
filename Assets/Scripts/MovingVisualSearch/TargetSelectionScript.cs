using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VRK_BuildingBlocks;
using GazeQuestUtils;

public class TargetSelectionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject shootingSphere;

    [SerializeField]
    private IntVariable score;

    [SerializeField]
    private float chargeInterval = 0.75f;

    [SerializeField]
    private List<Material> materials = new List<Material>();

    [SerializeField]
    private int maxLevelCharge = 3;

    [SerializeField]
    private List<Vector2> lineGraph = new List<Vector2>();

    [SerializeField]
    private ParticleSystem chargeParticles;

    public Material material;

    private Color baseColor = new Color();

    private Color baseEmissionColor = new Color();

    private int chargeLevel = 0;

    private float chargeEventTime = 0;

    private List<GameObject> childrenInHierarchy = new List<GameObject>();

    public void Start()
    {
        Initialize(this);
    }

    public void Initialize(TargetSelectionScript prefabSettings)
    {
        // Set all serialized or public fields to the prefab settings
        foreach (var field in prefabSettings.GetType().GetFields())
        {
            field.SetValue(this, field.GetValue(prefabSettings));
        }

        // Set private fields to starting values
        baseColor = new Color();

        baseEmissionColor = new Color();

        chargeLevel = 0;

        chargeEventTime = 0;

        childrenInHierarchy = GazeQuestUtilityFunctions.GetDescendents(this.gameObject);

        SetChildMaterials();

        UpdateMaterial();
    }

    private void SetChildMaterials()
    {
        if (materials.Count == 0)
        {
            Debug.LogError("No materials set. Please assign at least material to the materials list.");

            return;
        }

        material = new Material(materials[0]);

        foreach (GameObject child in childrenInHierarchy)
        {
            if (child.transform.GetComponent<MeshRenderer>() != null)
            {
                child.transform.GetComponent<MeshRenderer>().material = material;
            }
        }
    }

    public static float GetYFromLineGraph(List<Vector2> points, float t)
    {
        if (points == null || points.Count < 2)
            throw new System.ArgumentException("Points list must contain at least two points.");

        points.Sort((a, b) => a.x.CompareTo(b.x));

        for (int i = 0; i < points.Count - 1; i++)
        {
            if (t >= points[i].x && t <= points[i + 1].x)
            {
                // Define control points: midpoint between two points as control
                Vector2 start = points[i];
                Vector2 end = points[i + 1];
                Vector2 control = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);

                float tFraction = (t - start.x) / (end.x - start.x);

                // Quadratic Bezier interpolation
                return Mathf.Pow(1 - tFraction, 2) * start.y + 2 * (1 - tFraction) * tFraction * control.y + Mathf.Pow(tFraction, 2) * end.y;
            }
        }

        // Handle cases where t is out of range.
        if (t < points[0].x)
            return points[0].y;
        if (t > points[points.Count - 1].x)
            return points[points.Count - 1].y;

        return 0f; // Default case, should not be reached.
    }

    public void OnClick()
    {
        shootingSphere.GetComponent<EntityMovementScript>().SetNewTargetCourse(this.transform);
    }

    public void OnHit()
    {
        score.value++;
        shootingSphere.GetComponent<EntityMovementScript>().SetNewTargetCourse(this.transform);
    }

    private void UpdateMaterial()
    {
        if (chargeLevel < materials.Count)
        {
            baseColor = materials[chargeLevel].color;
            baseEmissionColor = materials[chargeLevel].GetColor("_EmissionColor");

            material.color = baseColor;
            material.SetColor("_EmissionColor", baseEmissionColor);

            chargeParticles.Play();

            // check if the material has the emission enabled
            if (materials[chargeLevel].IsKeywordEnabled("_EMISSION"))
                material.EnableKeyword("_EMISSION");
        }
    }

    public void Update()
    {
        chargeEventTime += Time.deltaTime;
        if (chargeEventTime >= chargeInterval && chargeLevel < maxLevelCharge)
        {
            chargeLevel += 1;

            UpdateMaterial();

            chargeEventTime = 0;
        }

        UpdateEmissionIntensity();
    }

    private void UpdateEmissionIntensity()
    {
        float emissionIntensity = GetYFromLineGraph(lineGraph, chargeEventTime / chargeInterval);

        material.SetColor("_EmissionColor", baseEmissionColor * emissionIntensity);

        this.transform.localScale = new Vector3(0.9f + 0.1f * emissionIntensity, 0.9f + 0.1f * emissionIntensity, 0.9f + 0.1f * emissionIntensity);
    }

    private float Pulsate(float pulseMin, float pulseMax, float pulseInterval, float timeOffset)
    {
        float pulsateValue = Mathf.Sin((Time.time + timeOffset) * 2 * Mathf.PI / pulseInterval) * (pulseMax - pulseMin) * 0.5f + pulseMin;

        return pulsateValue;
    }
}

