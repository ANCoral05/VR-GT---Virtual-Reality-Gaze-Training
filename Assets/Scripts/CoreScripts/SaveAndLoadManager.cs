using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Text;
using System;
using System.IO;


/// Summary:
/// Manages the saving and loading of data from ScriptableObjects to a JSON file.

/// Remarks:
/// The ScriptableObjects to be saved are added to the storedParameterList.
/// Only public or serialized private fields or properties of the ScriptableObjects are saved.
/// ScriptableObjects are identified by their name, and the data is saved in the order of the ScriptableObjects in the list.
/// Loading a file that uses a different order of ScriptableObjects than the current project can result in failure.

public class SaveAndLoadManager : MonoBehaviour
{
    // an enum to determine when to save or load, with options for "On variable change", "At fixed intervals", and "Manually"
    public enum SaveTrigger
    {
        Manually,
        OnVariableChange,
        AtFixedTimeIntervals,
        AtFixedFrameIntervals
    }

    [Header("Save and Load Settings")]
    [SerializeField, Tooltip("Set when to save the data. \nManually: Only when calling the CreateNewSave() method. \nOn Variable Change: Whenever one of the variables in the Stored Parameter List is changed. \nAt Fixed Time Intervals: Creates a save file every X seconds, determined in fixedTimeInterval. \nAt Fixed Frame Intervals: Creates a save file every X frames, determined in fixedFrameInterval.")]
    private SaveTrigger saveTrigger = SaveTrigger.Manually;

    [SerializeField, Tooltip("List of ScriptableObjects to be saved")]
    private List<ScriptableObject> storedParameterList = new List<ScriptableObject>();

    [SerializeField, Tooltip("Save File Name (If not set, default Save File Name is used)")]
    private string saveFileName;

    [SerializeField, Tooltip("Load File Name (If not set, default Load File Name is used)")]
    private string loadFileName;

    [SerializeField, Tooltip("Fixed time interval (in seconds) to store the save file.")]
    private float fixedTimeInterval = 10f;

    [SerializeField, Tooltip("Fixed frame interval to store the save file.")]
    private int fixedFrameInterval = 1;

    [SerializeField, Tooltip("If checked, a save file will be created on initialization. Otherwise, the first save file will be created as soon as the save trigger is met.")]
    private bool saveOnInitialization;

    private float fixedTimeStamp;

    private int fixedFrameStamp;

    private const string DefaultFileName = "saveData.json";

    public void Initialize()
    {
        if (saveTrigger == SaveTrigger.OnVariableChange)
        {
            SubscribeToOnChanged();
        }

        if (saveOnInitialization)
        {
            CreateNewSave();
        }
    }

    public void CreateNewSave()
    {
        var sb = new StringBuilder();
        foreach (var scriptableObject in storedParameterList)
        {
            AppendScriptableObjectData(sb, scriptableObject);
        }

        string saveData = sb.ToString();
        Debug.Log("Data saved!");

        string saveFileNameExt = string.IsNullOrEmpty(saveFileName) ? DefaultFileName : $"{saveFileName}.json";
        File.WriteAllText(saveFileNameExt, saveData);
    }

    private void AppendScriptableObjectData(StringBuilder sb, ScriptableObject scriptableObject)
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

    private void SetFieldOrPropertyValue(object obj, string name, string value)
    {
        var type = obj.GetType();

        var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            SetFieldValue(field, obj, value);
            return;
        }

        var property = type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property != null && property.CanWrite)
        {
            SetPropertyValue(property, obj, value);
        }
    }

    private void SetFieldValue(FieldInfo field, object obj, string value)
    {
        var targetType = field.FieldType;
        var convertedValue = ConvertValue(targetType, value);
        field.SetValue(obj, convertedValue);
    }

    private void SetPropertyValue(PropertyInfo property, object obj, string value)
    {
        var targetType = property.PropertyType;
        var convertedValue = ConvertValue(targetType, value);
        property.SetValue(obj, convertedValue);
    }

    private object ConvertValue(Type targetType, string value)
    {
        return targetType.IsEnum ? Enum.Parse(targetType, value) : Convert.ChangeType(value, targetType);
    }

    public void LoadSave()
    {
        string loadFileNameExt = string.IsNullOrEmpty(loadFileName) ? DefaultFileName : $"{loadFileName}.json";

        if (!File.Exists(loadFileNameExt))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string loadData = File.ReadAllText(loadFileNameExt);
        ParseAndLoadData(loadData);
        Debug.Log("Data loaded!");
    }

    private void ParseAndLoadData(string loadData)
    {
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
    }

    private void SubscribeToOnChanged()
    {
        foreach (ScriptableObject parameter in storedParameterList)
        {
            // TODO: Implement a way to subscribe to the OnChanged event of each variable component

            // subscribe to the OnChanged event of each variable component
            //if (parameter is IVariableComponent variableComponent)
            //{
            //    variableComponent.OnChanged += CreateNewSave;
            //}

            // subscribe to the OnChanged event of each variable component by finding the event by string
            var eventInfo = parameter.GetType().GetEvent("OnChanged");
            if (eventInfo != null)
            {
                var methodInfo = GetType().GetMethod("CreateNewSave", BindingFlags.NonPublic | BindingFlags.Instance);
                var action = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
                eventInfo.AddEventHandler(parameter, action);
            }
        }
    }

    private void FixedUpdate()
    {
        if (saveTrigger == SaveTrigger.AtFixedTimeIntervals && Time.fixedTime >= fixedTimeStamp )
        {
            CreateNewSave();
            fixedTimeStamp = Time.fixedTime + fixedTimeInterval;
        }
    }

    private void Update()
    {
        if (saveTrigger == SaveTrigger.AtFixedFrameIntervals && Time.frameCount >= fixedFrameStamp)
        {
            CreateNewSave();
            fixedFrameStamp = Time.frameCount + fixedFrameInterval;
        }
    }
}
