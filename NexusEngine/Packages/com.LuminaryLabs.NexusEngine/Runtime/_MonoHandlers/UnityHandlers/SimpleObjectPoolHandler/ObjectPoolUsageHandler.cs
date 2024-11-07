using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class ObjectPoolUsageHandler : EntitySequence<ObjectPoolUsageHandlerData>
    {
        public enum PoolAction
        {
            Spawn,
            Return,
            Clear
        }

        private ObjectPoolCreationHandler _poolCreationHandler;
        private PoolAction _action;
        private MonoSequence _prefabSequence;

        protected override UniTask Initialize(ObjectPoolUsageHandlerData currentData)
        {
            _poolCreationHandler = superSequence.GetTransform().GetComponentInChildren<ObjectPoolCreationHandler>();
            _action = currentData.action;
            _prefabSequence = currentData.prefabSequence;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            if (_poolCreationHandler == null || _prefabSequence == null)
            {
                Debug.LogWarning("Pool creation handler or prefab sequence is missing.");
                Sequence.Finish(this);
                return;
            }

            switch (_action)
            {
                case PoolAction.Spawn:
                    SpawnFromPool(_prefabSequence);
                    break;
                case PoolAction.Return:
                    ReturnToPool(_prefabSequence);
                    break;
                case PoolAction.Clear:
                    ClearPool(_prefabSequence);
                    break;
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private void SpawnFromPool(MonoSequence prefabSequence)
        {
            var pooledInstance = _poolCreationHandler.GetFromPool(prefabSequence);
            if (pooledInstance != null)
            {
                Sequence.Run(pooledInstance);  // Reactivate the sequence
            }
            else
            {
                Debug.LogWarning($"No available instances in the pool for {prefabSequence.name}");
            }
        }

        private void ReturnToPool(MonoSequence sequenceInstance)
        {
            Sequence.Finish(sequenceInstance);
            _poolCreationHandler.ReturnToPool(sequenceInstance);
        }

        private void ClearPool(MonoSequence prefabSequence)
        {
            var pool = _poolCreationHandler.GetPool(prefabSequence.name);
            if (pool != null)
            {
                while (pool.Count > 0)
                {
                    Destroy(pool.Dequeue().gameObject);
                }
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class ObjectPoolUsageHandlerData : SequenceData
    {
        public ObjectPoolUsageHandler.PoolAction action;
        public MonoSequence prefabSequence;
    }
}
