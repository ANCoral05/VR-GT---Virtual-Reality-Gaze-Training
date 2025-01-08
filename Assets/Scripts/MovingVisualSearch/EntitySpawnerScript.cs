using System.Collections.Generic;
using UnityEngine;
using VRK_BuildingBlocks;

public class EntitySpawnerScript : MonoBehaviour
{
    [SerializeField, Tooltip("The entitíes to spawn (can also include new EntitySpawners).")]
    private List<GameObject> entityPrefab;

    [SerializeField, Tooltip("List of intervals after which the entities will be spawned.")]
    private List<float> spawnIntervals;

    [SerializeField, Tooltip("The points or fields in which the objects are spawned.")]
    private List<Transform> spawnFields;

    [SerializeField, Tooltip("The maximum number of entities to spawn in one go.")]
    private List<int> numberOfSpawnedEntities;

    [SerializeField, Tooltip("The minimum distance that entities must have from each other on spawn.")]
    private float minDistanceBetweenEntities;

    [SerializeField, Tooltip("The number of loops until the spawner stops. Set to -1 for infinite repetitions.")]
    private int numberOfLoops = -1;

    [SerializeField, Tooltip("Toggle on to spawn entities in random order instead of looping through the list.")]
    private bool randomEntityOrder;

    [SerializeField, Tooltip("Toggle on to set intervals in random order instead of looping through the list.")]
    private bool randomIntervalOrder;

    [SerializeField, Tooltip("Toggle on to use spawn fields in random order instead of looping through the list.")]
    private bool randomSpawnFieldOrder;

    [SerializeField, Tooltip("Set the target pool containers to add the targets to after spawning.")]
    private List<PooledGameObjectVariable> targetPoolContainers;

    [SerializeField, Tooltip("Toggle on to disable the spawner after spawning.")]
    private bool disableAfterSpawning = true;


    // A function that generates a random point within a collider.
    public Vector3 GetPointInsideSpawnFieldCollider(Transform spawnField)
    {
        Collider spawnFieldCollider = spawnField.GetComponent<Collider>();

        if (spawnFieldCollider == null)
        {
            return Vector3.zero;
        }

        Bounds bounds = spawnFieldCollider.bounds;
        int maxAttempts = 100; // Prevent infinite loops
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Generate a random point within the collider's bounding box
            Vector3 randomPoint = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );

            // Check if the point is within the custom collider
            if (IsPointInsideCollider(randomPoint, spawnFieldCollider))
            {
                return randomPoint;
            }

            attempts++;
        }

        Debug.LogWarning("Could not find a point inside the custom collider.");
        return Vector3.zero;
    }

    private bool IsPointInsideCollider(Vector3 point, Collider spawnFieldCollider)
    {
        // Use Physics.ClosestPoint to determine if the point is inside
        Vector3 closestPoint = spawnFieldCollider.ClosestPoint(point);
        return Vector3.Distance(closestPoint, point) < Mathf.Epsilon;
    }
}
