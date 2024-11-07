using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class WaitForDelayEntity : EntitySequence<WaitForDelayEntityData>
    {
        private float delayTimeRemaining;

        protected override UniTask Initialize(WaitForDelayEntityData currentData)
        {
            // Initialize the delay time from the current data
            delayTimeRemaining = currentData.delayTime;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // If there's no delay time, complete immediately
            if (delayTimeRemaining <= 0)
            {
                Complete();
            }
        }

        void Update()
        {
            if (delayTimeRemaining <= 0)
                return;

            // Decrease the remaining time
            delayTimeRemaining -= Time.deltaTime;

            // If the delay time has expired, complete the sequence
            if (delayTimeRemaining <= 0)
            {
                Complete();
            }
        }

        private async void Complete()
        {
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            // Clean up by destroying the GameObject when the sequence is done
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class WaitForDelayEntityData : SequenceData
    {
        public float delayTime = 1f; // Time to wait in seconds
    }
}
