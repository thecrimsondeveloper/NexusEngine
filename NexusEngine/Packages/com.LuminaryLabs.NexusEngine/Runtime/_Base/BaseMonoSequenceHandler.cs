using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace LuminaryLabs.NexusEngine
{
    public class BaseMonoSequenceHandler : BaseSequence<BaseMonoSequenceHandlerData>
    {
        private MonoSequence monoSequence;

        private ISequence runningSequence;

        protected override UniTask Initialize(BaseMonoSequenceHandlerData currentData)
        {
            // Set the MonoSequence from the provided data
            monoSequence = currentData.monoSequence;
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (monoSequence == null)
            {
                Debug.LogWarning("MonoSequence is missing.");
                Complete();
                return;
            }

            // Run the provided MonoSequence
            var runResult = Sequence.Run(monoSequence, new SequenceRunData
            {
                superSequence = this,
                onBegin = OnMonoSequenceBegin,
                onFinished = OnMonoSequenceFinished,
            });

            if (runResult.sequence == null)
            {
                Debug.LogWarning("Failed to run MonoSequence.");
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
            if (monoSequence != null && monoSequence.phase == Phase.Run)
            {
                await Sequence.Stop(monoSequence);
            }
        }
    }

    [System.Serializable]
    public class BaseMonoSequenceHandlerData : BaseSequenceData
    {
        [Tooltip("The MonoSequence to run")]
        public MonoSequence monoSequence;
    }
}
