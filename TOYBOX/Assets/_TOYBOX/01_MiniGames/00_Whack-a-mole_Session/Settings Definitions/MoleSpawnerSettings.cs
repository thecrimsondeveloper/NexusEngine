using System.Collections;
using System.Collections.Generic;
using ToyBox.WackAMole;
using UnityEngine;

namespace ToyBox
{
    [CreateAssetMenu(menuName = "TOYBOX/Settings/Wack-A-Mole/Mole Spawner Settings")]
    public class MoleSpawnerSettings : ScriptableObject
    {
        [SerializeField] Mole baseMolePrefab = null;
        [SerializeField] float maxSpawnInterval = 0.0f;
        [SerializeField] float minSpawnInterval = 1.0f;

        [SerializeField] float spawnRadius = 0.1f;
        [SerializeField] MoleWave[] moleWaves = null;
        [SerializeField] Bounds[] spawnBounds = null;

        public Mole BaseMolePrefab => baseMolePrefab;
        public float SpawnRadius => spawnRadius;
        public Bounds[] SpawnBounds => spawnBounds;

        public float GetRandomSpawnInterval()
        {
            return Random.Range(minSpawnInterval, maxSpawnInterval);
        }

        public MoleWave GetMoleWave(int waveIndex)
        {
            Debug.Log("GetMoleWave: " + waveIndex);
            if (waveIndex >= moleWaves.Length || waveIndex < 0)
            {
                Debug.LogError("MoleWave index out of range");
                return null;
            }
            MoleWave wave = moleWaves[waveIndex];

            Debug.Log("MoleWave: " + wave.MoleCount + " " + wave.SpawnInterval);
            return wave;
        }
    }
}
