using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace LuminaryLabs.NexusEngine
{
    public class BaseMonoSequenceHandler : BaseSequence<BaseMonoSequenceHandlerData>
    {
        private RunnerSequenceDefinition monoSequenceDefinition;
        private bool waitForSequenceToFinish = true;       
        private ISequence runningSequence;

        protected override UniTask Initialize(BaseMonoSequenceHandlerData currentData)
        {
            // Set the MonoSequence from the provided data
            monoSequenceDefinition = currentData.monoSequence;
            waitForSequenceToFinish = currentData.waitForSequenceToFinish;
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (monoSequenceDefinition == null)
            {
                Debug.LogWarning("MonoSequence is missing.");
                Complete();
                return;
            }

            SequenceRunData defaultRunData = new SequenceRunData
            {
                superSequence = this,
                onBegin = (sequence) => {
                    OnMonoSequenceBegin(sequence);
                    ApplyModifiers(sequence, monoSequenceDefinition);
                },
                onFinished = OnMonoSequenceFinished,
            };

            if(monoSequenceDefinition.updateTranform)
            {
                defaultRunData.spawnPosition = monoSequenceDefinition.spawnPosition;
                defaultRunData.spawnRotation = Quaternion.Euler(monoSequenceDefinition.spawnRotation);
                defaultRunData.spawnSpace = monoSequenceDefinition.space;
            }

            // Run the provided MonoSequence
            var runResult = Sequence.Run(monoSequenceDefinition.sequenceToRun, defaultRunData);

            if (runResult.sequence == null || waitForSequenceToFinish == false)
            {
                await UniTask.NextFrame();
                Complete();
            }

        }

        private void ApplyModifiers(ISequence sequence, RunnerSequenceDefinition definition)
        {
            Debug.Log($"(RUNNER) Applying ({definition.baseSequenceDefinitions.Count}) modifiers to " + definition.sequenceToRun.name);
            foreach (var baseSequenceDefinition in definition.baseSequenceDefinitions)
            {
                Debug.Log("(RUNNER) Applying modifiers to " + definition.sequenceToRun.name);
                if (baseSequenceDefinition != null && baseSequenceDefinition.sequenceToRun != null)
                {
                    Sequence.Run(baseSequenceDefinition.sequenceToRun, new SequenceRunData
                    {
                        superSequence = sequence,
                        sequenceData = baseSequenceDefinition.sequenceData,
                    });
                }
            }
        }

        private void OnMonoSequenceBegin(ISequence sequence)
        {
            Debug.Log("MonoSequence began: " + sequence.name);

            // Store the running sequence
            runningSequence = sequence;
        }

        private async void OnMonoSequenceFinished(ISequence sequence)
        {
            Debug.Log("MonoSequence finished: " + sequence.name);
            await UniTask.NextFrame();
            Complete();
        }

        protected override async UniTask Unload()
        {

        }
    }

    [System.Serializable]
    public class BaseMonoSequenceHandlerData : BaseSequenceData
    {
        public bool waitForSequenceToFinish = true;
        [Tooltip("The MonoSequence to run")]
        public RunnerSequenceDefinition monoSequence;
    }
}
