using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Extras;
using Extras;
using Sirenix.OdinInspector;

namespace ToyBox.Minigames.CyberSlingers
{
    public class DroneSpawner : MonoBehaviour
    {
        [Title("Settings"), BoxGroup("Drone Spawner")]
        [SerializeField, BoxGroup("Drone Spawner"), Range(0, 1)] float currentRoundTime = 0;
        [Title("References"), BoxGroup("Drone Spawner")]
        [SerializeField, BoxGroup("Drone Spawner")] Transform droneParent;
        [SerializeField, BoxGroup("Drone Spawner")] Drone dronePrefab;
        [SerializeField, BoxGroup("Drone Spawner")] DroneSpawnerSettings spawnerSettings;

        [Title("Spawn Bounds"), BoxGroup("Drone Spawner")]
        [SerializeField, HideLabel, BoxGroup("Drone Spawner")] Bounds spawnBounds;

        [Title("Move Bounds"), BoxGroup("Drone Spawner")]
        [SerializeField, HideLabel, BoxGroup("Drone Spawner")] Bounds moveBounds;

        [Title("Debug Values"), BoxGroup("Drone Spawner")]

        [SerializeField] List<Drone> drones = new List<Drone>();





        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnBounds.center + droneParent.transform.position, spawnBounds.size);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(moveBounds.center + droneParent.transform.position, moveBounds.size);
        }


        float spawnTimer = 0;

        private void Update()
        {
            if (drones.Count >= spawnerSettings.GetMaxDroneCount(currentRoundTime))
            {
                return;
            }

            if (spawnTimer < 0)
            {
                spawnTimer = 1 / spawnerSettings.GetSpawnRate(currentRoundTime);
                SpawnNewDrone();
            }

            spawnTimer -= Time.deltaTime;
        }
        Drone newDrone = null;
        Vector3[] targetPoints = null;

        [Title("Debug Buttons"), BoxGroup("Drone Spawner")]
        [Button("Spawn New Drone"), BoxGroup("Drone Spawner")]
        public void SpawnNewDrone()
        {
            Vector3 spawnpoint = BoundsInterface.GetRandomPointInBounds(spawnBounds);

            //convert the spawn point to world space
            spawnpoint = transform.TransformPoint(spawnpoint);

            newDrone = Instantiate(dronePrefab, spawnpoint, Quaternion.identity, droneParent);
            // targetPoints = await BoundsInterface.GetRandomPointsInBounds_Async(spawnBounds, 2, 0.1f);

            targetPoints = new Vector3[2];
            targetPoints[0] = BoundsInterface.GetRandomPointInBounds(moveBounds);
            targetPoints[1] = BoundsInterface.GetRandomPointInBounds(moveBounds);

            //log the target points
            for (int i = 0; i < targetPoints.Length; i++)
            {
                Debug.Log("Target Point " + i + ": " + targetPoints[i]);
            }
            newDrone.Inititalize(this, targetPoints);

            drones.Add(newDrone);
        }
    }
}
