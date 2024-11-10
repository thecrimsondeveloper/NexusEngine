using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseRigidbodyHandler : BaseSequence<BaseRigidbodyHandler.BaseRigidbodyHandlerData>
    {
        // Private fields for initial settings
        private Rigidbody targetRigidbody;

        private bool setInitialVelocity;
        private Vector3 initialVelocity;
        private Space initialVelocitySpace;

        private bool setInitialAngularVelocity;
        private Vector3 initialAngularVelocity;
        private Space initialAngularVelocitySpace;

        private bool setInitialGravity;
        private bool initialUseGravity;

        private bool setInitialDrag;
        private float initialDrag;

        private bool setInitialAngularDrag;
        private float initialAngularDrag;

        private bool setInitialKinematic;
        private bool initialIsKinematic;

        // Private fields for over time settings
        private bool setOverTimeVelocity;
        private Vector3 overTimeVelocity;
        private Space overTimeVelocitySpace;

        private bool setOverTimeAngularVelocity;
        private Vector3 overTimeAngularVelocity;
        private Space overTimeAngularVelocitySpace;

        private bool setOverTimeDrag;
        private float overTimeDrag;

        private bool setOverTimeAngularDrag;
        private float overTimeAngularDrag;

        private float overTimeDuration;
        private bool isOverTimeInfinite;

        protected override UniTask Initialize(BaseRigidbodyHandlerData currentData)
        {
            // Store initial settings
            targetRigidbody = currentData.targetRigidbody;

            setInitialVelocity = currentData.setInitialVelocity;
            initialVelocity = currentData.initialVelocity;
            initialVelocitySpace = currentData.initialVelocitySpace;

            setInitialAngularVelocity = currentData.setInitialAngularVelocity;
            initialAngularVelocity = currentData.initialAngularVelocity;
            initialAngularVelocitySpace = currentData.initialAngularVelocitySpace;

            setInitialGravity = currentData.setInitialGravity;
            initialUseGravity = currentData.initialUseGravity;

            setInitialDrag = currentData.setInitialDrag;
            initialDrag = currentData.initialDrag;

            setInitialAngularDrag = currentData.setInitialAngularDrag;
            initialAngularDrag = currentData.initialAngularDrag;

            setInitialKinematic = currentData.setInitialKinematic;
            initialIsKinematic = currentData.initialIsKinematic;

            // Store over time settings
            setOverTimeVelocity = currentData.setOverTimeVelocity;
            overTimeVelocity = currentData.overTimeVelocity;
            overTimeVelocitySpace = currentData.overTimeVelocitySpace;

            setOverTimeAngularVelocity = currentData.setOverTimeAngularVelocity;
            overTimeAngularVelocity = currentData.overTimeAngularVelocity;
            overTimeAngularVelocitySpace = currentData.overTimeAngularVelocitySpace;

            setOverTimeDrag = currentData.setOverTimeDrag;
            overTimeDrag = currentData.overTimeDrag;

            setOverTimeAngularDrag = currentData.setOverTimeAngularDrag;
            overTimeAngularDrag = currentData.overTimeAngularDrag;

            overTimeDuration = currentData.overTimeDuration;
            isOverTimeInfinite = Mathf.Approximately(overTimeDuration, 1f);

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (targetRigidbody == null)
            {
                Debug.LogError("Rigidbody not assigned in BaseRigidbodyHandler.");
                Complete();
                return;
            }

            // Apply initial settings
            ApplyInitialSettings();

            // Apply over time settings if specified
            if (setOverTimeVelocity || setOverTimeAngularVelocity || setOverTimeDrag || setOverTimeAngularDrag)
            {
                await ApplyOverTimeSettings();
            }

            Complete();
        }

        private void ApplyInitialSettings()
        {
            if (setInitialVelocity)
            {
                if (initialVelocitySpace == Space.World)
                    targetRigidbody.velocity = initialVelocity;
                else
                    targetRigidbody.velocity = targetRigidbody.transform.TransformDirection(initialVelocity);
            }

            if (setInitialAngularVelocity)
            {
                if (initialAngularVelocitySpace == Space.World)
                    targetRigidbody.angularVelocity = initialAngularVelocity;
                else
                    targetRigidbody.angularVelocity = targetRigidbody.transform.TransformDirection(initialAngularVelocity);
            }

            if (setInitialGravity)
                targetRigidbody.useGravity = initialUseGravity;

            if (setInitialDrag)
                targetRigidbody.drag = initialDrag;

            if (setInitialAngularDrag)
                targetRigidbody.angularDrag = initialAngularDrag;

            if (setInitialKinematic)
                targetRigidbody.isKinematic = initialIsKinematic;
        }

        private async UniTask ApplyOverTimeSettings()
        {
            float elapsedTime = 0f;

            while (isOverTimeInfinite || elapsedTime < overTimeDuration)
            {
                if (setOverTimeVelocity)
                {
                    Vector3 targetVelocity = (overTimeVelocitySpace == Space.World)
                        ? overTimeVelocity
                        : targetRigidbody.transform.TransformDirection(overTimeVelocity);
                    targetRigidbody.velocity = Vector3.Lerp(targetRigidbody.velocity, targetVelocity, Time.deltaTime);
                }

                if (setOverTimeAngularVelocity)
                {
                    Vector3 targetAngularVelocity = (overTimeAngularVelocitySpace == Space.World)
                        ? overTimeAngularVelocity
                        : targetRigidbody.transform.TransformDirection(overTimeAngularVelocity);
                    targetRigidbody.angularVelocity = Vector3.Lerp(targetRigidbody.angularVelocity, targetAngularVelocity, Time.deltaTime);
                }

                if (setOverTimeDrag)
                    targetRigidbody.drag = Mathf.Lerp(targetRigidbody.drag, overTimeDrag, Time.deltaTime);

                if (setOverTimeAngularDrag)
                    targetRigidbody.angularDrag = Mathf.Lerp(targetRigidbody.angularDrag, overTimeAngularDrag, Time.deltaTime);

                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        [System.Serializable]
        public class BaseRigidbodyHandlerData : BaseSequenceData
        {
            public Rigidbody targetRigidbody;

            [Header("Initial Settings")]
            public bool setInitialVelocity;
            public Vector3 initialVelocity = Vector3.zero;
            public Space initialVelocitySpace = Space.World;

            public bool setInitialAngularVelocity;
            public Vector3 initialAngularVelocity = Vector3.zero;
            public Space initialAngularVelocitySpace = Space.World;

            public bool setInitialGravity;
            public bool initialUseGravity = true;

            public bool setInitialDrag;
            public float initialDrag = 0f;

            public bool setInitialAngularDrag;
            public float initialAngularDrag = 0.05f;

            public bool setInitialKinematic;
            public bool initialIsKinematic = false;

            [Header("Over Time Settings")]
            public bool setOverTimeVelocity;
            public Vector3 overTimeVelocity = Vector3.zero;
            public Space overTimeVelocitySpace = Space.World;

            public bool setOverTimeAngularVelocity;
            public Vector3 overTimeAngularVelocity = Vector3.zero;
            public Space overTimeAngularVelocitySpace = Space.World;

            public bool setOverTimeDrag;
            public float overTimeDrag = 0f;

            public bool setOverTimeAngularDrag;
            public float overTimeAngularDrag = 0.05f;

            [Tooltip("Duration for over time changes. Set to 0 for instant, 1 for infinite.")]
            [Range(0, 1)]
            public float overTimeDuration = 0f;
        }
    }
}
