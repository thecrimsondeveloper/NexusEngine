using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class RotateTransformWithMouse : EntitySequence<RotateTransformWithMouseData>
    {
        private Transform targetTransform;
        private float rotationSpeed;

        /// <summary>
        /// Initializes the entity sequence with the provided data.
        /// </summary>
        /// <param name="currentData">The current data for the entity sequence.</param>
        /// <returns>A UniTask representing the initialization process.</returns>
        protected override UniTask Initialize(RotateTransformWithMouseData currentData)
        {
            Debug.Log("Entity Sequence Initialized");

            // Assign data from currentData
            targetTransform = currentData.targetTransform;
            rotationSpeed = currentData.rotationSpeed;

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called when the sequence begins.
        /// </summary>
        protected override void OnBegin()
        {
            Debug.Log("Entity Sequence Started");
        }

        /// <summary>
        /// Update is called once per frame to handle mouse rotation.
        /// </summary>
        private void Update()
        {
            if (targetTransform == null) return;

            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotate the transform based on mouse input
            targetTransform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
            targetTransform.Rotate(Vector3.right, -mouseY * rotationSpeed, Space.Self);
        }
    }

    /// <summary>
    /// Data class for RotateTransformWithMouse.
    /// </summary>
    [System.Serializable]
    public class RotateTransformWithMouseData : EntitySequenceData
    {
        [Tooltip("The transform to rotate.")]
        public Transform targetTransform;

        [Tooltip("Rotation speed multiplier.")]
        public float rotationSpeed = 100f;
    }
}
