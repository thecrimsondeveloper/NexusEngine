using UnityEngine;
using Cysharp.Threading.Tasks;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class WaitForCollisionHandler : EntitySequence<WaitForCollisionHandlerData>
    {
        // Enum for choosing between Enter and Exit events
        public enum CollisionEventType
        {
            Enter,
            Exit
        }

        // Enum for choosing between Collision and Trigger events
        public enum CollisionDetectionType
        {
            Collision,
            Trigger
        }

        private Collider targetCollider;
        private CollisionEventType eventType;
        private CollisionDetectionType detectionType;

        /// <summary>
        /// Initializes the sequence with the provided data.
        /// </summary>
        /// <param name="currentData">The current data for the sequence.</param>
        /// <returns>A UniTask representing the initialization process.</returns>
        protected override UniTask Initialize(WaitForCollisionHandlerData currentData)
        {
            targetCollider = currentData.collider;
            eventType = currentData.eventType;
            detectionType = currentData.detectionType;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Debug.Log("WaitForCollisionHandler started");
        }

        /// <summary>
        /// Handles Unity's OnCollisionEnter event.
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (detectionType != CollisionDetectionType.Collision || eventType != CollisionEventType.Enter) return;
            HandleCollision(collision.collider);
        }

        /// <summary>
        /// Handles Unity's OnCollisionExit event.
        /// </summary>
        private void OnCollisionExit(Collision collision)
        {
            if (detectionType != CollisionDetectionType.Collision || eventType != CollisionEventType.Exit) return;
            HandleCollision(collision.collider);
        }

        /// <summary>
        /// Handles Unity's OnTriggerEnter event.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (detectionType != CollisionDetectionType.Trigger || eventType != CollisionEventType.Enter) return;
            HandleCollision(other);
        }

        /// <summary>
        /// Handles Unity's OnTriggerExit event.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (detectionType != CollisionDetectionType.Trigger || eventType != CollisionEventType.Exit) return;
            HandleCollision(other);
        }

        /// <summary>
        /// Handles the collision or trigger event based on the specified collider.
        /// </summary>
        /// <param name="collider">The collider involved in the event.</param>
        private void HandleCollision(Collider collider)
        {
            if (targetCollider == null || collider == targetCollider)
            {
                Debug.Log($"Detected {eventType} with {detectionType}");
                CompleteSequence();
            }
        }

        /// <summary>
        /// Completes and stops the sequence.
        /// </summary>
        private void CompleteSequence()
        {
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class WaitForCollisionHandlerData : SequenceData
    {
        public Collider collider; // Optional: the specific collider to wait for (null if any collider is acceptable)
        public WaitForCollisionHandler.CollisionEventType eventType; // Specifies whether to finish on Enter or Exit
        public WaitForCollisionHandler.CollisionDetectionType detectionType; // Specifies whether to respond to Collisions or Triggers
    }
}
