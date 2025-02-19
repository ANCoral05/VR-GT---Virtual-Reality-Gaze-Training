using System.Collections;
using UnityEngine;

public class OutlineComponentManager : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private float outlineGrowDuration = 0.5f;
    [SerializeField] private float outlineHighlightDuration = 0.5f;
    [SerializeField] private float outlineShrinkDuration = 0.5f;
    [SerializeField] private AnimationCurve outlineGrowCurve;
    [SerializeField] private AnimationCurve outlineHighlightCurve;
    [SerializeField] private AnimationCurve outlineShrinkCurve;

    private float baseOutlineWidth;
    private Coroutine outlineCoroutine;

    private void Start()
    {
        baseOutlineWidth = outline.OutlineWidth;
        outline.OutlineWidth = 0f;
    }

    [ContextMenu("Toggle Outline On")]
    public void ToggleOutlineOn()
    {
        if (outlineCoroutine != null)
            StopCoroutine(outlineCoroutine);

        outlineCoroutine = StartCoroutine(AnimateOutline(outlineGrowCurve, outlineGrowDuration, baseOutlineWidth));
    }

    [ContextMenu("Toggle Outline Highlight")]
    public void ToggleOutlineHighlight()
    {
        if (outlineCoroutine != null)
            StopCoroutine(outlineCoroutine);
        outlineCoroutine = StartCoroutine(AnimateOutline(outlineHighlightCurve, outlineHighlightDuration, baseOutlineWidth));
    }

    [ContextMenu("Toggle Outline Off")]
    public void ToggleOutlineOff()
    {
        if (outlineCoroutine != null)
            StopCoroutine(outlineCoroutine);

        outlineCoroutine = StartCoroutine(AnimateOutline(outlineShrinkCurve, outlineShrinkDuration, 0f));
    }

    private IEnumerator AnimateOutline(AnimationCurve curve, float duration, float targetWidth)
    {
        float startWidth = outline.OutlineWidth;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            outline.OutlineWidth = baseOutlineWidth*curve.Evaluate(t);
            yield return null;
        }

        outline.OutlineWidth = targetWidth;
    }
}