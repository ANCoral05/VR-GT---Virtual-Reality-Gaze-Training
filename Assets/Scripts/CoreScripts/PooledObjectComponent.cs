using UnityEngine;
using UnityEngine.Events;

public class PooledObjectComponent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onSpawn = new UnityEvent();

    [SerializeField]
    private UnityEvent onDespawn = new UnityEvent();

    public void OnSpawn()
    {
        onSpawn.Invoke();
    }

    public void OnDespawn()
    {
        onDespawn.Invoke();
    }
}
