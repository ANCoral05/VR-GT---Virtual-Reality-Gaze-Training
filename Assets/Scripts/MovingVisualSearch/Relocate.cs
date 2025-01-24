using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Relocate : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject canvasObject;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private Color fadeColor = Color.black;

    public void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("XR Origin (XR Rig)").transform;
        }
    }

    private void Start()
    {
        InitializeCanvas();
    }

    private void InitializeCanvas()
    {
        if (canvasObject == null)
        {
            canvasObject = new GameObject("FadeCanvas");
            Canvas fadeCanvas = canvasObject.AddComponent<Canvas>();
            fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            fadeCanvas.sortingOrder = 99;
            canvasObject.AddComponent<CanvasGroup>();
        }

        canvasObject.SetActive(false);
        canvasObject.transform.SetParent(this.transform);

        if (fadeImage == null)
        {
            GameObject imageObject = new GameObject("FadeImage");
            imageObject.transform.SetParent(canvasObject.transform);
            fadeImage = imageObject.AddComponent<Image>();
            fadeImage.rectTransform.anchorMin = new Vector2(0, 0);
            fadeImage.rectTransform.anchorMax = new Vector2(1, 1);
            fadeImage.rectTransform.offsetMin = Vector2.zero;
            fadeImage.rectTransform.offsetMax = Vector2.zero;
            fadeImage.color = new Color(0f, 0f, 0f, 0f);
        }

        this.transform.SetParent(null);

        DontDestroyOnLoad(this.gameObject);
    }

    public void RelocatePlayer()
    {
        FadeOutAndIn(fadeColor, 1f, 1f);

    }

    private IEnumerator FadeOutAndIn(Color fadeColor, float fadeOutDuration, float fadeInDuration)
    {
        if (fadeOutDuration > 0f)
        {
            canvasObject.SetActive(true);

            yield return StartCoroutine(FadeOut(fadeColor, fadeOutDuration));
        }

        player.transform.position = target.position;

        if (fadeInDuration > 0f)
        {
            canvasObject.SetActive(true);

            yield return StartCoroutine(FadeIn(fadeColor, fadeInDuration));
        }

        canvasObject.SetActive(false);
    }

    private IEnumerator FadeIn(Color fadeColor, float fadeDuration)
    {
        float fadeEndTime = Time.time + fadeDuration;

        while (Time.time < fadeEndTime)
        {
            float fadeLerpValue = (fadeEndTime - Time.time) / fadeDuration;
            Color currentFadeColor = Color.Lerp(new Color(0f, 0f, 0f, 0f), fadeColor, fadeLerpValue);
            fadeImage.color = currentFadeColor;

            yield return null;
        }
    }

    private IEnumerator FadeOut(Color fadeColor, float fadeDuration)
    {
        float fadeEndTime = Time.time + fadeDuration;

        while (Time.time < fadeEndTime)
        {
            float fadeLerpValue = (fadeEndTime - Time.time) / fadeDuration;
            Color currentFadeColor = Color.Lerp(fadeColor, new Color(0f, 0f, 0f, 0f), fadeLerpValue);
            fadeImage.color = currentFadeColor;

            yield return null;
        }
    }
}
