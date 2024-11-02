using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class RunnerSequence : EntitySequence<RunnerSequenceData>
    {
        private bool destroyOnUnload = false;
        private int currentWaitForIndex = 0; // Index for tracking the current waitFor sequence

        #if ODIN_INSPECTOR
        [FoldoutGroup("Data"), ShowInInspector, HideInEditorMode]
        #endif
        private List<MonoSequence> beginWith, waitFor, finishWith, continueWith;

        #if ODIN_INSPECTOR
        [FoldoutGroup("Logic"), ShowInInspector, HideInEditorMode]
        #endif
        private List<ISequence> beginSequences = new(), 
                                waitForSequences = new(),
                                finishWithSequences = new();

        protected override UniTask Initialize(RunnerSequenceData currentData)
        {
            destroyOnUnload = currentData.destroyOnUnload;

            beginWith = new List<MonoSequence>(currentData.beginWith);
            finishWith = new List<MonoSequence>(currentData.finishWith);
            waitFor = new List<MonoSequence>(currentData.waitFor);
            continueWith = new List<MonoSequence>(currentData.continueWith);

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            // Start sequences specified in beginWith
            foreach (MonoSequence sequence in beginWith)
            {
                Sequence.Run(sequence, new SequenceRunData
                {
                    superSequence = this,
                    onBegin = OnBeginSequenceBegin,
                    onUnload = OnWaitSequenceUnload,
                });
            }

            // Start the first waitFor sequence
            currentWaitForIndex = 0;
            if (waitFor.Count > 0)
            {
                RunWaitForSequence();
            }
        }


        void RunWaitForSequence(int index = 0)
        {
             // Run the current waitFor sequence based on the index
            MonoSequence currentWaitForSequence = waitFor[index];

            if(currentWaitForSequence is RunnerSequence runner)
            {
                Nexus.Log(" Pre Running Sub Runner: " + runner.name);
            }

            Nexus.Log("Pre Run" + currentWaitForSequence.name);
            Sequence.Run(currentWaitForSequence, new SequenceRunData
            {
                superSequence = this,
                onBegin = OnWaitSequenceBegin,
                onFinished = OnWaitSequenceFinished,
                onUnload = OnWaitSequenceUnload
            });
        }

        
        private void OnWaitSequenceFinished(ISequence sequence)
        {
            Nexus.Log("Wait Sequence Finished and was part of the wait for list: " + sequence.name);
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

        private void OnBeginSequenceBegin(ISequence sequence)
        {
            beginSequences.Add(sequence);
        }

        private void OnWaitSequenceBegin(ISequence sequence)
        {
            Debug.Log("WAIT SEQUENCE BEGIN: " + sequence.name);
            waitForSequences.Add(sequence);
        }

        private void OnWaitSequenceUnload(ISequence sequence)
        {
            Debug.Log("WAIT SEQUENCE UNLOAD: " + sequence.GetType());
            waitForSequences.Remove(sequence);
        }
        private void OnFinishSequenceBegin(ISequence sequence)
        {
            Debug.Log("FINISHED SEQUENCE BEGIN: " + sequence.GetType());
            finishWithSequences.Add(sequence);
        }
        private void OnFinishSequenceUnload(ISequence sequence)
        {
            Debug.Log("FINISHED" + sequence.GetType());
            finishWithSequences.Remove(sequence);
        }

        private void StartContinueWithSequences()
        {

            Nexus.Log("Continuing With " + continueWith.Count + " sequences on " + name);
            foreach (MonoSequence sequence in continueWith)
            {
                Nexus.Log("Continuing With " + sequence.name);
                Sequence.Run(sequence, new SequenceRunData 
                {
                    superSequence = sequence.superSequence,
                });
            }
        }

       private async void Complete()
        {
            // Ensure we finish the sequence first
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override async UniTask Finish()
        {
            // Trigger finishing sequences first
            foreach (MonoSequence sequence in finishWith)
            {
                Debug.Log("Finishing With " + sequence.name);
                Sequence.Run(sequence, new SequenceRunData
                {
                    superSequence = this,
                    onBegin = OnFinishSequenceBegin,
                    onUnload = OnFinishSequenceUnload,
                });
            }

            await UniTask.NextFrame();
        }

        protected override async UniTask Unload()
        {

             Nexus.Log("Complete " + name);
            while(beginSequences.Count > 0)
            {
                ISequence sequenceToStop = beginSequences[0];
                if(sequenceToStop == null) continue;

                await Sequence.Stop(sequenceToStop);

                if(beginSequences.Contains(sequenceToStop))
                {
                    beginSequences.Remove(sequenceToStop);
                }
            }

            while(waitForSequences.Count > 0)
            {
                ISequence sequenceToStop = waitForSequences[0];
                if(sequenceToStop == null) continue;


                await Sequence.Stop(sequenceToStop);

                if(waitForSequences.Contains(sequenceToStop))
                {
                    waitForSequences.Remove(sequenceToStop);
                }
            }

            await UniTask.NextFrame();

            while(finishWithSequences.Count > 0)
            {
                ISequence sequenceToStop = finishWithSequences[0];
                if(sequenceToStop == null) continue;


                await Sequence.Stop(sequenceToStop);

                if(finishWithSequences.Contains(sequenceToStop))
                {
                    finishWithSequences.Remove(sequenceToStop);
                }
            }
            
        }

        protected override void OnUnloaded()
        {
            Nexus.Log("Runner Unloaded: " + name);
            StartContinueWithSequences();
            if (destroyOnUnload)
            {
                Destroy(gameObject);
            }

            base.OnUnloaded();
        }
    }

    [System.Serializable]
    public class RunnerSequenceData : SequenceData
    {
        public bool destroyOnUnload = false;

        #if ODIN_INSPECTOR
        [BoxGroup("beginWith", false)]
        #endif
        public List<MonoSequence> beginWith = new List<MonoSequence>();

        #if ODIN_INSPECTOR
        [BoxGroup("waitFor", false)]
        #endif
        public List<MonoSequence> waitFor = new List<MonoSequence>();

        #if ODIN_INSPECTOR
        [BoxGroup("finishWith", false)]
        #endif
        public List<MonoSequence> finishWith = new List<MonoSequence>();

        #if ODIN_INSPECTOR
        [BoxGroup("continueWith", false)]
        #endif
        public List<MonoSequence> continueWith = new List<MonoSequence>();
    }
}
