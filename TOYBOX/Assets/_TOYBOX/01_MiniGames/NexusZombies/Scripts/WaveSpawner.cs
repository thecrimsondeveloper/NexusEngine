using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Extras
{
    [System.Serializable]
    public class Wave : ScriptableObject
    {
        //create constructor
        public void Initialize(WaveData wave)
        {
            this.wave = wave;
        }
        public async UniTask Distribute()
        {
            for (int i = 0; i < wave.numberOfEnemies; i++)
            {
                if (wave.limitNumberOfEnemies)
                    await UniTask.WaitWhile(() => GetNumberOfWaveObjects() >= wave.maximumAllowedAtOnce);

                if (isCancelled) return;


                GameObject obj = GameObject.Instantiate(wave.wavePrefab, wave.spawnPointHandler.GetSpawnPoint(), Quaternion.identity, wave.parent);

                WaveObject waveObject = obj.AddComponent<WaveObject>();
                waveObject.Initialize(this);

                waveObject.WhenDestroyed.AddListener(() =>
                {
                    WaveObjectDestroyed(waveObject);
                    numberOfWaveObjects--;
                });

                this._OnSpawn?.Invoke(waveObject);

                this.AddWaveObject(waveObject);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
            this._OnWaveFinishSpawning?.Invoke();
        }

        WaveData wave;
        internal Guid id = Guid.NewGuid();
        public Action _OnWaveStartSpawning { get; private set; } = null;
        public Action _OnWaveFinishSpawning { get; private set; } = null;
        public Action _OnWaveEnd { get; private set; } = null;
        public Action<WaveObject> _OnSpawn { get; private set; } = null;
        public Action OnCanceled { get; private set; } = null;
        List<WaveObject> waveObjects = new List<WaveObject>();
        bool isCancelled = false;


        public Wave OnWaveStart(System.Action action)
        {
            _OnWaveStartSpawning += action;
            return this;
        }

        public Wave OnWaveSpawningFinished(System.Action action)
        {
            _OnWaveFinishSpawning += action;
            return this;
        }

        public Wave OnSpawn(System.Action<WaveObject> action)
        {
            _OnSpawn += action;
            return this;
        }

        public Wave OnWaveEnd(System.Action action)
        {
            _OnWaveEnd += action;
            return this;
        }

        public Wave AddWaveObject(WaveObject obj)
        {
            waveObjects.Add(obj);
            numberOfWaveObjects++;
            return this;
        }

        public void Cancel()
        {
            isCancelled = true;
            OnCanceled?.Invoke();
            ClearWaveObjects();
        }

        void ClearWaveObjects()
        {
            foreach (var obj in waveObjects)
            {
                if (obj != null)
                    GameObject.Destroy(obj);
            }
            waveObjects.Clear();
        }

        public void WaveObjectDestroyed(WaveObject obj)
        {
            if (waveObjects.Contains(obj)) waveObjects.Remove(obj);

            if (waveObjects.Count == 0)
            {
                _OnWaveEnd?.Invoke();
            }
        }

        int numberOfWaveObjects = 0;
        public int GetNumberOfWaveObjects()
        {

            return numberOfWaveObjects;
        }
    }

    [System.Serializable]
    public struct WaveData
    {
        public GameObject wavePrefab { get; set; } //wave prefab
        public int numberOfEnemies { get; set; } //number of enemies
        public int maximumAllowedAtOnce { get; set; }//maximum allowed at once
        public bool limitNumberOfEnemies { get; set; } //limit number of enemies
        public NumberRange timeBetweenEnemies { get; set; } //time between enemies
        public SpawnPointHandler spawnPointHandler { get; set; } //spawn point handle
        public Transform parent { get; set; } //parent
    }
}
