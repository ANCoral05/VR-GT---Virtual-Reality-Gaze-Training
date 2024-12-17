using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Text;
using System;

public class DataCaptureManager : MonoBehaviour
{
    [SerializeField, Tooltip("List of ScriptableObjects to be saved")]
    private List<ScriptableObject> storedParameterList = new List<ScriptableObject>();

    [SerializeField, Tooltip("Define the delimiter between entries.")]
    private string delimiter = ";";

    [SerializeField, Tooltip("Set the sample capture frequency in 1/s. If set to 0, samples are captured once per frame.")]
    private float sampleCaptureRate = 200f;

    [SerializeField, Tooltip("Set the interval in which captured data is written to the file (this does not affect the sample capture rate, but can affect performance.")]
    private float captureInterval = 0.1f;

    [SerializeField, Tooltip("Toggle to add a row numbering each sample.")]
    private bool addRowNumbering = true;

    [SerializeField, Tooltip("Toggle to add a row with time stamps.")]
    private bool addTimeStamps = true;

    private int rowNumber = 0;

    private StringBuilder sb = new StringBuilder();

    // A function that continuously takes data from scriptable objects and saves it periodically to a .csv file, with the parameter names as the first row
    private void InitiateCaptureFile()
    {
        string filePath = Application.persistentDataPath + "/DataCapture.csv";

        // Build the first line
        string firstLine = "";
        if (addRowNumbering)
        {
            firstLine += "Row Number" + delimiter;
        }
        if (addTimeStamps)
        {
            firstLine += "Time Stamp" + delimiter;
        }
        foreach (var scriptableObject in storedParameterList)
        {
            var fields = scriptableObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                firstLine += field.Name + delimiter;
            }
        }

        // Write the first line to the file
        System.IO.File.WriteAllText(filePath, firstLine + Environment.NewLine);
    }    

    // Create a coroutine to capture data at very high frequency and save it to the string builder
    private IEnumerator CaptureData()
    {
        while (true)
        {
            if (sampleCaptureRate > 0)
            {
                yield return new WaitForSeconds(1.0f / sampleCaptureRate);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }

            // Build the row
            string row = "";
            if (addRowNumbering)
            {
                row += rowNumber + delimiter;
            }
            if (addTimeStamps)
            {
                row += Time.time + delimiter;
            }
            foreach (var scriptableObject in storedParameterList)
            {
                var fields = scriptableObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    var value = field.GetValue(scriptableObject);
                    row += value + delimiter;
                }
            }
            // Add the row to the string builder
            sb.AppendLine(row);
            rowNumber++;
        }
    }

    // Coroutine to write the data to the file at a set interval
    private IEnumerator WriteDataToFile()
    {
        while (true)
        {
            yield return new WaitForSeconds(captureInterval);
            string filePath = Application.persistentDataPath + "/DataCapture.csv";
            System.IO.File.AppendAllText(filePath, sb.ToString());
            sb.Clear();
        }
    }

    // Function to start the data capture
    public void StartDataCapture()
    {
        // Initiate the file
        InitiateCaptureFile();

        StartCoroutine(CaptureData());

        StartCoroutine(WriteDataToFile());
    }

    // Function to stop the data capture
    public void StopDataCapture()
    {
        StopAllCoroutines();
    }
}
