using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using GazeQuestUtils;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Pooled Game Object")]
    public class PooledGameObjectVariable : ScriptableObject
    {
        [SerializeField]
        private GameObject gameObjectPrefab;

        private GameObject prevGameObjectPrefab;

        private Dictionary<string, object> initialValues = new Dictionary<string, object>();

        private List<GameObject> pooledObjects = new List<GameObject>();

        [SerializeField, Tooltip("Add all components from the gameObjectPrefab that should be reset upon recycling.")]
        private List<Component> componentsToReset = new List<Component>();

        [SerializeField, Tooltip("If true, all fields will be reset upon recycling. If false, only fields with the NonSerialized attribute will be reset.")]
        private bool resetNonSerializedFields = true;

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            ClearList();
        }

        public GameObject InstantiateOrRecycle(Transform spawnTransform = null, Transform parent = null)
        {
            if (initialValues == null)
            {
                GameObject initialGameObject = Instantiate(gameObjectPrefab);
                initialGameObject.transform.position = spawnTransform.position;
                initialGameObject.transform.rotation = spawnTransform.rotation;
                initialGameObject.transform.localScale = spawnTransform.localScale;
                initialGameObject.transform.parent = parent;

                CreateInitialValueDict(initialGameObject);

                Destroy(initialGameObject);
            }

            GameObject obj = null;
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    obj = pooledObjects[i];
                    break;
                }
            }

            if (obj == null)
            {
                obj = Instantiate(gameObjectPrefab);

                Transform objTransform = null;

                if (spawnTransform != null)
                {
                    objTransform = spawnTransform;
                }
                else
                {
                    objTransform = gameObjectPrefab.transform;
                }

                obj.transform.position = objTransform.position;
                obj.transform.rotation = objTransform.rotation;
                obj.transform.localScale = objTransform.localScale;

                if (parent != null)
                {
                    obj.transform.SetParent(parent);
                }

                pooledObjects.Add(obj);
            }
            else
            {
                float startTime = Time.realtimeSinceStartup;

                RetrieveStoredComponentStates(obj);

                obj.SetActive(true);
            }

            return obj;
        }

        private void CreateInitialValueDict(GameObject obj)
        {
            initialValues = new Dictionary<string, object>();

            int componentIndex = 0;

            foreach (Component component in componentsToReset)
            {
                StoreInitialValuesOfComponent(component, componentIndex);

                componentIndex++;
            }
        }

        private void StoreInitialValuesOfComponent(Component component, int componentIndex)
        {
            if (component == null)
            {
                return;
            }

            var type = component.GetType();

            Debug.Log("Storing initial values for " + component.gameObject.name + "_" + type.Name);

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Debug.Log("Fields: " + fields.Length);
            Debug.Log("Properties: " + properties.Length);

            // add fields to dictionary
            string gameObjectName = component.gameObject.name;
            string componentName = type.Name;

            foreach (var field in fields)
            {
                string fieldKey = $"{gameObjectName}_{componentName}_{componentIndex}_{field.Name}";
                // Debug.Log(componentName + "_" + field.Name + ": " + field.GetValue(component).ToString());

                // Detect if the field is a reference type (excluding strings)
                if (field.FieldType.IsClass && field.FieldType != typeof(string))
                {
                    Debug.LogWarning($"Warning: Field '{field.Name}' in component '{component.GetType().Name}' is a reference type. Changes to the referenced object won't be automatically reset upon recycling.", this);
                }

                if (!initialValues.ContainsKey(fieldKey))
                {
                    initialValues[fieldKey] = field.GetValue(component);
                }
            }

            foreach (var property in properties)
            {
                string propertyKey = $"{gameObjectName}_{componentName}_{componentIndex}_{property.Name}";
                // Detect if the property is a reference type (excluding strings)
                if (property.PropertyType.IsClass)
                {
                    Debug.LogWarning($"Warning: Property '{property.Name}' in component '{component.GetType().Name}' is a reference type. Changes to the referenced object won't be automatically reset upon recycling.", this);
                }

                if (!initialValues.ContainsKey(propertyKey))
                {
                    initialValues[propertyKey] = property.GetValue(component);
                }
            }
        }

        public void ClearList()
        {
            initialValues = null;

            pooledObjects = new List<GameObject>();
        }

        private void RetrieveStoredComponentStates(GameObject target)
        {
            int componentIndex = 0;

            foreach (var component in componentsToReset)
            {
                RetrieveInitialValuesOfComponent(component, componentIndex);
                
                componentIndex++;
            }
        }

        public void RetrieveInitialValuesOfComponent(Component component, int componentIndex)
        {
            if (component == null)
            {
                return;
            }

            var type = component.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            string gameObjectName = component.gameObject.name;
            string componentName = type.Name;

            foreach (var field in fields)
            {
                string fieldKey = $"{gameObjectName}_{componentName}_{componentIndex}_{field.Name}";

                if (!initialValues.ContainsKey(fieldKey))
                {
                    Debug.LogError("Initial values not found for " + fieldKey);
                }
                else
                {
                    field.SetValue(component, initialValues[fieldKey]);
                }
            }

            foreach (var property in properties)
            {
                string propertyKey = $"{gameObjectName}_{componentName}_{componentIndex}_{property.Name}";
                if (!initialValues.ContainsKey(propertyKey))
                {
                    Debug.LogError("Initial values not found for " + propertyKey);
                }
                else
                {
                    property.SetValue(component, initialValues[propertyKey]);
                }
            }
        }

        private void GenerateComponentList(GameObject obj)
        {
            if (obj == null)
            {
                componentsToReset = new List<Component>();
                return;
            }

            List<GameObject> children = GazeQuestUtilityFunctions.GetDescendants(obj);

            componentsToReset = new List<Component>(obj.GetComponents<Component>());

            foreach (GameObject child in children)
            {
                componentsToReset.AddRange(child.GetComponents<Component>());
            }
        }

        private void OnValidate()
        {
            if (gameObjectPrefab != prevGameObjectPrefab)
            {
                GenerateComponentList(gameObjectPrefab);

                prevGameObjectPrefab = gameObjectPrefab;
            }
        }
    }
}
