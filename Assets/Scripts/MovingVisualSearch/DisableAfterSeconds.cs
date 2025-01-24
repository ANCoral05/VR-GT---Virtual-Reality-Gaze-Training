using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private float disableAfterSeconds = 3f;

    [SerializeField]
    private ParticleSystem particleSystem;

    // function to disable the game object after a certain amount of time
    private void OnEnable()
    {
        StartCoroutine(DisableAfterSecondsCoroutine());

        particleSystem.Play();
    }

    private IEnumerator DisableAfterSecondsCoroutine()
    {
        yield return new WaitForSeconds(disableAfterSeconds);
        this.gameObject.SetActive(false);
    }
}
