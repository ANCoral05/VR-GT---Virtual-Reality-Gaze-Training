using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRK_BuildingBlocks;

public class PooledGameObjectComponent : MonoBehaviour
{
    public UnityEvent OnSpawn;

    public UnityEvent OnDespawn;

    [SerializeField] private bool showInDebugLog = false;

    [SerializeField] private string poolID;

    private GameObjectPool pool;

    private void InstantiatePoolObject()
    {
        if (pool == null)
        {
            pool = GameObject.Find($"{poolID}_Pool")?.GetComponent<GameObjectPool>();

            if (pool != null)
            {
                return;
            }

            GameObject poolObject = new GameObject();
            poolObject.name = $"{poolID}_Pool";
            pool = poolObject.AddComponent<GameObjectPool>();
        }
    }

    public GameObject GetInstance(Transform originTransform = null, Transform parent = null)
    {
        InstantiatePoolObject();

        GameObject instanciatedObject = null;

        if (pool.pooledObjects.Count > 0)
        {
            if (showInDebugLog)
                Debug.Log("Recycled pooled game object.");

            instanciatedObject = pool.pooledObjects.Dequeue();
            instanciatedObject.SetActive(true);
        }
        
        if (instanciatedObject == null)
        {
            if (showInDebugLog)
                Debug.Log("No pooled game objects available. Instantiating a new one.");
            instanciatedObject = Instantiate(this.gameObject);
            instanciatedObject.SetActive(true);
        }

        if (originTransform != null)
        {
            instanciatedObject.transform.position = originTransform.position;
            instanciatedObject.transform.rotation = originTransform.rotation;
        }
        if (parent != null)
        {
            instanciatedObject.transform.SetParent(parent);
        }

        return instanciatedObject;
    }

    private void OnValidate()
    {
        if(poolID == null)
        {
            poolID = this.GetInstanceID().ToString();
        }
    }

    private void OnEnable()
    {
        OnSpawn.Invoke();
    }

    private void OnDisable()
    {
        InstantiatePoolObject();

        OnDespawn.Invoke();

        pool.pooledObjects.Enqueue(this.gameObject);
    }
}
