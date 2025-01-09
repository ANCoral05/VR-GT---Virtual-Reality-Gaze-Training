using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Pooled Game Object")]
    public class PooledGameObjectVariable : ScriptableObject
    {
        public GameObject gameObjectPrefab;

        private List<GameObject> pooledObjects = new List<GameObject>();

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

        public GameObject InstantiateOrRecycle(Transform parent, Transform spawnTransform)
        {
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
                obj.transform.position = spawnTransform.position;
                obj.transform.rotation = spawnTransform.rotation;
                obj.transform.localScale = spawnTransform.localScale;
                obj.transform.SetParent(parent);
                pooledObjects.Add(obj);
            }
            else
            {
                obj.SetActive(true);
                obj.transform.position = spawnTransform.position;
                obj.transform.rotation = spawnTransform.rotation;
                obj.transform.localScale = spawnTransform.localScale;
                obj.transform.SetParent(parent);
            }

            return obj;
        }

        public void ClearList()
        {
            pooledObjects = new List<GameObject>();
        }
    }
}
