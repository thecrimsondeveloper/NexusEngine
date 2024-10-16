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
                RunNextWaitForSequence();
            }
        }

        private void RunNextWaitForSequence()
        {
            if (currentWaitForIndex >= waitFor.Count)
            {
                Complete();
                return;
            }

            // Run the current waitFor sequence based on the index
            MonoSequence currentWaitForSequence = waitFor[currentWaitForIndex];
            Sequence.Run(currentWaitForSequence, new SequenceRunData
            {
                superSequence = this,
                onBegin = OnWaitSequenceBegin,
                onFinished = OnWaitSequenceFinished,
                onUnload = OnWaitSequenceUnload
            });
        }

        private void OnBeginSequenceBegin(ISequence sequence)
        {
            beginSequences.Add(sequence);
        }

        private void OnWaitSequenceBegin(ISequence sequence)
        {
            Debug.Log("WAIT SEQUENCE BEGIN: " + sequence.GetType());
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

        private void OnWaitSequenceFinished(ISequence sequence)
        {
            waitForSequences.Remove(sequence);

            // Increment the index and run the next sequence
            currentWaitForIndex++;
            RunNextWaitForSequence();
        }

        private void StartContinueWithSequences()
        {
            foreach (MonoSequence sequence in continueWith)
            {
                Debug.Log("Continuing With " + sequence.name);
                Sequence.Run(sequence, new SequenceRunData {});
            }
        }

        private async void Complete()
        {
            // Stop any remaining sequences
            foreach (ISequence sequence in beginSequences)
            {
                await Sequence.Stop(sequence);
            }

            foreach (ISequence sequence in waitForSequences)
            {
                await Sequence.Stop(sequence);
            }

            
            await Sequence.Finish(this);
        }

        protected override async UniTask Finish()
        {
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

            await Sequence.Stop(this);
        }

        protected override async UniTask Unload()
        {
            for(int i = 0; i< finishWithSequences.Count - 1; i++)
            {
                ISequence sequence = finishWithSequences[i];
                await Sequence.Stop(sequence);
            }
        }

        protected override void OnUnloaded()
        {
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
