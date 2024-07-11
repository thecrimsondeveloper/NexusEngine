using System;
using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Toolkit.Samples;
using Toolkit.Sequences;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class SunAndMoonPuzzle : MonoSequence
    {
        [SerializeField] private DialCompletable outsideCrankCompleteable; //populated in the inspector
        [SerializeField] private DialCompletable insideCrankCompleteable; //populated in the inspector
        [SerializeField] private LockCompletable sunLock; //populated in the inspector
        [SerializeField] private LockCompletable moonLock; //populated in the inspector

        [SerializeField] private GameObject minuteHand; //populated in the inspector
        [SerializeField] private GameObject hourHand; //populated in the inspector

        [SerializeField] private GameObject sun; //populated in the inspector
        [SerializeField] private GameObject moon; //populated in the inspector

        [SerializeField] private Transform outsideCrank; //populated in the inspector
        [SerializeField] private Transform insideCrank; //populated in the inspector

        private void Start()
        {
            outsideCrankCompleteable.OnComplete.AddListener(OnCrankComplete);
            insideCrankCompleteable.OnComplete.AddListener(OnCrankComplete);
            sunLock.OnComplete.AddListener(OnSunLockComplete);
            moonLock.OnComplete.AddListener(OnMoonLockComplete);

            outsideCrankCompleteable.Deactivate();
            insideCrankCompleteable.Deactivate();
        }

        private void Update()
        {
            float outsideCrankAngle = outsideCrank.transform.localEulerAngles.z;
            minuteHand.transform.localEulerAngles = new Vector3(0, 0, outsideCrankAngle);  //set the minute hand to the same angle as the crank
            //convert the value from 0 - 360 to 0 - 1
            outsideCrankAngle = outsideCrankAngle / 360;
            outsideCrankCompleteable.SetDialValue(outsideCrankAngle);

            float insideCrankAngle = insideCrank.transform.localEulerAngles.z;
            hourHand.transform.localEulerAngles = new Vector3(0, 0, insideCrankAngle);  //set the hour hand to the same angle as the crank
            //convert the value from 0 - 360 to 0 - 1
            insideCrankAngle = insideCrankAngle / 360;
            insideCrankCompleteable.SetDialValue(insideCrankAngle);


            if (outsideCrankCompleteable.IsComplete)
            {
                OnCrankComplete();
            }

            if (insideCrankCompleteable.IsComplete)
            {
                OnCrankComplete();
            }
        }
        private void OnCrankComplete()
        {
            Debug.Log("Crank Complete");
            if (outsideCrankCompleteable.IsComplete && insideCrankCompleteable.IsComplete)
            {
                (this as ICompletable).Complete();
            }
        }

        private void OnSunLockComplete()
        {
            outsideCrankCompleteable.Activate();
        }

        private void OnMoonLockComplete()
        {
            insideCrankCompleteable.Activate();
        }

        protected override UniTask Finish()
        {
            // Logic for async finish
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            // Logic for async load
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            // Logic for async unload
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Logic to execute after load
        }

        protected override void OnStart()
        {
            // Logic to execute on start
        }

        protected override void OnFinished()
        {
            // Logic to execute when sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic to execute on sequence unload
        }
    }
}
