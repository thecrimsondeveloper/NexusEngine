using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseSuperTransformHandler : BaseSequence<BaseSuperTransformHandler.BaseSuperTransformHandlerData>
    {
        // Private internal variables for all data fields
        private Transform newParent;

        private bool setPosition;
        private Vector3 position;
        private Space space;

        private bool setRotation;
        private Quaternion rotation;
        private Space rotationSpace;

        private bool setScale;
        private Vector3 scale;

        protected override UniTask Initialize(BaseSuperTransformHandlerData currentData)
        {
            // Store data into internal variables for use in OnBegin
            newParent = currentData.newParent;

            setPosition = currentData.setPosition;
            position = currentData.position;
            space = currentData.space;

            setRotation = currentData.setRotation;
            rotation = currentData.rotation;
            rotationSpace = currentData.rotationSpace;

            setScale = currentData.setScale;
            scale = currentData.scale;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Ensure the GameObject's transform is set
            Transform superTransform = superSequence.GetTransform();
            
            if (superTransform == null)
            {
                Debug.LogError("Transform not assigned in BaseSuperTransformHandler.");
                Complete();
                return;
            }

            // Set the parent if specified
            if (newParent != null)
            {
                superTransform.SetParent(newParent);
            }

            // Set position if enabled
            if (setPosition)
            {
                SetPosition(superTransform, position, space);
            }

            // Set rotation if enabled
            if (setRotation)
            {
                SetRotation(superTransform, rotation, rotationSpace);
            }

            // Set scale if enabled
            if (setScale)
            {
                SetScale(superTransform, scale);
            }

            // Complete the sequence after setting the transforms
            Complete();
        }

        protected override UniTask Unload()
        {
            // Cleanup if necessary
            return UniTask.CompletedTask;
        }

        // Utility method to set the position
        private void SetPosition(Transform superTransform, Vector3 newPosition, Space space)
        {
            if (space == Space.World)
                superTransform.position = newPosition;
            else
                superTransform.localPosition = newPosition;
        }

        // Utility method to set the rotation
        private void SetRotation(Transform superTransform, Quaternion newRotation, Space space)
        {
            if (space == Space.World)
                superTransform.rotation = newRotation;
            else
                superTransform.localRotation = newRotation;
        }

        // Utility method to set the scale
        private void SetScale(Transform superTransform, Vector3 newScale)
        {
            superTransform.localScale = newScale;
        }
        [System.Serializable]
        public class BaseSuperTransformHandlerData : BaseSequenceData
        {
            public Transform newParent;

            [Header("Position Settings")]
            public bool setPosition;
            public Vector3 position = Vector3.zero;
            public Space space = Space.World;

            [Header("Rotation Settings")]
            public bool setRotation;
            public Quaternion rotation = Quaternion.identity;
            public Space rotationSpace = Space.World;

            [Header("Scale Settings")]
            public bool setScale;
            public Vector3 scale = Vector3.one;
        }
    }

}
