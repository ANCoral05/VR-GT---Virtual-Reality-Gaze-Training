using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadScript : MonoBehaviour
{
    [Header("Input Variables")]
    [SerializeField, Tooltip("Name of the scene to be loaded.")]
    private string sceneName;

    [SerializeField, Tooltip("The color to fade to/from during the transition.")]
    private Color fadeColor = Color.black;

    [SerializeField, Range(0, 3), Tooltip("Set the duration of the fade-out (set to 0 to turn it off).")]
    private float fadeOutDuration = 1;

    [SerializeField, Range(0, 3), Tooltip("Set the duration of the fade-in for the next scene (set to 0 to turn it off).")]
    private float fadeInDuration = 1;

    [Header("Editor Inputs")]
    [SerializeField, Tooltip("Reference to the Canvas used for fading.")]
    private GameObject canvasObject;

    [SerializeField, Tooltip("Reference to the Image used for the fade effect.")]
    private Image fadeImage;

    private void Start()
    {
        InitializeCanvas();

        print("hello!");

        StartCoroutine(AutoLoadScenes());
    }

    private IEnumerator AutoLoadScenes()
    {
        print("0sec");

        yield return new WaitForSeconds(5f);

        print("5sec");

        LoadNextScene();
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

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNextScene()
    {
        float timeForFadeCompleted = Time.time + fadeInDuration;

        canvasObject.SetActive(true);

        print("load scene");

        StartCoroutine(FadeOutAndIn(sceneName, fadeColor, fadeOutDuration, fadeInDuration));
    }

    private IEnumerator FadeOutAndIn(string sceneName, Color fadeColor, float fadeOutDuration, float fadeInDuration)
    {
        print("fade out in start");

        if (fadeOutDuration > 0f)
        {
            canvasObject.SetActive(true);

            yield return StartCoroutine(FadeOut(fadeColor, fadeOutDuration));
        }

        print("after fade out");

        SceneManager.LoadScene(sceneName);

        if (fadeInDuration > 0f)
        {
            canvasObject.SetActive(true);

            yield return StartCoroutine(FadeIn(fadeColor, fadeInDuration));
        }

        print("after fade in");

        canvasObject.SetActive(false);

        Destroy(this.gameObject);
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

    private IEnumerator FadeIn(Color fadeColor, float fadeDuration)
    {
        float fadeEndTime = Time.time + fadeDuration;

        while (Time.time < fadeEndTime)
        {
            float fadeLerpValue = (fadeEndTime - Time.time) / fadeInDuration;
            Color currentFadeColor = Color.Lerp(new Color(0f, 0f, 0f, 0f), fadeColor, fadeLerpValue);
            fadeImage.color = currentFadeColor;

            yield return null;
        }
    }
}
