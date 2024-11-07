using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class TransformUpdateHandler : BaseSequence<TransformUpdateHandler.TransformUpdateHandlerData>
    {
        private Transform targetTransform;
        private bool modifyPosition;
        private bool modifyRotation;
        private bool modifyScale;
        private Vector3 positionInput;
        private Vector3 rotation;
        private Vector3 scale;
        private Space space;
        private AnimationCurve ease;

        private float duration;
        private Vector3 initialPosition;
        private Vector3 initialRotation;
        private Vector3 initialScale;
        private float elapsedTime;
        private ModificationType modificationType;

        protected override UniTask Initialize(TransformUpdateHandlerData currentData)
        {
            // Set private variables from data
            targetTransform = currentData.targetTransform == null ? superSequence.GetTransform() : currentData.targetTransform;
            modifyPosition = currentData.modifyPosition;
            modifyRotation = currentData.modifyRotation;
            modifyScale = currentData.modifyScale;
            positionInput = currentData.positionInput;
            rotation = currentData.rotation;
            scale = currentData.scale;
            space = currentData.space;
            ease = currentData.ease;
            duration = currentData.duration;
            modificationType = currentData.modificationType;

            // Store initial values for interpolation
            if (targetTransform != null)
            {
                initialPosition = targetTransform.position;
                initialRotation = targetTransform.eulerAngles;
                initialScale = targetTransform.localScale;
            }

            elapsedTime = 0f;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Start the tweening sequence
            UpdateTransform().Forget();
        }

        private async UniTaskVoid UpdateTransform()
        {
            while (elapsedTime < duration)
            {
                if(Application.isPlaying == false)
                {
                    break;
                }
                elapsedTime += Time.deltaTime;
                float t = ease.Evaluate(Mathf.Clamp01(elapsedTime / duration));

                // Modify position if enabled
                if (modifyPosition)
                {
                    Vector3 targetPosition = (modificationType == ModificationType.SET) ? positionInput : initialPosition + positionInput;
                    if (space == Space.World)
                    {
                        targetTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                    }
                    else if (space == Space.Self)
                    {
                        targetTransform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
                    }
                }

                // Modify rotation if enabled
                if (modifyRotation)
                {
                    Vector3 targetRotation = (modificationType == ModificationType.SET) ? rotation : initialRotation + rotation;
                    targetTransform.rotation = Quaternion.Lerp(Quaternion.Euler(initialRotation), Quaternion.Euler(targetRotation), t);
                }

                // Modify scale if enabled
                if (modifyScale)
                {
                    Vector3 targetScale = (modificationType == ModificationType.SET) ? scale : initialScale + scale;
                    targetTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                }

                // Await a frame to yield control and allow the next update
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            // Complete the transformation
            Complete().Forget();
        }

        private async UniTaskVoid Complete()
        {
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        public enum ModificationType
        {
            SET,  // Modify to set values
            ADD   // Modify by adding to current values
        }

        [System.Serializable]
        public class TransformUpdateHandlerData : BaseSequenceData
        {
            public Transform targetTransform;

            [Space(10)]
            public bool modifyPosition = false;
            public Space space = Space.Self;
            public Vector3 positionInput;

            [Space(10)]
            public bool modifyRotation = false;
            public Vector3 rotation;

            [Space(10)]
            public bool modifyScale = false;
            public Vector3 scale = Vector3.one;

            [Space(10)]
            public float duration = 1.0f;

            public AnimationCurve ease = AnimationCurve.Linear(0, 0, 1, 1);

            public ModificationType modificationType = ModificationType.SET; // Specify whether to use SET or ADD
        }
    }
}
