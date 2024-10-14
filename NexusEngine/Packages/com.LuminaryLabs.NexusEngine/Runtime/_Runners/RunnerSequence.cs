using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class RunnerSequence : EntitySequence<RunnerSequenceData>
    {
        private bool destroyOnUnload = false;
        private List<MonoSequence> beginWith = new List<MonoSequence>();
        private List<MonoSequence> waitFor = new List<MonoSequence>();
        private List<MonoSequence> finishWith = new List<MonoSequence>();
        private List<MonoSequence> continueWith = new List<MonoSequence>();

        private List<ISequence> waitForSequences = new List<ISequence>();

        private List<ISequence> beginSequences = new List<ISequence>();

        private List<ISequence> continueSequences = new List<ISequence>();

        protected override UniTask Initialize(RunnerSequenceData currentData)
        {
            destroyOnUnload = currentData.destroyOnUnload;
            beginWith = currentData.beginWith;
            finishWith = currentData.finishWith;
            waitFor = currentData.waitFor;
            continueWith = currentData.continueWith; // Initialize the new continueWith list
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Start sequences specified in beginWith
            foreach (MonoSequence sequence in beginWith)
            {
                Sequence.Run(sequence, new SequenceRunData
                {
                    superSequence = this,
                    onBegin = OnBeginSequenceBegin
                });
            }

            // Start sequences specified in waitFor and keep track of them
            waitForSequences.Clear();
            foreach (MonoSequence sequence in waitFor)
            {
                Sequence.Run(sequence, new SequenceRunData
                {
                    superSequence = this,
                    onBegin = OnWaitSequenceBegin,
                    onFinished = OnWaitSequenceFinished
                });
            }
        }

        private void OnBeginSequenceBegin(ISequence sequence)
        {
            beginSequences.Add(sequence);
        }

        private void OnWaitSequenceBegin(ISequence sequence)
        {
            waitForSequences.Add(sequence);
        }

        private void OnWaitSequenceFinished(ISequence sequence)
        {
            Debug.Log("WaitFor sequence finished: " + sequence.GetType());
            waitForSequences.Remove(sequence);

            // Check if all waitFor sequences are finished
            CheckIfWaitForComplete();
        }

        private void CheckIfWaitForComplete()
        {
            if (waitForSequences.Count == 0)
            {
                // Start continueWith sequences
                Complete();
            }
        }

        private void StartContinueWithSequences()
        {
            foreach (MonoSequence sequence in continueWith)
            {
                Debug.Log("Continueing With " + sequence.name);
                Sequence.Run(sequence, new SequenceRunData
                {
                });
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
                Debug.Log("Continueing With " + sequence.name);
                Sequence.Run(sequence, new SequenceRunData
                {
                    // superSequence = this,
                });
            }

            await UniTask.NextFrame();
            Sequence.Stop(this);
        }



        protected override UniTask Unload()
        {
            if(destroyOnUnload)
            {
                Destroy(gameObject);
            }
            return UniTask.CompletedTask;
        }

        protected override void OnUnloaded()
        {
            StartContinueWithSequences();
            base.OnUnloaded();
        }
    }

    [System.Serializable]
    public class RunnerSequenceData : SequenceData
    {
        public bool destroyOnUnload = false;
        public List<MonoSequence> beginWith = new List<MonoSequence>();
        public List<MonoSequence> waitFor = new List<MonoSequence>();
        public List<MonoSequence> finishWith = new List<MonoSequence>(); // Added the continueWith list
        public List<MonoSequence> continueWith = new List<MonoSequence>(); // Added the continueWith list
    }
}
