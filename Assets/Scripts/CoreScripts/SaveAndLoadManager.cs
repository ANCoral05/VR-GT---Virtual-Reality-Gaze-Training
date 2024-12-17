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
        Debug.Log(saveData);

        // Save to file
        System.IO.File.WriteAllText("saveData.json", saveData);
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
        string loadData = System.IO.File.ReadAllText("saveData.json");

        string[] lines = loadData.Split('\n');
        string currentScriptableObject = "";

        foreach (var line in lines)
        {
            if (line.Contains("ScriptableObject"))
            {
                currentScriptableObject = line.Split(':')[1].Trim();
            }
            else
            {
                if(string.IsNullOrEmpty(line))
                {
                    continue;
                }
                string[] parts = line.Split(':');
                string fieldName = parts[0].Trim();
                string fieldValue = parts[1].Trim();
                foreach (var scriptableObject in storedParameterList)
                {
                    if (scriptableObject.name == currentScriptableObject)
                    {
                        SetFieldOrPropertyValue(scriptableObject, fieldName, fieldValue);
                    }
                }
            }
        }
    }
}
