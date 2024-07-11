using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    [Serializable]
    public class WeightedPrefab
    {
        public GameObject prefab;
        [Range(0, 100)] public int probability;
    }

    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] List<WeightedPrefab> weightedPrefabs = new List<WeightedPrefab>();
        [SerializeField] BoxCollider2D spawnArea;
        [SerializeField] float spawnRate = 1f;
        [SerializeField] float spawnRateIncrease = 0.1f;
        [SerializeField] float spawnRateIncreaseInterval = 5f;

        [SerializeField] float minSpawnInterval = 0.5f;

        public bool shouldSpawn = true;

        private void Start()
        {
            // Adjust the weighted probabilities to form a cumulative probability distribution
            AdjustProbabilities();
        }

        private void Update()
        {
            if (shouldSpawn == false)
            {
                return;
            }
            // Spawn obstacles at a regular interval
            if (Time.time % spawnRate < Time.deltaTime)
            {
                SpawnObstacles();
            }

            // Increase the spawn rate at regular intervals
            if (Time.time % spawnRateIncreaseInterval < Time.deltaTime)
            {
                if (spawnRate > minSpawnInterval)
                {
                    spawnRate -= spawnRateIncrease;
                }
            }
        }

        private void AdjustProbabilities()
        {
            int totalProbability = 0;

            // Calculate the total probability
            foreach (var weightedPrefab in weightedPrefabs)
            {
                totalProbability += weightedPrefab.probability;
            }

            // Normalize probabilities to form a cumulative probability distribution
            float cumulativeProbability = 0f;
            foreach (var weightedPrefab in weightedPrefabs)
            {
                cumulativeProbability += (float)weightedPrefab.probability / totalProbability * 100;
                weightedPrefab.probability = (int)cumulativeProbability;
            }
        }

        private void SpawnObstacles()
        {
            // Spawn obstacles based on the weighted probability distribution
            int randomValue = UnityEngine.Random.Range(0, 100);

            foreach (var weightedPrefab in weightedPrefabs)
            {
                if (randomValue < weightedPrefab.probability)
                {
                    GameObject spawnedObstacle = Instantiate(weightedPrefab.prefab, GetRandomSpawnPosition(), Quaternion.identity);
                    spawnedObstacle.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 5, ForceMode2D.Force);

                    break;
                }
            }
        }

        Vector3 GetRandomSpawnPosition()
        {
            Vector3 spawnPosition = new Vector3(
                UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                0
            );

            return spawnPosition;
        }

        public void StopSpawning()
        {
            shouldSpawn = false;
        }


    }
}
