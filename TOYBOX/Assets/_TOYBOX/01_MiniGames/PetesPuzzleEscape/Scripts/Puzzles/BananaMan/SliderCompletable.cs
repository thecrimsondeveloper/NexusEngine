using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Samples
{
    [System.Serializable]
    public class SliderCompletable : MonoBehaviour, ICompletable
    {
        [SerializeField] private Transform startMarker;
        [SerializeField] private Transform endMarker;
        [SerializeField] private List<float> targetValues;

        public Transform knobTransformTarget;
        [SerializeField] private float padding = 0.01f;
        [SerializeField] private float requiredHoldTime = 1f;

        [SerializeField]
        enum SliderType
        {
            X,
            Y,
            Z
        }

        [SerializeField] private SliderType sliderType = SliderType.X;

        public UnityEvent OnComplete { get; } = new UnityEvent();
        public UnityEvent OnReset { get; } = new UnityEvent();

        public bool IsComplete { get; set; } = false;

        [SerializeField] private float currentValue = 0;
        private bool isHoldingAtTarget = false;
        private float timeFirstHeld = 0f;

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void Internal_OnComplete()
        {
            Debug.Log("Internal_OnComplete - SliderCompletable");
        }

        public void Internal_OnReset()
        {
            Debug.Log("Internal_OnReset - SliderCompletable");
        }

        private void Update()
        {
            if (IsComplete)
                return;

            // Determine the distance along the given 
            float distanceFromStart = Vector3.Dot(knobTransformTarget.position - startMarker.position, endMarker.position - startMarker.position);
            float totalDistance = Vector3.Distance(startMarker.position, endMarker.position);

            //calculate the current value of the slider as a zero to one value
            currentValue = distanceFromStart / totalDistance;

            // Check if the slider has reached any of the target values
            foreach (var targetValue in targetValues)
            {
                // If the current value is within the padding of the target value
                if (Mathf.Abs(currentValue - targetValue) < padding)
                {
                    // If not holding at the target, start holding
                    if (!isHoldingAtTarget)
                    {
                        isHoldingAtTarget = true;
                        timeFirstHeld = Time.time;
                    }
                    else
                    {
                        // If holding for required hold time, complete
                        if (Time.time - timeFirstHeld >= requiredHoldTime)
                        {
                            IsComplete = true;
                            OnComplete.Invoke();
                        }
                    }
                    return; // Exit loop early if target value found
                }
            }

            // Reset hold state if not at target value
            isHoldingAtTarget = false;
        }
    }
}
