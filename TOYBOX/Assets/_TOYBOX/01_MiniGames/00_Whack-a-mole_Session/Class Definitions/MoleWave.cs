using Sirenix.OdinInspector;
using Toolkit.Extras;
using UnityEngine;

namespace ToyBox.WackAMole
{
    [System.Serializable]
    public class MoleWave
    {
        [SerializeField, HideLabel, BoxGroup("Mole Count")]
        int moleCount = 0;


        [SerializeField]
        [HideLabel]
        [BoxGroup("Spawn Interval")]
        NumberRange spawnInterval = new NumberRange(0.0f, 1.0f);


        public int MoleCount => moleCount;
        public float SpawnInterval => spawnInterval;
        public float GetRandomSpawnInterval()
        {
            return spawnInterval.Random();
        }
    }
}
