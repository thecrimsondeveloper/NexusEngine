using System.Collections;
using System.Collections.Generic;
using TMPro;
using Toolkit.Entity;
using Toolkit.Extras;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class ZombiesWaveHandler : MonoBehaviour
    {


        int currentWave = 1;
        [SerializeField] GameObject zombiePrefab;
        [SerializeField] SpawnPointHandler spawnPointHandler;
        [SerializeField] NexusScoreHandler scoreHandler;
        [SerializeField] TMP_Text waveNumberText;

        // Start is called before the first frame update
        void Start()
        {
            StartWave();
        }

        int CalculateNumberOfZombies()
        {
            return currentWave * 10;
        }


        public void CancelActiveWave()
        {
            if (activeWave != null) activeWave.Cancel();
        }

        [SerializeField] Wave activeWave;

        void StartWave()
        {

            WaveData waveData = new WaveData()
            {
                wavePrefab = zombiePrefab,
                numberOfEnemies = CalculateNumberOfZombies(),
                timeBetweenEnemies = new NumberRange(0.1f, 3),
                maximumAllowedAtOnce = 1,
                limitNumberOfEnemies = true,
                spawnPointHandler = this.spawnPointHandler,
                parent = this.transform
            };
            if (waveNumberText) waveNumberText.text = "Wave: " + currentWave;

            //create an instance of the scriptable object

            activeWave = ScriptableObject.CreateInstance<Wave>();
            activeWave.Initialize(waveData);
            activeWave.OnWaveStart(OnWaveStart);
            activeWave.OnWaveSpawningFinished(OnWaveFinishedSpawning);
            activeWave.OnSpawn(OnZombieSpawned);
            activeWave.OnWaveEnd(OnWaveEnd);

            activeWave.Distribute();

        }

        void NextWave()
        {
            currentWave++;
            StartWave();
        }

        void OnWaveStart()
        {
            Debug.Log("Wave Started");
        }

        void OnWaveFinishedSpawning()
        {
            //increment the wave
            // NextWave();
        }

        void OnWaveEnd()
        {
            Debug.Log("Wave Ended");
            //increment the wave
            NextWave();
        }

        void OnZombieSpawned(WaveObject zombie)
        {
            if (zombie.TryGetComponent(out IScoreable scoreable))
            {
                Debug.Log("Zombie Scoreable");
                scoreHandler.RegisterWithScorable(scoreable);
            }
            Debug.Log("Zombie Spawned");
        }


    }
}
