using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class WaitForCollision : EntitySequence<WaitForCollisionData>
    {
        private Collider targetCollider;

        protected override UniTask Initialize(WaitForCollisionData currentData)
        {
            // Store the target collider to check against during the collision event
            targetCollider = currentData.targetCollider;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // The sequence begins, waiting for the collision to match the target collider
        }

        private async void OnCollisionEnter(Collision collision)
        {
            // Check if the collided object has the target collider
            if (collision.collider == targetCollider)
            {
                // The collision matched, so we can mark this sequence as finished
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
    public class WaitForCollisionData : SequenceData
    {
        public Collider targetCollider; // The collider to check for
    }
}
