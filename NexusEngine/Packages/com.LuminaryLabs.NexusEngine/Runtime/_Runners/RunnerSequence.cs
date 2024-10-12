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
        private List<MonoSequence> continueWith = new List<MonoSequence>();

        [ShowInInspector]
        private List<ISequence> waitForSequences = new List<ISequence>();

        [ShowInInspector]
        private List<ISequence> beginSequences = new List<ISequence>();

        [ShowInInspector]
        private List<ISequence> continueSequences = new List<ISequence>();

        protected override UniTask Initialize(RunnerSequenceData currentData)
        {
            destroyOnUnload = currentData.destroyOnUnload;
            beginWith = currentData.beginWith;
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

            // Check if all waitFor sequences are already complete
            CheckIfWaitForComplete();
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
                StartContinueWithSequences();
            }
        }

        private void StartContinueWithSequences()
        {
            continueSequences.Clear();
            foreach (MonoSequence sequence in continueWith)
            {
                Sequence.Run(sequence, new SequenceRunData
                {
                    superSequence = this,
                    onBegin = OnContinueSequenceBegin,
                    onFinished = OnContinueSequenceFinished
                });
            }

            // If there are no continueWith sequences, complete immediately
            if (continueSequences.Count == 0)
            {
                Complete();
            }
        }

        private void OnContinueSequenceBegin(ISequence sequence)
        {
            continueSequences.Add(sequence);
        }

        private void OnContinueSequenceFinished(ISequence sequence)
        {
            Debug.Log("ContinueWith sequence finished: " + sequence.GetType());
            continueSequences.Remove(sequence);

            // Check if all continueWith sequences are finished
            if (continueSequences.Count == 0)
            {
                Complete();
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

            foreach (ISequence sequence in continueSequences)
            {
                await Sequence.Stop(sequence);
            }

            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            if(destroyOnUnload)
            {
                Destroy(gameObject);
            }
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class RunnerSequenceData : SequenceData
    {
        public bool destroyOnUnload = false;
        public List<MonoSequence> beginWith = new List<MonoSequence>();
        public List<MonoSequence> waitFor = new List<MonoSequence>();
        public List<MonoSequence> continueWith = new List<MonoSequence>(); // Added the continueWith list
    }
}
