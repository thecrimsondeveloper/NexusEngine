using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox
{
    public class CosmicCrafterTutorial : MonoBehaviour
    {
        [SerializeField] List<CosmicTutorialStep> tutorialSteps = new List<CosmicTutorialStep>();

        int stepIndex = 0;
        [SerializeField, ReadOnly] CosmicTutorialStep currentStep;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            tutorialSteps.ForEach(step => step.gameObject.SetActive(false));
        }

        public void StartTutorial()
        {
            //start the first tutorial step
            currentStep = tutorialSteps[stepIndex];
            currentStep.Activate();
            currentStep.OnComplete.AddListener(OnStepComplete);
        }

        CosmicTutorialStep ProceedToNextStep()
        {


            //increment the step index
            stepIndex++;

            stepIndex = Mathf.Clamp(stepIndex, 0, tutorialSteps.Count - 1);
            currentStep = tutorialSteps[stepIndex];

            currentStep.Activate();
            currentStep.OnComplete.AddListener(OnStepComplete);
            return currentStep;
        }

        async void OnStepComplete()
        {
            //deactivate the current step
            await currentStep.Deactivate();
            currentStep.OnComplete.RemoveListener(OnStepComplete);

            //check if there are more steps
            if (tutorialSteps.Count > 0)
            {
                //start the next step
                ProceedToNextStep();
            }
            else
            {
                //end the tutorial
                EndTutorial();
            }
        }

        void EndTutorial()
        {
            //end the tutorial
        }



    }
}
