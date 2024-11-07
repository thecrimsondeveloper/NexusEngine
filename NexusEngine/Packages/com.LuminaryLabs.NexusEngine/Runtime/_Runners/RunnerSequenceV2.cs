using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
    public class RunnerSequenceV2 : EntitySequence<RunnerSequenceV2Data>
    {
        private bool destroyOnUnload = false;
        private int currentWaitForIndex = 0; // Index for tracking the current waitFor sequence

        #if ODIN_INSPECTOR
        [FoldoutGroup("Data"), ShowInInspector, HideInEditorMode]
        #endif
        private List<RunnerSequenceDefinition> beginWith, waitFor, finishWith, continueWith;

           private List<ISequence> beginSequences = new(), 
                                waitForSequences = new(),
                                finishWithSequences = new();

        protected override UniTask Initialize(RunnerSequenceV2Data currentData)
        {
            destroyOnUnload = currentData.destroyOnUnload;

            beginWith = new List<RunnerSequenceDefinition>(currentData.beginWith);
            finishWith = new List<RunnerSequenceDefinition>(currentData.finishWith);
            waitFor = new List<RunnerSequenceDefinition>(currentData.waitFor);
            continueWith = new List<RunnerSequenceDefinition>(currentData.continueWith);

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
                RunSequenceWithModifiers(definition, OnBeginSequenceCallback, OnBeginSequenceUnload);
            }
        }

        private void RunSequenceWithModifiers(RunnerSequenceDefinition definition, UnityAction<ISequence> onBeginCallback, UnityAction<ISequence> onUnloadCallback)
        {


            // Run the main sequence
            Sequence.Run(definition.sequenceToRun, new SequenceRunData
            {
                superSequence = this,
                onBegin = (sequence) => {
                    onBeginCallback(sequence);
                    ApplyModifiers(definition);
                },
                onUnload = onUnloadCallback,
            });
        }

        private void ApplyModifiers(RunnerSequenceDefinition definition)
        {
            foreach (var modifierDefinition in definition.sequenceModifierDefinitions)
            {
                if (modifierDefinition != null && modifierDefinition.sequenceModifier != null)
                {
                    Sequence.Run(modifierDefinition.sequenceModifier, new SequenceRunData
                    {
                        superSequence = this,
                        sequenceData = modifierDefinition.sequenceModifierData,
                    });
                }
            }
        }

        private void RunWaitForSequence()
        {
            if (currentWaitForIndex >= waitFor.Count)
            {
                Complete();
                return;
            }

            var definition = waitFor[currentWaitForIndex];
            RunSequenceWithModifiers(definition, OnWaitForSequenceCallback, OnWaitForSequenceUnload);
        }

        private void RunFinishSequences()
        {
            foreach (var definition in finishWith)
            {
                RunSequenceWithModifiers(definition, OnFinishSequenceCallback, OnFinishSequenceUnload);
            }
        }

        private void RunContinueWithSequences()
        {
            foreach (var definition in continueWith)
            {
                RunSequenceWithModifiers(definition, OnContinueWithSequenceCallback, OnContinueWithSequenceUnload);
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

            if (beginSequences.Contains(sequence))
            {
                beginSequences.Remove(sequence);
            }
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

        private void OnContinueWithSequenceCallback(ISequence sequence)
        {
            Debug.Log("ContinueWith sequence started: " + sequence.name);
        }

        private void OnContinueWithSequenceUnload(ISequence sequence)
        {
            Debug.Log("ContinueWith sequence unloaded: " + sequence.name);
        }

        private async void Complete()
        {
            RunFinishSequences();
            await UniTask.NextFrame();
            RunContinueWithSequences();

            if (destroyOnUnload)
            {
                Destroy(gameObject);
            }
        }

        protected override async UniTask Unload()
        {
            // Stop all beginWith, waitFor, and finishWith sequences
            await StopSequences(beginWith);
            await StopSequences(waitFor);
            await StopSequences(finishWith);
        }

        private async UniTask StopSequences(List<RunnerSequenceDefinition> sequences)
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
    public class RunnerSequenceV2Data : SequenceData
    {
        public bool destroyOnUnload = false;

        #if ODIN_INSPECTOR
        [BoxGroup("beginWith", false)]
        #endif
        public List<RunnerSequenceDefinition> beginWith = new List<RunnerSequenceDefinition>();

        #if ODIN_INSPECTOR
        [BoxGroup("waitFor", false)]
        #endif
        public List<RunnerSequenceDefinition> waitFor = new List<RunnerSequenceDefinition>();

        #if ODIN_INSPECTOR
        [BoxGroup("finishWith", false)]
        #endif
        public List<RunnerSequenceDefinition> finishWith = new List<RunnerSequenceDefinition>();

        #if ODIN_INSPECTOR
        [BoxGroup("continueWith", false)]
        #endif
        public List<RunnerSequenceDefinition> continueWith = new List<RunnerSequenceDefinition>();
    }

    [System.Serializable]
    public class SequenceModifierDefinition
    {
        [SerializeReference]
        public SequenceModifier sequenceModifier;
        [SerializeReference]
        public SequenceModifierData sequenceModifierData;
    }

    [System.Serializable]
    public class RunnerSequenceDefinition
    {
        public MonoSequence sequenceToRun; // Made public to allow referencing in the inspector
        public List<SequenceModifierDefinition> sequenceModifierDefinitions = new List<SequenceModifierDefinition>();
    }
}
