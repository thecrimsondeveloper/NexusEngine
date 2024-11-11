using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseColliderIgnoreHandler : BaseSequence<BaseColliderIgnoreData>
    {
        private bool resetIgnoringWhenUnload;
        private List<Collider> collidersToIgnore;

        /// <summary>
        /// Initializes the sequence with the provided data.
        /// </summary>
        /// <param name="currentData">The current data for the sequence.</param>
        /// <returns>A UniTask representing the initialization process.</returns>
        protected override UniTask Initialize(BaseColliderIgnoreData currentData)
        {
            // Set up data from currentData
            collidersToIgnore = currentData.collidersToIgnore;
            resetIgnoringWhenUnload = currentData.resetIgnoringWhenUnload;
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called when the sequence begins.
        /// </summary>
        protected override void OnBegin()
        {
            IgnoreCollisions(collidersToIgnore);
        }

        /// <summary>
        /// Called when the sequence is unloaded.
        /// </summary>
        protected override UniTask Unload()
        {
            if (resetIgnoringWhenUnload)
            {
                ResetCollisions(collidersToIgnore);
            }
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Sets the specified colliders to ignore each other.
        /// </summary>
        private void IgnoreCollisions(List<Collider> colliders)
        {
            if (colliders == null || colliders.Count < 2) return;

            // Loop through all pairs of colliders and set them to ignore each other
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

        /// <summary>
        /// Resets the collision settings between the specified colliders.
        /// </summary>
        private void ResetCollisions(List<Collider> colliders)
        {
            if (colliders == null || colliders.Count < 2) return;

            // Loop through all pairs of colliders and re-enable collision
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
    /// <summary>
    /// Data class to store colliders that should ignore collisions.
    /// </summary>
    [System.Serializable]
    public class BaseColliderIgnoreData : BaseSequenceData
    {
        [Tooltip("List of colliders to ignore collisions between.")]
        public List<Collider> collidersToIgnore;

        [Tooltip("Reset collision ignoring when the sequence is unloaded.")]
        public bool resetIgnoringWhenUnload = true;
    }
}
