using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


namespace Toolkit.EditorTools
{
    public class RadialPlacement : MonoBehaviour
    {
        public GameObject objectToSpawn; // Prefab of the object to spawn
        public int numberOfObjects = 10; // Number of objects to spawn
        public float minDistance = 5f; // Minimum distance to spawn objects
        public float maxDistance = 10f; // Maximum distance to spawn objects
        public float avoidDistance = 1f; // Minimum distance between objects
        public int maxTries = 100; // Maximum number of tries to calculate spawn position


        // Start is called before the first frame update
        void Start()
        {
            SpawnObjects();
        }

        [Button]
        async void SpawnObjects()
        {
            int childCount = transform.childCount;
            int tries = 0;
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                tries++;
                if (tries > childCount * 2)
                {
                    Debug.LogError("Too many tries to destroy children");
                    break;
                }
                await UniTask.Delay(1);
            }

            for (int i = 0; i < numberOfObjects; i++)
            {
                float distance = maxDistance - minDistance;
                float spawnDistance = (i * distance / numberOfObjects) + minDistance;

                Vector3 spawnPosition = CalculateSpawnPoint(spawnDistance); // Calculate spawn position

                GameObject spawnedObject = PrefabUtility.InstantiatePrefab(objectToSpawn, transform) as GameObject;
                spawnedObject.transform.position = spawnPosition;

                Vector3 direction = (spawnPosition - transform.position).normalized; // Calculate direction to face the center

                spawnedObject.transform.forward = -direction; // Rotate object to face the center
            }
        }

        Vector3 CalculateSpawnPoint(float spawnDistance)
        {
            Vector3 direction = Random.onUnitSphere; // Get a random direction
            Vector3 spawnPosition = transform.position + direction * spawnDistance; // Calculate spawn position

            int tries = 0;
            while (CheckIfChildrenNearPosition(spawnPosition))
            {
                direction = Random.onUnitSphere; // Get a random direction
                spawnPosition = transform.position + direction * spawnDistance; // Calculate spawn position

                tries++;
                if (tries >= maxTries) // Maximum number of tries
                {
                    Debug.LogError("Exceeded maximum number of spawn position calculation tries");
                    break;
                }
            }

            return spawnPosition + transform.position;
        }


        bool CheckIfChildrenNearPosition(Vector3 position)
        {
            foreach (Transform child in transform)
            {
                if (Vector3.Distance(child.position, position) <= avoidDistance)
                {
                    // Found a child within the specified distance threshold
                    return true;
                }
            }

            // No children found within the specified distance threshold
            return false;
        }
    }
}
#endif