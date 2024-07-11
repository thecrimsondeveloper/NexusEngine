using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Toolkit.Extras;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Samples
{
    [System.Serializable]
    public class RangeCompletable : NexusObject, ICompletable
    {
        [System.Serializable]
        public struct RangeTargets
        {
            [SerializeField, Range(0, 1)] public float value;

        }
        [SerializeField] public List<RangeTargets> rangeTargets;
        [SerializeField, Range(0, 1)] float padding;

        [SerializeField, Range(0, 1)] float currentValue = 0;

        public bool IsComplete { get; set; }

        public UnityEvent OnComplete { get; } = new UnityEvent();
        public UnityEvent OnReset { get; } = new UnityEvent();

        public void Internal_OnComplete()
        {

        }

        protected virtual void OnDialValueChanged(float value)
        {
            CheckDialValue();
        }

        public void SetPadding(float padding)
        {
            this.padding = padding;
        }

        public float SetDialValue(float value = 0)
        {

            //set the value to the value with a remainder of 1
            value = value % 1;
            //if the value is negative, add the difference to 0
            if (value < 0)
            {
                float diff = 0 - value;
                float zeroToOneValue = 1 - diff;
                value = zeroToOneValue;
            }

            currentValue = value;
            OnDialValueChanged(value);
            return currentValue;
        }


        bool IsInRange(float value, float target, float padding)
        {
            float minDialValue = target - padding;
            float maxDialValue = target + padding;
            return value >= minDialValue && value <= maxDialValue;
        }

        void CheckDialValue()
        {
            bool isAnyValueInRange = false;

            //check if the current value is in range of any of the targets
            for (int i = 0; i < rangeTargets.Count; i++)
            {
                RangeTargets rangeTarget = rangeTargets[i];
                isAnyValueInRange = IsInRange(currentValue, rangeTarget.value, padding);
                if (isAnyValueInRange == true)
                {
                    break;
                }
            }

            if (isAnyValueInRange)
            {
                (this as ICompletable).Complete();
            }
            else if (IsComplete)
            {
                (this as ICompletable).Reset();
            }
        }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void Internal_OnReset()
        {

        }

        public void GenerateRangeTargets(int count)
        {
            rangeTargets = new List<RangeTargets>();
            for (int i = 0; i < count; i++)
            {
                RangeTargets rangeTarget = new RangeTargets();
                rangeTarget.value = i / (float)count;
                rangeTargets.Add(rangeTarget);
            }
        }


        public async void GenerateRandomRangeTargets(int count)
        {
            rangeTargets = new List<RangeTargets>();
            for (int i = 0; i < count; i++)
            {
                float startTime = Time.time;
                while (true)
                {
                    float randomValue = Random.Range(0, 1f);

                    //check if the value is already in the list and if they are in range of each other
                    if (OtherTargetsWithinPadding(randomValue) == false)
                    {
                        RangeTargets rangeTarget = new RangeTargets();
                        rangeTarget.value = randomValue;
                        rangeTargets.Add(rangeTarget);
                        break;
                    }

                    //if the loop has been running for more than 2 seconds, break
                    if (Time.time - startTime > 2)
                    {
                        break;
                    }
                    await UniTask.NextFrame();
                }
            }
        }


        bool OtherTargetsWithinPadding(float currentTarget)
        {
            for (int i = 0; i < rangeTargets.Count; i++)
            {
                if (currentTarget == rangeTargets[i].value)
                {
                    continue;
                }
                if (IsInRange(currentTarget, rangeTargets[i].value, padding))
                {
                    return true;
                }
            }
            return false;
        }

        public void SetRangeTargets(List<RangeTargets> rangeTargets)
        {
            this.rangeTargets = rangeTargets;
        }


        public bool AddRangeTarget(float value)
        {
            //check if the value is already in the list
            if (rangeTargets.FindIndex(x => x.value == value) != -1)
            {
                return false;
            }


            RangeTargets rangeTarget = new RangeTargets();
            rangeTarget.value = value;
            rangeTargets.Add(rangeTarget);
            return true;
        }
    }
}
