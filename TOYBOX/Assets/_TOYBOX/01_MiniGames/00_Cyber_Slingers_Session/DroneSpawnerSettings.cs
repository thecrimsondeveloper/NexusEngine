using Toolkit.Extras;
using UnityEngine;

namespace ToyBox
{
    [CreateAssetMenu(menuName = "TOYBOX/MiniGames/Cyber Slingers/Drone Spawner Settings")]
    public class DroneSpawnerSettings : ScriptableObject
    {
        [SerializeField] NumberRange spawnRate = new NumberRange(0.1f, 0.5f);
        [SerializeField] AnimationCurve spawnRateCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] NumberRange maxDroneCount = new NumberRange(5, 10);
        [SerializeField] AnimationCurve droneCountCurve = AnimationCurve.Linear(0, 0, 1, 1);


        float currentSpawnRate = 0.5f;

        int currentMaxDroneCount = 5;

        float lastTimeForSpawn = 0;
        public float GetSpawnRate(float time)
        {
            if (time == lastTimeForSpawn)
                return currentSpawnRate;


            time = Mathf.Clamp01(time);
            currentSpawnRate = spawnRate.Lerp(spawnRateCurve.Evaluate(time));

            lastTimeForSpawn = time;
            return currentSpawnRate;
        }


        float lastTimeForCount = 0;
        public int GetMaxDroneCount(float time)
        {
            if (time == lastTimeForCount)
                return currentMaxDroneCount;

            time = Mathf.Clamp01(time);
            currentMaxDroneCount = Mathf.RoundToInt(maxDroneCount.Lerp(droneCountCurve.Evaluate(time)));
            lastTimeForCount = time;
            return currentMaxDroneCount;
        }


    }
}
