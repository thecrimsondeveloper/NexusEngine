using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extras;
using Toolkit.Sessions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToyBox.WackAMole
{

    public class MoleSpawner : MonoBehaviour
    {
        static List<Mole> spawnedMoles = new List<Mole>();
        public bool isInitialized { get; set; } = false;
        [SerializeField] MoleSpawnerSettings moleSpawnerSettings = null;

        int nextWaveIndex = 0;
        MoleWave currentWave = null;
        bool spawningWave = false;


        [Button]
        async void SpawnNextWave()
        {
            if (spawningWave) return;
            await UniTask.RunOnThreadPool(async () =>
            {
                MoleWave nextWave = moleSpawnerSettings.GetMoleWave(nextWaveIndex);
                if (nextWave == null)
                {
                    return;
                }
                await SpawnWave(moleSpawnerSettings, nextWave);
            });
        }





        async UniTask SpawnWave(MoleSpawnerSettings spawnSettings, MoleWave moleWave)
        {
            if (spawningWave)
            {
                return;
            }

            Debug.Log("Spawning wave " + moleWave);

            currentWave = moleWave;
            spawningWave = true;
            for (int i = 0; i < moleWave.MoleCount; i++)
            {
                await UniTask.Delay((int)(moleWave.GetRandomSpawnInterval() * 1000));

                Vector3? spawnPose = await GetSpawnPoint(spawnSettings);
                if (spawnPose == null)
                {
                    Debug.Log("Could not find a spawn point.");
                    continue;
                }

                Pose pose = new Pose(spawnPose.Value + transform.position, Quaternion.identity);
                Mole mole = await SpawnMole(spawnSettings.BaseMolePrefab, pose, transform);
            }
            spawningWave = false;
            nextWaveIndex++;
        }

        static async UniTask<Mole> SpawnMole(Mole prefab, Pose spawnPose = default, Transform parent = null)
        {
            Mole mole = Instantiate(prefab, spawnPose.position, spawnPose.rotation, parent);
            await UniTask.NextFrame();
            return mole;
        }

        public static void AddMole(Mole mole)
        {
            if (spawnedMoles.Contains(mole))
            {
                return;
            }
            spawnedMoles.Add(mole);
        }

        public static void RemoveMole(Mole mole)
        {
            if (!spawnedMoles.Contains(mole))
            {
                return;
            }
            spawnedMoles.Remove(mole);
        }

        async static UniTask<Vector3?> GetSpawnPoint(MoleSpawnerSettings spawnSettings)
        {

            float moleRadius = spawnSettings.SpawnRadius;
            Vector3 spawnPoint = BoundsInterface.GetRandomPointInBounds(spawnSettings.SpawnBounds);

            bool IsMoleClose()
            {
                return spawnedMoles.Exists(mole => Vector3.Distance(mole.transform.position, spawnPoint) < moleRadius);
            }

            void CalculatePoint()
            {
                int tries = 0;

                // //loop until we find a point that is not close to another mole or reach 100 tries
                // while (IsMoleClose() && tries < 100)
                // {
                //     spawnPoint = BoundsInterface.GetRandomPointInBounds(spawnSettings.SpawnBounds);
                //     tries++;
                // }

                if (tries >= 100)
                {
                    return;
                }
            }

            await UniTask.RunOnThreadPool(CalculatePoint);

            return spawnPoint;
        }
    }
}