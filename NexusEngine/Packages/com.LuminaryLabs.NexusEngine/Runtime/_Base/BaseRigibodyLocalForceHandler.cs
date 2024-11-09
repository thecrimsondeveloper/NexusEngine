using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseRigidbodyLocalForceHandler : BaseSequence<BaseRigidbodyLocalForceHandler.BaseRigidbodyLocalForceData>
    {
        // Private fields for initial force
        private Rigidbody targetRigidbody;
        private bool applyInitialForce;
        private Vector3 initialForce;
        private ForceMode initialForceMode;

        // Private fields for continuous force
        private bool applyContinuousForce;
        private Vector3 continuousForce;
        private ForceMode continuousForceMode;
        private float continuousForceDuration;
        private bool isContinuousForceInfinite;

        protected override UniTask Initialize(BaseRigidbodyLocalForceData currentData)
        {
            // Assign data to internal variables
            targetRigidbody = currentData.targetRigidbody;

            applyInitialForce = currentData.applyInitialForce;
            initialForce = currentData.initialForce;
            initialForceMode = currentData.initialForceMode;

            applyContinuousForce = currentData.applyContinuousForce;
            continuousForce = currentData.continuousForce;
            continuousForceMode = currentData.continuousForceMode;
            continuousForceDuration = currentData.continuousForceDuration;
            isContinuousForceInfinite = Mathf.Approximately(continuousForceDuration, 1f);

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (targetRigidbody == null)
            {
                Debug.LogError("Rigidbody not assigned in BaseRigidbodyLocalForceHandler.");
                Complete();
                return;
            }

            // Apply the initial force
            if (applyInitialForce)
            {
                ApplyLocalForce(initialForce, initialForceMode);
            }

            // Apply continuous force if specified
            if (applyContinuousForce)
            {
                await ApplyContinuousForce();
            }

            Complete();
        }

        private void ApplyLocalForce(Vector3 force, ForceMode mode)
        {
            // Apply force in the local space of the Rigidbody
            targetRigidbody.AddForce(targetRigidbody.transform.TransformDirection(force), mode);
        }

        private async UniTask ApplyContinuousForce()
        {
            float elapsedTime = 0f;

            while (isContinuousForceInfinite || elapsedTime < continuousForceDuration)
            {
                ApplyLocalForce(continuousForce, continuousForceMode);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        protected override UniTask Unload()
        {
            // Cleanup if necessary
            return UniTask.CompletedTask;
        }

        [System.Serializable]
        public class BaseRigidbodyLocalForceData : BaseSequenceData
        {
            public Rigidbody targetRigidbody;

            [Header("Initial Force Settings")]
            public bool applyInitialForce;
            public Vector3 initialForce = Vector3.zero;
            public ForceMode initialForceMode = ForceMode.Force;

            [Header("Continuous Force Settings")]
            public bool applyContinuousForce;
            public Vector3 continuousForce = Vector3.zero;
            public ForceMode continuousForceMode = ForceMode.Force;

            [Tooltip("Duration for continuous force application. Set to 0 for instant, 1 for infinite.")]
            [Range(0, 1)]
            public float continuousForceDuration = 0f;
        }
    }
}
