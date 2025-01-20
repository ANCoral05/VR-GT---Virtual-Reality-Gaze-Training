using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRK_BuildingBlocks;

public class TargetSelectionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject shootingSphere;

    [SerializeField]
    private IntVariable score;

    public float chargeInterval = 0.75f;

    [SerializeField]
    private List<Material> materials = new List<Material>();

    [SerializeField]
    private int maxLevelCharge = 3;

    [SerializeField]
    private AnimationCurve emissionCurve = new AnimationCurve();

    [SerializeField]
    private float emissionStrength = 2f;

    [SerializeField, Range(0f, 300f)]
    private int speedIncreasePerMinute = 20;

    [SerializeField]
    private ParticleSystem chargeParticles;

    [SerializeField]
    private MeshRenderer[] modifiedMeshRenderers = new MeshRenderer[0];

    [SerializeField]
    private GameObject unshatteredObject;

    [SerializeField]
    private GameObject shatteredObject;

    private MaterialPropertyBlock mpb;

    private Color baseColor = new Color();

    private Color baseEmissionColor = new Color();

    private int chargeLevel = 0;

    private float chargeEventTime = 0;

    private Vector3 rotationAxis;

    private float rotationSpeed;

    private List<Transform> shatteredObjectChildren = new List<Transform>();

    public void Start()
    {
        Initialize();

        foreach (Transform child in shatteredObject.transform)
        {
            if (child != null)
            {
                shatteredObjectChildren.Add(child);

                Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = child.gameObject.AddComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                    }
                }
            }
        }
    }

    public void Initialize()
    {
        // Set private fields to starting values
        if (baseColor == null)
        {
            baseColor = new Color();
        }

        if (baseEmissionColor == null)
        {
            baseEmissionColor = new Color();
        }

        if (shootingSphere == null)
        {
            shootingSphere = GameObject.Find("ShooterSphere");
        }

        chargeLevel = 0;

        chargeEventTime = 0;

        SetRotation();

        SetMaterialPropertyBlock();

        UpdateMaterialProperties();
    }

    // set the transform of all children of the shattered object to the stored values
    public void SetShatteredObjectChildrenTransforms()
    {
        for (int i = 0; i < shatteredObject.transform.childCount; i++)
        {
            Transform child = shatteredObject.transform.GetChild(i);
            child.position = shatteredObjectChildren[i].position;
            child.rotation = shatteredObjectChildren[i].rotation;
        }
    }

    private void SetMaterialPropertyBlock()
    {
        if(mpb == null)
        {
            mpb = new MaterialPropertyBlock();
        }

        ApplyMaterialPropertyBlock();
    }

    public void OnClick()
    {
        shootingSphere.GetComponent<EntityMovementScript>().SetNewTargetCourse(this.transform);
    }

    public void OnHit()
    {
        score.value += chargeLevel;

        // disable unshattered object and enable shattered object
        unshatteredObject.SetActive(false);
        shatteredObject.SetActive(true);

        chargeParticles.Play();

        // add force to all sub-objects of the shattered object, originating from the point of the shooting sphere
        foreach (Transform child in shatteredObject.transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce((child.position - shootingSphere.transform.position).normalized * 10f, ForceMode.Impulse);
            }
        }

        // disable this object after 3 seconds
        StartCoroutine(DisableAfterSeconds(3f));
    }

    // ienumerator that disables this object after 3 seconds
    public IEnumerator DisableAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);

        shatteredObject.SetActive(false);
        unshatteredObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void UpdateMaterialProperties()
    {
        if (chargeLevel < materials.Count)
        {
            baseColor = materials[chargeLevel].color;
            baseEmissionColor = materials[chargeLevel].GetColor("_EmissionColor");

            // set color of the material property block
            mpb.SetColor("_BaseColor", baseColor);

            if (chargeLevel != 0)
            {
                chargeParticles.Play();
            }
        }
    }

    public void Update()
    {
        if (chargeLevel < maxLevelCharge || chargeEventTime <= 0.8f)
        {
            chargeEventTime += Time.deltaTime;
        }

        if (chargeEventTime >= chargeInterval && chargeLevel < maxLevelCharge)
        {
            chargeLevel += 1;

            UpdateMaterialProperties();

            ApplyMaterialPropertyBlock();

            chargeEventTime = 0;
        }

        if((shootingSphere.transform.position - this.transform.position).magnitude <= 0.07f)
        {
            OnHit();
        }

        UpdateEmissionIntensity();

        ApplyMaterialPropertyBlock();

        this.transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    // Function that lets the object rotate along a random axis
    public void SetRotation()
    {
        rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rotationSpeed = Random.Range(60f, 180f);
    }

    private void ApplyMaterialPropertyBlock()
    {
        foreach (MeshRenderer meshRenderer in modifiedMeshRenderers)
        {
            meshRenderer.SetPropertyBlock(mpb);
        }
    }

    private void OnEnable()
    {
        Initialize();
    }


    private void UpdateEmissionIntensity()
    {
        // float emissionIntensity = GetYFromLineGraph(lineGraph, chargeEventTime / chargeInterval);

        float emissionIntensity = emissionStrength * emissionCurve.Evaluate(chargeEventTime / chargeInterval);

        mpb.SetColor("_EmissionColor", baseEmissionColor * emissionIntensity);

        this.transform.localScale = (1f + chargeLevel*0.25f) * new Vector3(0.9f + 0.1f * emissionIntensity, 0.9f + 0.1f * emissionIntensity, 0.9f + 0.1f * emissionIntensity);
    }

    private float Pulsate(float pulseMin, float pulseMax, float pulseInterval, float timeOffset)
    {
        float pulsateValue = Mathf.Sin((Time.time + timeOffset) * 2 * Mathf.PI / pulseInterval) * (pulseMax - pulseMin) * 0.5f + pulseMin;

        return pulsateValue;
    }
}

