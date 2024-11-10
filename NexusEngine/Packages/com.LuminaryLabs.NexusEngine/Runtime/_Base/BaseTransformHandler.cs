using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseTransformHandler : BaseSequence<BaseTransformHandler.BaseTransformHandlerData>
    {
        // Private internal variables for all data fields
        private Transform targetTransform;
        private Transform newParent;

        private bool setPosition;
        private Vector3 position;
        private Space space;

        private bool setRotation;
        private Quaternion rotation;
        private Space rotationSpace;

        private bool setScale;
        private Vector3 scale;

        protected override UniTask Initialize(BaseTransformHandlerData currentData)
        {
            // Store data into internal variables for use in OnBegin
            targetTransform = currentData.targetTransform;
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
            // Ensure the target transform is set
            if (targetTransform == null)
            {
                Debug.LogError("Transform not assigned in BaseTransformHandler.");
                Complete();
                return;
            }

            // Set the parent if specified
            if (newParent != null)
            {
                targetTransform.SetParent(newParent);
            }

            // Set position if enabled
            if (setPosition)
            {
                SetPosition(position, space);
            }

            // Set rotation if enabled
            if (setRotation)
            {
                SetRotation(rotation, rotationSpace);
            }

            // Set scale if enabled
            if (setScale)
            {
                SetScale(scale);
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
        private void SetPosition(Vector3 newPosition, Space space)
        {
            if (space == Space.World)
                targetTransform.position = newPosition;
            else
                targetTransform.localPosition = newPosition;
        }

        // Utility method to set the rotation
        private void SetRotation(Quaternion newRotation, Space space)
        {
            if (space == Space.World)
                targetTransform.rotation = newRotation;
            else
                targetTransform.localRotation = newRotation;
        }

        // Utility method to set the scale
        private void SetScale(Vector3 newScale)
        {
            targetTransform.localScale = newScale;
        }
        [System.Serializable]
        public class BaseTransformHandlerData : BaseSequenceData
        {
            public Transform targetTransform;
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
