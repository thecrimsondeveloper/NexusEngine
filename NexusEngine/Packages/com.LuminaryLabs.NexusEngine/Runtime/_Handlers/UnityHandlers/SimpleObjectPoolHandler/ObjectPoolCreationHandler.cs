using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class ObjectPoolCreationHandler : EntitySequence<ObjectPoolCreationHandlerData>
    {
        private Dictionary<string, Queue<EntitySequence>> _pools;
        private Transform _poolParent;

        protected override async UniTask Initialize(ObjectPoolCreationHandlerData currentData)
        {
            _pools = new Dictionary<string, Queue<EntitySequence>>();
            _poolParent = new GameObject("PooledSequences").transform;
            _poolParent.gameObject.SetActive(false);

            foreach (var poolConfig in currentData.poolConfigs)
            {
                await CreatePool(poolConfig.prefabSequence, poolConfig.size);
            }
        }

        private async UniTask CreatePool(EntitySequence prefabSequence, int initialSize)
        {
            if (!_pools.ContainsKey(prefabSequence.name))
            {
                var sequenceQueue = new Queue<EntitySequence>();

                for (int i = 0; i < initialSize; i++)
                {
                    var sequenceInstance = await InstantiateSequenceWithRun(prefabSequence);
                    sequenceQueue.Enqueue(sequenceInstance);
                }

                _pools.Add(prefabSequence.name, sequenceQueue);
            }
        }

        private EntitySequence sequenceInstance;

        private async UniTask<EntitySequence> InstantiateSequenceWithRun(EntitySequence prefabSequence)
        {
            // Use Sequence.Run to initialize the sequence
            Sequence.Run(prefabSequence, new SequenceRunData { sequenceData = prefabSequence.currentData, onBegin = StoreSequenceInstance });
            sequenceInstance.name = prefabSequence.name;
            sequenceInstance.transform.SetParent(_poolParent);
            Sequence.Stop(sequenceInstance);  // Stop and pool
            return sequenceInstance;
        }

        private void StoreSequenceInstance(ISequence _sequence)
        {
            sequenceInstance = _sequence as EntitySequence;
        }

        public EntitySequence GetFromPool(EntitySequence prefabSequence)
        {
            if (_pools.TryGetValue(prefabSequence.name, out var sequenceQueue))
            {
                if (sequenceQueue.Count > 0)
                {
                    var pooledInstance = sequenceQueue.Dequeue();
                    Sequence.Run(pooledInstance);  // Reactivate the sequence
                    return pooledInstance;
                }
                else
                {
                    // Expand pool if needed
                    Debug.LogWarning($"Expanding pool for {prefabSequence.name}");
                    var newSequence = InstantiateSequenceWithRun(prefabSequence).GetAwaiter().GetResult();
                    Sequence.Run(newSequence);
                    return newSequence;
                }
            }
            else
            {
                Debug.LogWarning($"No pool found for sequence {prefabSequence.name}. Expanding on demand.");
                CreatePool(prefabSequence, 1).Forget();
                return GetFromPool(prefabSequence);
            }
        }

        public void ReturnToPool(EntitySequence sequenceInstance)
        {
            Sequence.Finish(sequenceInstance);
            Sequence.Stop(sequenceInstance);

            sequenceInstance.transform.SetParent(_poolParent);

            if (_pools.TryGetValue(sequenceInstance.name, out var sequenceQueue))
            {
                sequenceQueue.Enqueue(sequenceInstance);
            }
        }

        protected override UniTask Unload()
        {
            foreach (var pool in _pools.Values)
            {
                while (pool.Count > 0)
                {
                    Destroy(pool.Dequeue().gameObject);
                }
            }
            _pools.Clear();

            if (_poolParent != null)
            {
                Destroy(_poolParent.gameObject);
            }

            return UniTask.CompletedTask;
        }

        public Queue<EntitySequence> GetPool(string sequenceName)
        {
            if (_pools.TryGetValue(sequenceName, out var sequenceQueue))
            {
                return sequenceQueue;
            }
            else
            {
                return null;
            }
        }

        protected override void OnBegin()
        {
            
        }
    }

    [System.Serializable]
    public class ObjectPoolCreationHandlerData : SequenceData
    {
        public List<PoolConfig> poolConfigs;
    }

    [System.Serializable]
    public class PoolConfig
    {
        public EntitySequence prefabSequence;
        public int size;
        
        public PoolConfig(EntitySequence prefabSequence, int size)
        {
            this.prefabSequence = prefabSequence;
            this.size = size;
        }
    }
}
