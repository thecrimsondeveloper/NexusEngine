using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class RepeatSequenceMonoHandler : EntitySequence<RepeatSequenceMonoHandler.RepeatSequenceData>
    {
        // Internal fields
        private int executedCount = 0;
        private List<BaseSequenceDefinition> waitForSequences = new List<BaseSequenceDefinition>();

        List<ISequence> sequencesToWaitFor = new List<ISequence>();

        protected override UniTask Initialize(RepeatSequenceData currentData)
        {
            // Assign data to internal variables
            waitForSequences = currentData.waitForSequences;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Reset counters
            executedCount = 0;

            RunWaitForSequences();
        }


        async void RunWaitForSequences()
        {
            
            foreach (var definition in waitForSequences)
            {
                Sequence.Run(definition.sequenceToRun, new SequenceRunData
                {
                    superSequence = this,
                    sequenceData = definition.sequenceData,
                    onBegin = OnWaitForSequenceBegin,
                    onFinished = OnWaitForSequenceFinished
                });
            }
        }

        private void OnWaitForSequenceBegin(ISequence sequence)
        {
            Nexus.Log("Repeate Sequence started: " + sequence.name);
            sequencesToWaitFor.Add(sequence);
        }

        private async void OnWaitForSequenceFinished(ISequence sequence)
        {
            Nexus.Log("Repeate Sequence finished: " + sequence.name);
            if (sequencesToWaitFor.Contains(sequence))
            {
                sequencesToWaitFor.Remove(sequence);
            }

            if (sequencesToWaitFor.Count == 0)
            {
                await UniTask.NextFrame();
                Nexus.Log("All sequences finished. Running WaitSequences Again.");
                RunWaitForSequences();
            }
        }


        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        [System.Serializable]
        public class RepeatSequenceData : SequenceData
        {

            [Tooltip("Sequences to wait for before running the main sequence.")]
            public List<BaseSequenceDefinition> waitForSequences;

            [Tooltip("Number of times to repeat the sequence. Use -1 for infinite.")]
            public int repeatCount = -1;

        }
    }
}
