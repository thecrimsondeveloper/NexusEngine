using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class AlignWithMovementHandler : EntitySequence<AlignWithMovementHandler.AlignWithMovementData>
    {
        private Transform transformToAlign;
        private Vector3 lastPosition;
        private float dampingDivisor;

        protected override UniTask Initialize(AlignWithMovementData currentData)
        {
            // Store the transform to align and initialize the damping factor and last position
            transformToAlign = currentData.transformToAlign;
            dampingDivisor = Mathf.Max(currentData.dampingDivisor, 0); // Ensure non-negative value
            lastPosition = transformToAlign.position;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Initialize the last known position
            lastPosition = transformToAlign.position;
        }

        private void Update()
        {
            if (this.phase != Phase.Run || transformToAlign == null)
                return;

            // Calculate the direction of movement
            Vector3 currentPosition = transformToAlign.position;
            Vector3 movementDirection = currentPosition - lastPosition;

            // Align the transform if there was significant movement
            if (movementDirection.sqrMagnitude > 0.0001f)
            {
                // Calculate the target rotation
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection.normalized);

                if (dampingDivisor <= 0)
                {
                    // Instant alignment if dampingDivisor is 0
                    transformToAlign.forward = movementDirection.normalized;
                }
                else
                {
                    // Smooth alignment using Lerp with the damping divisor
                    float lerpFactor = 1 / dampingDivisor * Time.deltaTime;
                    Debug.Log(lerpFactor);
                    transformToAlign.rotation = Quaternion.Lerp(
                        transformToAlign.rotation,
                        targetRotation,
                        lerpFactor
                    );
                }
            }

            // Update the last position for the next frame
            lastPosition = currentPosition;
        }

        protected override UniTask Unload()
        {
            // Clean up if necessary
            return UniTask.CompletedTask;
        }

        [System.Serializable]
        public class AlignWithMovementData : SequenceData
        {
            public Transform transformToAlign;
            
            [Tooltip("Damping divisor. If set to 0, the alignment will be instant.")]
            [Range(0f, 1f)] 
            public float dampingDivisor = 1f;
        }
    }
}
