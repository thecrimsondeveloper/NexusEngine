using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Samples
{
    public class DialCompletable : MonoBehaviour, ICompletable
    {
        enum RotationAxis { X, Y, Z }

        [Title("Settings")]
        [SerializeField] RotationAxis rotationAxis = RotationAxis.Y;
        [SerializeField] int numberOfTargets = 4;
        [SerializeField] int maxNumberOfTargets = 4;
        [SerializeField, Range(1, 180)] int completeRangeInDegrees = 10;
        [SerializeField] float requiredHoldTime = 1;
        [SerializeField, Range(0, 1)] float visualMoveDampening = 0;
        [SerializeField] bool resetWhenLeaveRange = true;
        [SerializeField] bool snapWhenInRange = false;
        [SerializeField] bool changeTargetsOnReset = false;


        [Title("References")]
        [SerializeField] Transform visual;

        [Title("Dependencies")]
        [SerializeField] RangeCompletable rangeCompletable;

        [Title("State")]
        [ShowInInspector] public bool IsComplete { get; set; } = false;
        [SerializeField] float currentValue = 0;
        public UnityEvent OnComplete { get; } = new UnityEvent();
        public UnityEvent OnReset { get; } = new UnityEvent();

        float targetValue
        {
            get;
            [Button]
            set;
        } = 0;

        private void OnValidate()
        {
            //number of targets that can fit
            maxNumberOfTargets = Mathf.FloorToInt(360 / completeRangeInDegrees);
            numberOfTargets = Mathf.Clamp(numberOfTargets, 0, maxNumberOfTargets);
        }


        public float CurrentValue => currentValue;

        private void Awake()
        {
            rangeCompletable = ScriptableObject.CreateInstance<RangeCompletable>();
            rangeCompletable.InitializeObject();
        }

        private void Start()
        {
            rangeCompletable.OnComplete.AddListener(OnRangeComplete);
            rangeCompletable.OnReset.AddListener(OnRangeReset);
            rangeCompletable.GenerateRandomRangeTargets(numberOfTargets);
            rangeCompletable.SetPadding(completeRangeInDegrees / 360f / 2);
        }


        void OnRangeComplete()
        {
            isHoldingDialInPosition = true;
        }

        void OnRangeReset()
        {
            isHoldingDialInPosition = false;



            if (resetWhenLeaveRange)
            {
                (this as ICompletable).Reset();
            }
        }

        public void GenerateRandomRangeTargets()
        {
            rangeCompletable.GenerateRandomRangeTargets(numberOfTargets);
        }

        public void Internal_OnComplete()
        {
            Debug.Log("Internal_OnComplete - DialCompletable");
        }

        bool isHoldingDialInPosition = false;
        float timeHolding = 0;
        float currentAngle = 0;

        private void Update()
        {
            if (isHoldingDialInPosition && IsComplete == false)
            {
                timeHolding += Time.deltaTime;

                if (timeHolding >= requiredHoldTime)
                {
                    (this as ICompletable).Complete();
                }

                // if (snapWhenInRange)
                // {
                //     float closestValue = 0;
                //     float closestDistance = 1;
                //     for (int i = 0; i < rangeCompletable.rangeTargets.Length; i++)
                //     {
                //         float currentValue = rangeCompletable.rangeTargets[i].value;
                //         float distance = Mathf.Abs(currentValue - currentValue);
                //         if (distance < closestDistance)
                //         {
                //             closestDistance = distance;
                //             closestValue = currentValue;
                //         }
                //     }
                //     targetValue = closestValue;
                // }
            }
            else
            {
                timeHolding = 0;
            }


            currentValue = visualMoveDampening == 0 ? targetValue :
                                                Mathf.Lerp(currentValue, targetValue, Time.deltaTime * visualMoveDampening * 10);
            if (visual != null)
            {


                float rotationAngle = currentValue * 360 % 360;
                //lerp the rotation angle to the target value
                currentAngle = (visualMoveDampening == 0) ? rotationAngle
                                : Mathf.LerpAngle(currentAngle, rotationAngle, Time.deltaTime * (1 / visualMoveDampening) * 10);
                // Set the dial value

                switch (rotationAxis)
                {
                    case RotationAxis.X:
                        visual.localRotation = Quaternion.Euler(currentAngle, visual.localRotation.eulerAngles.y, visual.localRotation.eulerAngles.z);
                        break;
                    case RotationAxis.Y:
                        visual.localRotation = Quaternion.Euler(visual.localRotation.eulerAngles.x, currentAngle, visual.localRotation.eulerAngles.z);
                        break;
                    case RotationAxis.Z:
                        visual.localRotation = Quaternion.Euler(visual.localRotation.eulerAngles.x, visual.localRotation.eulerAngles.y, currentAngle);
                        break;
                }
            }

            rangeCompletable.SetDialValue(currentValue);
        }



        public void SetDialValue(float value = 0)
        {
            targetValue = Mathf.Clamp01(value);
        }


        protected virtual void OnDialValueChanged(float value)
        {

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
            if (changeTargetsOnReset)
            {
                rangeCompletable.GenerateRandomRangeTargets(numberOfTargets);
            }
        }


#if UNITY_EDITOR

        [Button]
        void Complete()
        {
            (this as ICompletable).Complete();
        }

        [Button]
        void Reset()
        {
            (this as ICompletable).Reset();
        }
#endif
    }
}
