using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Samples
{
    [System.Serializable]
    public class NumberSequenceCompletable : NexusObject, ICompletable
    {
        [SerializeField] bool _isComplete = false;
        public bool IsComplete { get => _isComplete; set => _isComplete = value; }

        public UnityEvent OnComplete { get; } = new UnityEvent();

        public UnityEvent OnReset { get; } = new UnityEvent();
        public int sequenceLength = 4;

        public NexusIntQueue targetSequence;

        [Button]
        public void CheckNextNumber(int number)
        {
            if (targetSequence.Count == 0)
            {
                CheckIfComplete();
                return;
            }
            //if the input number is the next number in the sequence then remove it from the sequence
            if (targetSequence.Peek() == number)
            {
                targetSequence.Dequeue();

                CheckIfComplete();
            }
        }

        public void CheckNumbers(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                CheckNextNumber(numbers[i]);
            }
        }

        void CheckIfComplete()
        {
            if (targetSequence.Count == 0)
            {
                (this as ICompletable).Complete();
            }
        }

        [Button]
        public void CycleSequence(int nextNumber)
        {
            //enqueue the next number in the sequence at the end of the sequence
            targetSequence.Enqueue(nextNumber);

            //dequeue the first number in the sequence if the sequence is longer than the sequence length
            if (targetSequence.Count > sequenceLength)
            {
                targetSequence.Dequeue();
            }
        }

        protected override void OnInitializeObject()
        {
            base.OnInitializeObject();
            targetSequence = CreateInstance<NexusIntQueue>();

            for (int i = 0; i < sequenceLength; i++)
            {
                targetSequence.Enqueue(Random.Range(10, 99));
            }

            targetSequence.InitializeObject();
        }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void Internal_OnComplete()
        {
        }

        public void Internal_OnReset()
        {
        }
    }
}
