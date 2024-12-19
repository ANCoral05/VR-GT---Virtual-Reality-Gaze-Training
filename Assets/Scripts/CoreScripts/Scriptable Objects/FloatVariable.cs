using UnityEngine;
using UnityEditor;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Variables/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        public float value;
    }

    [CustomPropertyDrawer(typeof(FloatVariable))]
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

            // Create the ScriptableObject instance
            ScriptableObject newAsset = ScriptableObject.CreateInstance(type);


            // Use the property's display name as the asset name, converting to PascalCase
            string assetName = ObjectNames.NicifyVariableName(property.name).Replace(" ", "");

            // Determine the path to save the asset in a type-specific folder
            string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(ScriptableObject.CreateInstance(type)));
            string folderPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(scriptPath), $"{type.Name}Assets");
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(scriptPath), $"{type.Name}Assets");
            }
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{assetName}.asset");

            // Save the asset
            AssetDatabase.CreateAsset(newAsset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return newAsset;
        }
    }
}
