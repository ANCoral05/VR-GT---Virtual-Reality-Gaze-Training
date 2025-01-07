using UnityEngine;
using UnityEditor;

namespace VRK_BuildingBlocks
{
    [CustomPropertyDrawer(typeof(BoolVariable))]
    [CustomPropertyDrawer(typeof(FloatVariable))]
    [CustomPropertyDrawer(typeof(IntVariable))]
    [CustomPropertyDrawer(typeof(StringVariable))]
    [CustomPropertyDrawer(typeof(Vector2Variable))]
    [CustomPropertyDrawer(typeof(Vector3Variable))]
    [CustomPropertyDrawer(typeof(Vector4Variable))]
    [CustomPropertyDrawer(typeof(ArrayVariable<>))]
    public class VariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the object field
            position.width -= 70; // Reserve space for the button
            EditorGUI.PropertyField(position, property, label);

            // Draw the "Create" button
            position.x += position.width + 5;
            position.width = 65;
            if (GUI.Button(position, "Create"))
            {
                // Create and assign the ScriptableObject
                ScriptableObject newAsset = CreateScriptableObjectInSelectedFolder(property);
                if (newAsset != null)
                {
                    property.objectReferenceValue = newAsset;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private ScriptableObject CreateScriptableObjectInSelectedFolder(SerializedProperty property)
        {
            // Get the type of the field
            System.Type type = fieldInfo.FieldType;

            // Handle generic types
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ArrayVariable<>))
            {
                type = typeof(ArrayVariable<>).MakeGenericType(type.GetGenericArguments());
            }

            // Ensure the type is a ScriptableObject
            if (!typeof(ScriptableObject).IsAssignableFrom(type))
            {
                Debug.LogError($"Type {type} is not a ScriptableObject.");
                return null;
            }

            // Create the ScriptableObject instance
            ScriptableObject newAsset = ScriptableObject.CreateInstance(type);

            // Use the property's display name as the asset name, converting to PascalCase
            string assetName = ObjectNames.NicifyVariableName(property.name).Replace(" ", "");
            string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(newAsset));

            if (string.IsNullOrEmpty(scriptPath))
            {
                Debug.LogError($"Could not find script path for {type}.");
                scriptPath = "Assets"; // Fallback path
            }

            string folderPath;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ArrayVariable<>))
            {
                folderPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(scriptPath), "ArrayVariableAssets");
            }
            else
            {
                folderPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(scriptPath), $"{type.Name}Assets");
            }

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(scriptPath), $"{type.Name}Assets");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{assetName}.asset");

            Debug.Log(newAsset);

            Debug.Log(assetPath);

            // Save the asset
            AssetDatabase.CreateAsset(newAsset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return newAsset;
        }
    }
}


