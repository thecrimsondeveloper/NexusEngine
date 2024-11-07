using System.Collections.Generic;
using Cysharp.Threading.Tasks;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
    [System.Serializable]
    public class NexusSequence : BaseSequence<NexusSequenceData>
    {
        private int currentWaitForIndex = 0; // Index for tracking the current waitFor sequence

        #if ODIN_INSPECTOR
        [FoldoutGroup("Data"), ShowInInspector, HideInEditorMode]
        #endif
        private List<BaseSequenceDefinition> beginWith, waitFor, finishWith;

        
        private List<ISequence> beginSequences = new(), 
                            waitForSequences = new(),
                            finishWithSequences = new();

        protected override UniTask Initialize(NexusSequenceData currentData)
        {
            beginWith = new List<BaseSequenceDefinition>(currentData.beginWith);
            finishWith = new List<BaseSequenceDefinition>(currentData.finishWith);
            waitFor = new List<BaseSequenceDefinition>(currentData.waitFor);

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            RunBeginSequences();
            currentWaitForIndex = 0;

            if (waitFor.Count > 0)
            {
                RunWaitForSequence();
            }
        }

        private void RunBeginSequences()
        {
            foreach (var definition in beginWith)
            {
                RunNexusSequence(definition, OnBeginSequenceCallback, OnBeginSequenceUnload);
            }
        }

        private void RunNexusSequence(BaseSequenceDefinition definition, UnityAction<ISequence> onBeginCallback, UnityAction<ISequence> onUnloadCallback)
        {
            // Run the main NexusSequence
            Sequence.Run(definition.sequenceToRun, new SequenceRunData
            {
                superSequence = this,
                sequenceData = definition.sequenceData,
                onBegin = onBeginCallback,
                onUnload = onUnloadCallback,
            });
        }

        private void RunWaitForSequence()
        {
            if (currentWaitForIndex >= waitFor.Count)
            {
                Complete();
                return;
            }

            var definition = waitFor[currentWaitForIndex];
            RunNexusSequence(definition, OnWaitForSequenceCallback, OnWaitForSequenceUnload);
        }

        private void RunFinishSequences()
        {
            foreach (var definition in finishWith)
            {
                RunNexusSequence(definition, OnFinishSequenceCallback, OnFinishSequenceUnload);
            }
        }



        private void OnBeginSequenceCallback(ISequence sequence)
        {
            Debug.Log("Begin sequence started: " + sequence.name);
            beginSequences.Add(sequence);
        }

        private void OnBeginSequenceUnload(ISequence sequence)
        {
            Debug.Log("Begin sequence unloaded: " + sequence.name);
            beginSequences.Remove(sequence);
        }

        private void OnWaitForSequenceCallback(ISequence sequence)
        {
            Debug.Log("WaitFor sequence started: " + sequence.name);

            waitForSequences.Add(sequence);
        }

        private void OnWaitForSequenceFinished(ISequence sequence)
        {
            Debug.Log("WaitFor sequence finished: " + sequence.name);
            currentWaitForIndex++;
            RunWaitForSequence();
        }

        private void OnWaitForSequenceUnload(ISequence sequence)
        {
            Debug.Log("WaitFor sequence unloaded: " + sequence.name);
            waitForSequences.Remove(sequence);
        }

        private void OnFinishSequenceCallback(ISequence sequence)
        {
            Debug.Log("Finish sequence started: " + sequence.name);
            finishWithSequences.Add(sequence);
        }

        private void OnFinishSequenceUnload(ISequence sequence)
        {
            Debug.Log("Finish sequence unloaded: " + sequence.name);
            finishWithSequences.Remove(sequence);
        }
        private async void Complete()
        {
            RunFinishSequences();
            await UniTask.NextFrame();

            await Sequence.Finish(this);
            await Sequence.Stop(this);

        }

        protected override async UniTask Unload()
        {
            await StopSequences(beginWith);
            await StopSequences(waitFor);
            await StopSequences(finishWith);
        }

        private async UniTask StopSequences(List<BaseSequenceDefinition> sequences)
        {
            foreach (var definition in sequences)
            {
                var sequenceToStop = definition.sequenceToRun;
                if (sequenceToStop != null)
                {
                    await Sequence.Stop(sequenceToStop);
                }
            }
        }
    }

    [System.Serializable]
    public class NexusSequenceData : BaseSequenceData
    {
        #if ODIN_INSPECTOR
        [BoxGroup("beginWith", false)]
        #endif
        public List<BaseSequenceDefinition> beginWith = new List<BaseSequenceDefinition>();

        #if ODIN_INSPECTOR
        [BoxGroup("waitFor", false)]
        #endif
        public List<BaseSequenceDefinition> waitFor = new List<BaseSequenceDefinition>();

        #if ODIN_INSPECTOR
        [BoxGroup("finishWith", false)]
        #endif
        public List<BaseSequenceDefinition> finishWith = new List<BaseSequenceDefinition>();

    }


}
