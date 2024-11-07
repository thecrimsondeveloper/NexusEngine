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
            Nexus.Log("Running begin sequences in a NexusSequence there are " + beginWith.Count + " sequences to run");
            foreach (var definition in beginWith)
            {
                RunNexusSequence(definition, OnBeginSequenceCallback, OnBeginSequenceUnload);
            }
        }

        private void RunNexusSequence(BaseSequenceDefinition definition, UnityAction<ISequence> onBeginCallback, UnityAction<ISequence> onUnloadCallback, UnityAction<ISequence> onFinishedCallback = null)
        {
            Nexus.Log("Running NexusSequence: " + ((ISequence)definition.sequenceToRun).name);

            // Run the main NexusSequence
            Sequence.Run(definition.sequenceToRun, new SequenceRunData
            {
                superSequence = this,
                sequenceData = definition.sequenceData,
                onBegin = onBeginCallback,
                onUnload = onUnloadCallback,
                onFinished = onFinishedCallback
            });
        }

         void RunWaitForSequence(int index = 0)
        {
             // Run the current waitFor sequence based on the index


            BaseSequenceDefinition definition = waitFor[index];
            RunNexusSequence(definition, OnWaitForSequenceCallback, OnWaitForSequenceUnload, OnWaitForSequenceFinished);
        }

        private void RunFinishSequences()
        {
            foreach (var definition in finishWith)
            {
                Nexus.Log("Running finish sequence: " + ((ISequence)definition.sequenceToRun).name);
                RunNexusSequence(definition, OnFinishSequenceCallback, OnFinishSequenceUnload);
            }
        }



        private void OnBeginSequenceCallback(ISequence sequence)
        {
            Nexus.Log("Nexus Begin sequence started: " + sequence.name);
            beginSequences.Add(sequence);
        }

        private void OnBeginSequenceUnload(ISequence sequence)
        {
            Nexus.Log("Nexus Begin sequence unloaded: " + sequence.name);
            beginSequences.Remove(sequence);
        }

        private void OnWaitForSequenceCallback(ISequence sequence)
        {
            Nexus.Log("Nexus WaitFor sequence started: " + sequence.name);
            waitForSequences.Add(sequence);
        }

        private void OnWaitForSequenceFinished(ISequence sequence)
        {
            Nexus.Log("Nexus WaitFor sequence finished: " + sequence.name);
            RunNextWaitForSequence();
        }
         private void RunNextWaitForSequence()
        {
            // Increment the index and run the next sequence
            currentWaitForIndex++;
            if (currentWaitForIndex >= waitFor.Count)
            {
                Complete();
                return;
            }

            RunWaitForSequence(currentWaitForIndex);
        }


        private void OnWaitForSequenceUnload(ISequence sequence)
        {
            Nexus.Log("Nexus WaitFor sequence unloaded: " + sequence.name);
            waitForSequences.Remove(sequence);
        }

        private void OnFinishSequenceCallback(ISequence sequence)
        {
            Nexus.Log("Nexus Finish sequence started: " + sequence.name);
            finishWithSequences.Add(sequence);
        }

        private void OnFinishSequenceUnload(ISequence sequence)
        {
            Nexus.Log("Nexus Finish sequence unloaded: " + sequence.name);
            finishWithSequences.Remove(sequence);
        }
        protected override async UniTask Unload()
        {
            await StopSequences(beginSequences);
            await StopSequences(waitForSequences);
        }
        protected override async UniTask Finish()
        {
            Nexus.Log("Finishing NexusSequence: " + ((ISequence)this).name);
            RunFinishSequences();
            await UniTask.NextFrame();
            // await UniTask.NextFrame();
            // await UniTask.Delay(50);
        }



        protected override async void OnUnloaded()
        {
            await UniTask.NextFrame();
            StopSequences(finishWithSequences);
        }

        private async UniTask StopSequences(List<ISequence> sequences)
        {
            while(sequences.Count > 0)
            {
                ISequence sequenceToStop = sequences[0];
                if(sequenceToStop == null) continue;

                await Sequence.Stop(sequenceToStop);

                if(sequences.Contains(sequenceToStop))
                {
                    sequences.Remove(sequenceToStop);
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
