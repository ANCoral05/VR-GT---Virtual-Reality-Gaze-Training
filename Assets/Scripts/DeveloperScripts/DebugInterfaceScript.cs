using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.InputSystem.Android;

public class DebugInterfaceScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI debugLogTextMesh;

    [SerializeField]
    private TextMeshProUGUI fpsTextMesh;

    [SerializeField]
    private int maxLines = 10;

    [SerializeField]
    private bool showStackTrace = true;

    private Queue<string> queue = new Queue<string>();
    private string currentText = "";
    private int lineCounter;
    private int fpsCounter;
    private float fpsDurationSum;

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += UpdateDebugLogInterface;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= UpdateDebugLogInterface;
    }

    void UpdateDebugLogInterface(string logString, string stackTrace, LogType type)
    {
        // Delete oldest message
        if (queue.Count >= maxLines)
        {
            queue.Dequeue();
            queue.Dequeue();
        }

        lineCounter++;

        queue.Enqueue(logString);

        if (showStackTrace)
            queue.Enqueue(stackTrace);

        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(lineCounter.ToString() + ": ").Append(st).Append("\n");
        }

        currentText = builder.ToString();

        debugLogTextMesh.text = currentText;
    }

    private void Update()
    {
        fpsDurationSum += Time.deltaTime;
        fpsCounter++;

        if(fpsCounter >=10)
        {
            float averageFPS = 1 / (fpsDurationSum / fpsCounter);
            fpsTextMesh.text = averageFPS.ToString("F1") + " fps";
            fpsCounter = 0;
            fpsDurationSum = 0;
        }
    }

    // Function to toggle the stack trace
    public void ToggleStackTrace()
    {
        showStackTrace = !showStackTrace;
    }

    // Function to clear the debug log
    public void ClearDebugLog()
    {
        queue.Clear();
        currentText = "";
        debugLogTextMesh.text = currentText;
    }

    // Function to open or close the debug log interface
    public void ToggleDebugLogInterface()
    {
        if (debugLogTextMesh.gameObject.activeSelf)
        {
            StartCoroutine(ToggleDebugLogInterface(new Vector3(1, 0, 1), new Vector3(1, 1, 1), 0.2f));
        }
        else
        {
            StartCoroutine(ToggleDebugLogInterface(new Vector3(1, 1, 1), new Vector3(1, 0, 1), 0.2f));
        }
    }

    // Coroutine to grow or shrink the debug log interface
    private IEnumerator ToggleDebugLogInterface(Vector2 startSize, Vector2 endSize, float duration)
    {
        debugLogTextMesh.gameObject.SetActive(true);
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            debugLogTextMesh.rectTransform.localScale = Vector3.Lerp(startSize, endSize, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        debugLogTextMesh.transform.localScale = endSize;

        if (endSize.y == 0)
        {
            debugLogTextMesh.gameObject.SetActive(!debugLogTextMesh.gameObject.activeSelf);
        }
    }
}
