using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class WaitForTrigger : EntitySequence<WaitForTriggerData>
    {
        private Collider targetCollider;

        protected override UniTask Initialize(WaitForTriggerData currentData)
        {
            // Store the target collider to check against during the trigger event
            targetCollider = currentData.targetCollider;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // The sequence begins, waiting for the trigger to match the target collider
        }

        private async void OnTriggerEnter(Collider other)
        {
            // Check if the entered collider matches the target
            if (other == targetCollider)
            {
                // The trigger matched, so we can mark this sequence as finished
                await Sequence.Finish(this);
                await Sequence.Stop(this);
            }
        }

        protected override UniTask Unload()
        {
            // Clean up if necessary
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class WaitForTriggerData : SequenceData
    {
        public Collider targetCollider; // The collider to check for
    }
}
