using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Text;
using System;

public class SaveAndLoadManager : MonoBehaviour
{
    [SerializeField, Tooltip("List of ScriptableObjects to be saved")]
    private List<ScriptableObject> storedParameterList = new List<ScriptableObject>();

    [SerializeField, Tooltip("Save File Name (If not set, default Save File Name is used)")]
    private string saveFileName;

    [SerializeField, Tooltip("Load File Name (If not set, default Load File Name is used)")]
    private string loadFileName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CreateNewSave();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadSave();
        }
    }

    // A function that takes data from scriptable objects and saves it to a json file
    public void CreateNewSave()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var scriptableObject in storedParameterList)
        {
            sb.AppendLine($"ScriptableObject: {scriptableObject.name}");
            var fields = scriptableObject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var properties = scriptableObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(scriptableObject);
                sb.AppendLine($"{field.Name}: {value}");
            }

            foreach (var property in properties)
            {
                if (property.CanRead)
                {
                    var value = property.GetValue(scriptableObject);
                    sb.AppendLine($"{property.Name}: {value}");
                }
            }
        }

        string saveData = sb.ToString();
        Debug.Log("Data saved!");

        // Save to file
        string saveFileName_ext = (saveFileName ?? "saveData") + ".json";
        System.IO.File.WriteAllText(saveFileName_ext, saveData);
    }

    // A function that sets the value of a field or property of a scriptable object
    private void SetFieldOrPropertyValue(object obj, string name, string value)
    {
        var type = obj.GetType();

        var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            var targetType = field.FieldType;
            var convertedValue = targetType.IsEnum
                ? Enum.Parse(targetType, value)
                : Convert.ChangeType(value, targetType);
            field.SetValue(obj, convertedValue);
            return;
        }

        var property = type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property != null && property.CanWrite)
        {
            var targetType = property.PropertyType;
            var convertedValue = targetType.IsEnum
                ? Enum.Parse(targetType, value)
                : Convert.ChangeType(value, targetType);
            property.SetValue(obj, convertedValue);
        }
    }

    // A function that reads data from a json file and assigns it to scriptable objects
    public void LoadSave()
    {
        string loadFileName_ext = (loadFileName ?? "saveData") + ".json";

        if (!System.IO.File.Exists(loadFileName_ext))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string loadData = System.IO.File.ReadAllText(loadFileName_ext);

        string[] lines = loadData.Split('\n');
        ScriptableObject currentScriptableObject = null;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("ScriptableObject:"))
            {
                string scriptableObjectName = line.Split(':')[1].Trim();
                currentScriptableObject = storedParameterList.Find(so => so.name == scriptableObjectName);
            }
            else if (currentScriptableObject != null)
            {
                string[] parts = line.Split(':');
                string fieldName = parts[0].Trim();
                string fieldValue = parts[1].Trim();
                SetFieldOrPropertyValue(currentScriptableObject, fieldName, fieldValue);
            }
        }

        Debug.Log("Data loaded!");
    }
}
