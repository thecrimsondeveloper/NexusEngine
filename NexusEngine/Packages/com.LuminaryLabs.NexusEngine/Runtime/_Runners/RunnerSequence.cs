using System.Collections.Generic;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class RunnerSequence : EntitySequence<RunnerSequenceData>
    {
        private List<MonoSequence> beginWith = new List<MonoSequence>();
        private List<MonoSequence> waitFor = new List<MonoSequence>();

        [ShowInInspector]
        private List<ISequence> waitForSequences = new List<ISequence>();

        [ShowInInspector]
        private List<ISequence> beginSequences = new List<ISequence>();

        protected override UniTask Initialize(RunnerSequenceData currentData)
        {
            beginWith = currentData.beginWith;
            waitFor = currentData.waitFor;
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
            CheckIfComplete();
        }

        private void CheckIfComplete()
        {
            if (waitForSequences.Count == 0)
            {
                Debug.Log("Completing Runner Sequence." + this.name, gameObject);
                Complete();
            }
        }

        private async void Complete()
        {
            foreach(ISequence sequence in beginSequences)
            {
                await Sequence.Stop(sequence);
            }

            foreach(ISequence sequence in waitForSequences)
            {
                await Sequence.Stop(sequence);
            }


            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            Destroy(gameObject);
            // Perform any necessary cleanup here
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class RunnerSequenceData : SequenceData
    {
        public List<MonoSequence> beginWith = new List<MonoSequence>();
        public List<MonoSequence> waitFor = new List<MonoSequence>();
    }
}
