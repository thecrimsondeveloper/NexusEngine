using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Extras;
using UnityEngine;

namespace ToyBox.Minigames.BeatEmUp
{
    [System.Serializable]
    public class PrefabWeightEntry
    {
        public Obstacle prefab;
        public float weight;
    }

    public class ObstacleSpawner : MonoBehaviour
    {
        // [SerializeField] private Obstacle[] obstaclePrefabs;
        [ShowInInspector] public Dictionary<Obstacle, float> prefabWeights = new Dictionary<Obstacle, float>();
        [SerializeField] public PrefabWeightEntry[] prefabWeightEntries;
        [SerializeField] private float totalWeight;
        [SerializeField] private float spawnRate = 1f;

        private void Awake()
        {
            ConvertArrayToDictionary();
            totalWeight = 0f;
            //initialize the dictionary with the lists in the WeightedPrefabData

            foreach (var pair in prefabWeights)
            {
                totalWeight += pair.Value;
            }
        }

        // Convert 2D array to dictionary
        private void ConvertArrayToDictionary()
        {
            prefabWeights.Clear();

            foreach (var entry in prefabWeightEntries)
            {
                if (entry.prefab != null && !prefabWeights.ContainsKey(entry.prefab))
                {
                    prefabWeights.Add(entry.prefab, entry.weight);
                }
            }
        }

        private float spawnTimer = 0f;

        private void Update()
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnRate)
            {
                SpawnObstacle();
                spawnTimer = 0f;
            }
        }

        [Button]
        private void SpawnObstacle()
        {
            NumberRange range = new NumberRange(0, totalWeight);
            float random = range.Random();



            Obstacle chosenObstacle = null;
            float accumulatedWeight = 0f;

            foreach (var pair in prefabWeights)
            {
                accumulatedWeight += pair.Value;
                if (accumulatedWeight >= random)
                {
                    chosenObstacle = pair.Key;
                    break;
                }
            }

            if (chosenObstacle != null)
            {
                Obstacle obs = Instantiate(chosenObstacle, transform.position, Quaternion.identity, transform);
                obs.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
