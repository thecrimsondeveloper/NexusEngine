using System.Collections.Generic;
using Cysharp.Threading.Tasks;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseSequenceRunner : EntitySequence<BaseSequenceRunnerData>
    {
        private List<BaseSequenceDefinition> sequencesToRun = new List<BaseSequenceDefinition>();

#if ODIN_INSPECTOR
        [ShowInInspector, HideInEditorMode]
#endif
        private List<ISequence> runningSequences = new List<ISequence>();   


        protected override UniTask Initialize(BaseSequenceRunnerData currentData)
        {

            runningSequences.Clear();
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
                onBegin = OnSequenceBegin,
                onFinished = OnSequenceFinished,
                onUnload = OnSequenceUnload,
                onUnloaded = OnSequenceUnloaded
            });
        }

        private void OnSequenceBegin(ISequence sequence)
        {
            // Add any additional actions for sequence begin here if needed
            runningSequences.Add(sequence);

        }

        private void OnSequenceFinished(ISequence sequence)
        {
            // Check if all sequences are finished
            runningSequences.Remove(sequence);
        }

        private void OnSequenceUnload(ISequence sequence)
        {
            // Add any additional actions for sequence unload here if needed
            runningSequences.Remove(sequence);
        }
        private void OnSequenceUnloaded(ISequence sequence)
        {
        
            if (runningSequences.Count == 0)
            {
                Complete();
            }
        }

        private async void Complete()
        {
            // // Ensure we finish the runner itself after all sequences complete
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

    }

    [System.Serializable]
    public class BaseSequenceRunnerData : SequenceData
    {
        public List<BaseSequenceDefinition> sequencesToRun = new List<BaseSequenceDefinition>();
    }
}
