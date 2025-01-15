using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.InputSystem.LowLevel;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Pooled Game Object")]
    public class PooledGameObjectVariable : ScriptableObject
    {
        [SerializeField]
        private GameObject gameObjectPrefab;

        private GameObject initialGameObject;

        private List<GameObject> pooledObjects = new List<GameObject>();

        [SerializeField, Tooltip("Add all components from the gameObjectPrefab that should be reset upon recycling.")]
        private List<Component> componentsToReset = new List<Component>();

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

        public GameObject InstantiateOrRecycle(Transform spawnTransform, Transform parent = null)
        {

            if (initialGameObject == null)
            {
                initialGameObject = Instantiate(gameObjectPrefab);
                initialGameObject.SetActive(false);
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
                pooledObjects.Add(obj);

                if (spawnTransform != null)
                {
                    obj.transform.position = spawnTransform.position;
                    obj.transform.rotation = spawnTransform.rotation;
                    obj.transform.localScale = spawnTransform.localScale;
                }

                if (parent != null)
                {
                    obj.transform.SetParent(parent);
                }

            }
            else
            {
                obj.SetActive(true);
                obj.transform.position = spawnTransform.position;
                obj.transform.rotation = spawnTransform.rotation;
                obj.transform.localScale = spawnTransform.localScale;
                obj.transform.SetParent(parent);
                if (obj.GetComponent<PooledObjectComponent>() != null)
                {
                    obj.GetComponent<PooledObjectComponent>().OnSpawn();
                }
            }

            return obj;
        }

        public void ClearList()
        {
            initialGameObject = null;

            pooledObjects = new List<GameObject>();
        }

        public void ResetState(GameObject gameObject, bool resetNonSerializedFields = true)
        {
            // write the code to reset the state of the object using reflection
            // if resetNonSerializedFields is true, reset all fields
            // if resetNonSerializedFields is false, reset only fields with the NonSerialized attribute
            CopyComponentsToTarget(gameObject, initialGameObject, resetNonSerializedFields);
        }

        public void CopyComponentFieldsToTarget(Component targetComponent, Component sourceComponent, bool resetNonSerializedFields = true)
        {
            if (targetComponent.GetType() != sourceComponent.GetType())
            {
                Debug.LogError($"Component types do not match: {targetComponent.GetType()} vs {sourceComponent.GetType()}");
                return;
            }

            var type = sourceComponent.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (resetNonSerializedFields || field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                {
                    try
                    {
                        var value = field.GetValue(sourceComponent);
                        field.SetValue(targetComponent, value);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Failed to copy field '{field.Name}' from {sourceComponent} to {targetComponent}: {ex.Message}");
                    }
                }
            }
        }

        private void CopyComponentsToTarget(GameObject target, GameObject source, bool resetNonSerializedFields = true)
        {
            foreach (var component in componentsToReset)
            {
                var targetComponent = target.GetComponent(component.GetType());
                var sourceComponent = source.GetComponent(component.GetType());
                if (targetComponent != null && sourceComponent != null)
                {
                    CopyComponentFieldsToTarget(targetComponent, sourceComponent, resetNonSerializedFields);
                }
                else
                {
                    Debug.LogError($"Component not found in target or source: {component.GetType()}");
                }
            }
        }

        public void matchByName()
        {
            // a function that will compare two lists with strings and write ("match found: name) for each found pair and (no match found: name in list) for each name that has no match
            List<string> list1 = new List<string> { "name1", "name2", "name3", "name4" };
            List<string> list2 = new List<string> { "name1", "name3", "name4", "name5" };

            foreach (var name in list1)
            {
                if (list2.Contains(name))
                {
                    Debug.Log($"match found: {name}");
                }
                else
                {
                    Debug.Log($"no match found for: {name} in list1");
                }
            }

            foreach (var name in list2)
            {
                if (!list1.Contains(name))
                {
                    Debug.Log($"no match found for: {name} in list2");
                }
            }
        }
    }
}
