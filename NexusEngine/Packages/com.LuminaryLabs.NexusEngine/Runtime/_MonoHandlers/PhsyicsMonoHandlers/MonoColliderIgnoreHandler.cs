using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class MonoColliderIgnoreHandler : EntitySequence<MonoColliderIgnoreHandlerData>
    {
        private bool resetIgnoringWhenUnload;
        private List<Collider> collidersToIgnore;
        protected override UniTask Initialize(MonoColliderIgnoreHandlerData currentData)
        {
            collidersToIgnore = currentData.collidersToIgnore;
            resetIgnoringWhenUnload = currentData.resetIgnoringWhenUnload;
            // Ignore collisions between specified colliders
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            IgnoreCollisions(collidersToIgnore);
            // No additional logic required in BeginWith phase for this handler
        }

        protected override UniTask Unload()
        {
            // Re-enable collisions when the sequence is unloaded
            if(resetIgnoringWhenUnload)
            {
                ResetCollisions(collidersToIgnore);
            }
            return UniTask.CompletedTask;
        }

        private void IgnoreCollisions(List<Collider> colliders)
        {
            // Ensure the list has colliders to process
            if (colliders == null || colliders.Count < 2) return;

            // Loop through the list and ignore collisions between each pair
            for (int i = 0; i < colliders.Count; i++)
            {
                for (int j = i + 1; j < colliders.Count; j++)
                {
                    if (colliders[i] != null && colliders[j] != null)
                    {
                        Physics.IgnoreCollision(colliders[i], colliders[j], true);
                    }
                }
            }
        }

        private void ResetCollisions(List<Collider> colliders)
        {
            if (colliders == null || colliders.Count < 2) return;

            // Re-enable collisions between each pair
            for (int i = 0; i < colliders.Count; i++)
            {
                for (int j = i + 1; j < colliders.Count; j++)
                {
                    if (colliders[i] != null && colliders[j] != null)
                    {
                        Physics.IgnoreCollision(colliders[i], colliders[j], false);
                    }
                }
            }
        }
    }

        // Data class to store colliders that should ignore collisions
    [System.Serializable]
    public class MonoColliderIgnoreHandlerData : SequenceData
    {
        public bool resetIgnoringWhenUnload = true;
        public List<Collider> collidersToIgnore;
    }
}
