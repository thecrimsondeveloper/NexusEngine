using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseRigidbodyConfigureHandler : BaseSequence<BaseRigidbodyConfigureHandlerData>
    {
        // Private fields for Rigidbody settings
        private Rigidbody targetRigidbody;

        private bool setInitialGravity;
        private bool initialUseGravity;

        private bool setInitialDrag;
        private float initialDrag;

        private bool setInitialAngularDrag;
        private float initialAngularDrag;

        private bool setInitialKinematic;
        private bool initialIsKinematic;

        private bool setInitialConstraints;
        private RigidbodyConstraints initialConstraints;

        protected override UniTask Initialize(BaseRigidbodyConfigureHandlerData currentData)
        {
            // Store initial settings
            targetRigidbody = currentData.targetRigidbody;

            setInitialGravity = currentData.setInitialGravity;
            initialUseGravity = currentData.initialUseGravity;

            setInitialDrag = currentData.setInitialDrag;
            initialDrag = currentData.initialDrag;

            setInitialAngularDrag = currentData.setInitialAngularDrag;
            initialAngularDrag = currentData.initialAngularDrag;

            setInitialKinematic = currentData.setInitialKinematic;
            initialIsKinematic = currentData.initialIsKinematic;

            setInitialConstraints = currentData.setInitialConstraints;
            initialConstraints = currentData.initialConstraints;

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (targetRigidbody == null)
            {
                Debug.LogError("Rigidbody not assigned in BaseRigidbodyConfigureHandler.");
                Complete();
                return;
            }

            // Apply initial settings
            ApplyInitialSettings();

            Complete();
        }

        private void ApplyInitialSettings()
        {
            if (setInitialGravity)
                targetRigidbody.useGravity = initialUseGravity;

            if (setInitialDrag)
                targetRigidbody.drag = initialDrag;

            if (setInitialAngularDrag)
                targetRigidbody.angularDrag = initialAngularDrag;

            if (setInitialKinematic)
                targetRigidbody.isKinematic = initialIsKinematic;

            if (setInitialConstraints)
                targetRigidbody.constraints = initialConstraints;
        }

        protected override UniTask Unload()
        {
            // Cleanup if necessary
            return UniTask.CompletedTask;
        }

    }
    [System.Serializable]
    public class BaseRigidbodyConfigureHandlerData : BaseSequenceData
    {
        public Rigidbody targetRigidbody;

        [Header("Gravity Settings")]
        public bool setInitialGravity;
        public bool initialUseGravity = true;

        [Header("Drag Settings")]
        public bool setInitialDrag;
        public float initialDrag = 0f;

        [Header("Angular Drag Settings")]
        public bool setInitialAngularDrag;
        public float initialAngularDrag = 0.05f;

        [Header("Kinematic Settings")]
        public bool setInitialKinematic;
        public bool initialIsKinematic = false;

        [Header("Constraints Settings")]
        public bool setInitialConstraints;
        public RigidbodyConstraints initialConstraints = RigidbodyConstraints.None;
    }
}
