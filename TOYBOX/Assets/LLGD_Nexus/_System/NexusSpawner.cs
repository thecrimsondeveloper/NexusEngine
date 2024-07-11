using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusSpawner : NexusEntity
    {
        public NexusEntityDefinition entityDefinition;
        public NexusFloat spawnRate;
        public NexusInt maxEntities;
        public NexusEventReceiver spawnEntity = new NexusEventReceiver();
        public NexusList<NexusEntity> spawnedEntities = new NexusList<NexusEntity>();
        public NexusInt spawnRequestQueueSize;

        private float lastTimeSpawned = 0;

        private void Update()
        {
            TrySpawnQueuedEntity();

            if (spawnRate.value > 0 && Time.time - lastTimeSpawned > 1 / spawnRate.value)
            {
                lastTimeSpawned = Time.time;
                TrySpawnEntity();
            }
        }

        protected override void OnInitializeEntity()
        {

            if (entityDefinition == null)
            {
                Debug.LogError("EntityDefinition is null");
                return;
            }

            if (spawnRate == null)
            {
                spawnRate = ScriptableObject.CreateInstance<NexusFloat>();
                spawnRate.Set(1);
            }




            spawnEntity.InitializeObject();
            spawnRate.InitializeObject();
            spawnedEntities.InitializeObject();
            spawnRequestQueueSize.InitializeObject();
            maxEntities.InitializeObject();

            if (spawnEntity != null)
                spawnEntity.AddListener(TrySpawnEntity);
            else
                Debug.LogError("SpawnEntity is null");
        }

        NexusEntity SpawnNewEntity()
        {
            NexusEntity entity = Instantiate(entityDefinition.entityPrefab);
            entity.transform.position = transform.position;
            entity.InitializeEntity();
            spawnedEntities.Add(entity);
            return entity;
        }

        [Button]
        void TrySpawnEntity()
        {
            if (spawnedEntities.value.Count < maxEntities.value)
            {
                SpawnNewEntity();
            }
            else
            {
                //increment the queue size
                spawnRequestQueueSize.Increment();
            }
        }

        void TrySpawnQueuedEntity()
        {
            if (spawnRequestQueueSize.value == 0)
                return;

            if (spawnedEntities.value.Count < maxEntities.value)
            {
                SpawnNewEntity();
                spawnRequestQueueSize.Decrement();
            }
        }

        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }
    }
}