using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class EntitySpawnerScript : MonoBehaviour
    {
        [SerializeField, Tooltip("The entit�es to spawn (can also include new EntitySpawners).")]
        private List<GameObject> gameObjectList;

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

        [SerializeField, Tooltip("Toggle on to use number of spawned entities in random order instead of looping through the list.")]
        private bool randomNumberOfSpawnedEntitiesOrder;

        [SerializeField, Tooltip("Set the target pool containers to add the targets to after spawning.")]
        private List<PooledGameObjectVariable> targetPoolContainers;

        [SerializeField, Tooltip("Toggle on to disable the spawner after spawning.")]
        private bool disableAfterSpawning = true;

        [SerializeField]
        private FloatVariable difficultyMultiplier;

        private int currentEntityIndex = 0;

        private int currentIntervalIndex = 0;

        private int currentSpawnFieldIndex = 0;

        private int currentNumberOfSpawnedEntitiesIndex = 0;

        private float timeSinceLastSpawn = 0;

        private int currentLoop = 0;


        private void SpawnObject()
        {
            GameObject obj = null;

            if (gameObjectList[currentEntityIndex].GetComponent<PooledGameObjectComponent>() != null)
            {
                obj = gameObjectList[currentEntityIndex].GetComponent<PooledGameObjectComponent>().GetInstance();
            }
            else
            {
                obj = Instantiate(gameObjectList[currentEntityIndex]);
            }
            obj.transform.position = GetPointInsideSpawnFieldCollider(spawnFields[currentSpawnFieldIndex]);
            SetIndices();
            timeSinceLastSpawn = 0f;
            float newSpeed = 0.75f * difficultyMultiplier.Value;

            obj.GetComponent<EntityMovementScript>().targetSpeed = 0.5f * difficultyMultiplier.Value;
            if(obj.GetComponent<TargetSelectionScript>() != null)
                obj.GetComponent<TargetSelectionScript>().chargeInterval = 2f / difficultyMultiplier.Value;
        }

        private void SetIndices()
        {
            if (randomEntityOrder)
            {
                currentEntityIndex = Random.Range(0, gameObjectList.Count);
            }
            else
            {
                currentEntityIndex = (currentEntityIndex + 1) % gameObjectList.Count;
            }

            if (randomIntervalOrder)
            {
                currentIntervalIndex = Random.Range(0, spawnIntervals.Count);
            }
            else
            {
                currentIntervalIndex = (currentIntervalIndex + 1) % spawnIntervals.Count;
            }

            if (randomSpawnFieldOrder)
            {
                currentSpawnFieldIndex = Random.Range(0, spawnFields.Count);
            }
            else
            {
                currentSpawnFieldIndex = (currentSpawnFieldIndex + 1) % spawnFields.Count;
            }

            if(numberOfSpawnedEntities.Count == 0)
            {
                return;
            }
            if (randomNumberOfSpawnedEntitiesOrder)
            {
                currentNumberOfSpawnedEntitiesIndex = Random.Range(0, numberOfSpawnedEntities.Count);
            }
            else
            {
                currentNumberOfSpawnedEntitiesIndex = (currentNumberOfSpawnedEntitiesIndex + 1) % numberOfSpawnedEntities.Count;
            }
        }

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

        private void Update()
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn > spawnIntervals[currentIntervalIndex]/difficultyMultiplier.Value && (currentLoop < numberOfLoops || numberOfLoops < 0))
            {
                for (int i = 0; i < numberOfSpawnedEntities[currentNumberOfSpawnedEntitiesIndex]; i++)
                {
                    SpawnObject();
                }
                currentLoop++;
            }
        }
    }
}
