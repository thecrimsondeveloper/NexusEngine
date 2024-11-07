using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class NexusSequenceRunner : EntitySequence<NexusSequenceRunnerData>
    {
        private List<BaseSequenceDefinition> sequencesToRun = new List<BaseSequenceDefinition>();

        protected override UniTask Initialize(NexusSequenceRunnerData currentData)
        {
            // Populate the sequencesToRun list with the data provided
            sequencesToRun = new List<BaseSequenceDefinition>(currentData.sequencesToRun);
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Run all NexusSequenceDefinitions in parallel
            foreach (var definition in sequencesToRun)
            {
                RunSequence(definition);
            }
        }

        private void RunSequence(BaseSequenceDefinition definition)
        {
            if (definition.sequenceToRun == null)
            {
                Debug.LogWarning("Attempted to run a null sequence.");
                return;
            }

            // Run the sequence
            Sequence.Run(definition.sequenceToRun, new SequenceRunData
            {
                superSequence = this,
                sequenceData = definition.sequenceData,
                onBegin = (sequence) => OnSequenceBegin(sequence),
                onFinished = (sequence) => OnSequenceFinished(sequence),
                onUnload = (sequence) => OnSequenceUnload(sequence)
            });
        }

        private void OnSequenceBegin(ISequence sequence)
        {
            // Add any additional actions for sequence begin here if needed
        }

        private void OnSequenceFinished(ISequence sequence)
        {
            // Check if all sequences are finished
            sequencesToRun.RemoveAll(def => def.sequenceToRun == sequence);

            if (sequencesToRun.Count == 0)
            {
                Complete();
            }
        }

        private void OnSequenceUnload(ISequence sequence)
        {
            sequencesToRun.RemoveAll(def => def.sequenceToRun == sequence);
        }

        private async void Complete()
        {
            // Ensure we finish the runner itself after all sequences complete
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override async UniTask Unload()
        {
            // Stop all sequences when unloading
            while (sequencesToRun.Count > 0)
            {
                var sequenceToStop = sequencesToRun[0].sequenceToRun;
                if (sequenceToStop != null)
                {
                    await Sequence.Stop(sequenceToStop);
                }
                sequencesToRun.RemoveAt(0);
            }
        }

        protected override void OnUnloaded()
        {
            Destroy(gameObject);
            base.OnUnloaded();
        }
    }

    [System.Serializable]
    public class NexusSequenceRunnerData : SequenceData
    {
        public List<BaseSequenceDefinition> sequencesToRun = new List<BaseSequenceDefinition>();
    }
}
