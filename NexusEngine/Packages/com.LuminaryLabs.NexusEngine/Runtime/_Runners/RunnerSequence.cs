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
    public class RunnerSequence : EntitySequence<RunnerSequenceV2Data>
    {
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

        private void RunSequenceWithModifiers(RunnerSequenceDefinition definition, UnityAction<ISequence> onBeginCallback, UnityAction<ISequence> onUnloadCallback, UnityAction<ISequence> onFinishedCallback = null)
        {

            Debug.Log("(RUNNER) Running sequence with modifiers: " + definition.sequenceToRun.name);

            SequenceRunData defaultRunData = new SequenceRunData
            {
                superSequence = this,
                parent = definition.sequenceParent,
                onBegin = (sequence) =>
                {
                    onBeginCallback?.Invoke(sequence);
                    ApplyModifiers(sequence, definition);
                },
                onUnload = onUnloadCallback,
                onFinished = onFinishedCallback
            };

            if(definition.updateTranform)
            {
                defaultRunData.spawnPosition = definition.spawnPosition;
                defaultRunData.spawnRotation = Quaternion.Euler(definition.spawnRotation);
                defaultRunData.spawnSpace = definition.space;
            }


            // Run the main sequence
            Sequence.Run(definition.sequenceToRun, defaultRunData);
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
            RunNextWaitForSequence();
        }



        private void RunNextWaitForSequence()
        {
            // Increment the index and run the next sequence
            currentWaitForIndex++;

            Nexus.Log("Running Next Wait For Sequence on " + name);
            if (currentWaitForIndex >= waitFor.Count)
            {
                Nexus.Log("Auto Complete Runner Sequence" + name);
                Complete();
                return;
            }

            RunWaitForSequence(currentWaitForIndex);
        }

        
        void RunWaitForSequence(int index = 0)
        {
             // Run the current waitFor sequence based on the index
            RunnerSequenceDefinition definition = waitFor[index];
            RunSequenceWithModifiers(definition, OnWaitForSequenceCallback, OnWaitForSequenceUnload, OnWaitForSequenceFinished);
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
            // Ensure we finish the sequence first
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override async UniTask Finish()
        {
            Nexus.Log("Finishing NexusSequence: " + ((ISequence)this).name);
            RunFinishSequences();
            await UniTask.NextFrame();
            // await UniTask.NextFrame();
            // await UniTask.Delay(50);
        }


        protected override async UniTask Unload()
        {
            // Stop all beginWith, waitFor, and finishWith sequences
            await StopSequences(beginSequences);
            await StopSequences(waitForSequences);
        }

         protected async override void OnUnloaded()
        {
            Nexus.Log("Runner Unloaded: " + name);
            StartContinueWithSequences();

            await UniTask.NextFrame();
            StopSequences(finishWithSequences);

            base.OnUnloaded();
        }

         private void StartContinueWithSequences()
        {

            Nexus.Log("Continuing With " + continueWith.Count + " sequences on " + name);
            foreach (RunnerSequenceDefinition sequenceDefinition in continueWith)
            {
                RunSequenceWithModifiers(sequenceDefinition, OnContinueWithSequenceCallback, OnContinueWithSequenceUnload);
            }
        }

        private async UniTask StopSequences(List<ISequence> sequences)
        {
            while (sequences.Count > 0)
            {
                ISequence sequenceToStop = sequences[0];

                // If the sequence is null, remove it from the list and continue
                if (sequenceToStop == null)
                {
                    sequences.RemoveAt(0);
                    continue;
                }

                await Sequence.Stop(sequenceToStop);

                // Remove the sequence after stopping it
                if (sequences.Contains(sequenceToStop))
                {
                    sequences.Remove(sequenceToStop);
                }
            }
        }

    }

    [System.Serializable]
    public class RunnerSequenceV2Data : SequenceData
    {

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
        

        #if ODIN_INSPECTOR
        [FoldoutGroup("Modifiers")]
        #endif
        public Transform sequenceParent;

        #if ODIN_INSPECTOR
        [FoldoutGroup("Modifiers")]
        #endif
        public bool updateTranform = false;
        
        #if ODIN_INSPECTOR
        [FoldoutGroup("Modifiers")]
        #endif
        public Vector3 spawnPosition;

        #if ODIN_INSPECTOR
        [FoldoutGroup("Modifiers")]
        #endif
        public Vector3 spawnRotation;

        #if ODIN_INSPECTOR
        [FoldoutGroup("Modifiers")]
        #endif
        public Space space = Space.Self;

        #if ODIN_INSPECTOR
        [FoldoutGroup("Modifiers")]
        #endif
        public List<BaseSequenceDefinition> baseSequenceDefinitions = new List<BaseSequenceDefinition>();
    }
}
