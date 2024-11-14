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
        private bool unloadRunningSequenceOnUnload = false;

        private ISequence runningSequence;

        protected override UniTask Initialize(BaseMonoSequenceHandlerData currentData)
        {
            // Set the MonoSequence from the provided data
            monoSequenceDefinition = currentData.monoSequence;
            waitForSequenceToFinish = currentData.waitForSequenceToFinish;
            unloadRunningSequenceOnUnload = currentData.unloadRunningSequenceOnUnload;
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
                parent = monoSequenceDefinition.sequenceParent,
                onBegin = (sequence) => {
                    OnMonoSequenceBegin(sequence);
                    RunnerSequenceDefinition.ApplyModifiers(sequence, monoSequenceDefinition);
                },
                onFinished = OnMonoSequenceFinished,
                setupInHeirarchy = monoSequenceDefinition.setupSequenceInHeirarchy
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
            if (unloadRunningSequenceOnUnload && runningSequence != null)
            {
                await Sequence.Stop(runningSequence);
            }
        }
    }

    [System.Serializable]
    public class BaseMonoSequenceHandlerData : BaseSequenceData
    {
        public bool waitForSequenceToFinish = true;
        public bool unloadRunningSequenceOnUnload = false;
        [Tooltip("The MonoSequence to run")]
        public RunnerSequenceDefinition monoSequence;
    }
}
