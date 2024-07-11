using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sessions
{
    public abstract class ProgressionSession<S> : Session where S : Step
    {
        protected abstract ProgressionSessionData ProgressionSessionData { get; set; }
        public override SessionData SessionData
        {
            get => ProgressionSessionData;
            set => ProgressionSessionData = value as ProgressionSessionData;
        }
        [SerializeField] List<S> Steps = new List<S>();
        int currentStepIndex = 0;


        private void OnValidate()
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                Step step = Steps[i];
                if (step == null)
                {
                    Steps.RemoveAt(i);
                    continue;
                }
            }
        }


        void Awake()
        {
            foreach (var step in Steps)
            {
                step.ActivateContainer(false);
            }
        }

        public override void OnSessionEnd()
        {

        }

        public override void OnSessionStart()
        {
            foreach (var step in Steps)
            {
                step.ActivateContainer(true);
            }
            currentStepIndex = ProgressionSessionData.startingStep;
            if (ProgressionSessionData.playStepsOnStart)
            {
                StartStep(currentStepIndex);
            }
        }

        protected virtual UniTask OnEndStep(S step)
        {
            return UniTask.CompletedTask;
        }
        protected virtual UniTask OnIntroduceStep(S step)
        {
            return UniTask.CompletedTask;
        }
        protected virtual UniTask OnStartStep(S step)
        {
            return UniTask.CompletedTask;
        }



        S currentStep = null;
        bool stepTransitioning = false;
        async UniTask StartStep(int index)
        {
            //clean up previous step
            if (currentStep != null)
            {
                await OnEndStep(currentStep);
                currentStep.OnComplete.AddListener(OnStepComplete);
            }

            //start new step
            if (index < Steps.Count)
            {
                currentStep = Steps[currentStepIndex];

                await OnIntroduceStep(currentStep);
                await OnStartStep(currentStep);

                currentStep.OnComplete.AddListener(OnStepComplete);
            }
        }

        async void OnStepComplete()
        {
            currentStepIndex++;
            if (currentStepIndex < Steps.Count)
            {
                await StartStep(currentStepIndex);
            }
            else
            {
                Debug.Log("Session Complete");
            }
        }

        [Button]
        public void ForceCompleteCurrentStep()
        {
            (currentStep as ICompletable).Complete();
        }
    }


}
