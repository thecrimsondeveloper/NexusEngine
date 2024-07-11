using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Entity;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace Toolkit.Samples
{
    public class SimonMemoryPuzzle : MonoBehaviour, ICompletable
    {
        [SerializeField, HideLabel, BoxGroup("Sequence Controller")]
        protected NumberSequenceCompletable sequenceCompletable;
        public bool IsComplete { get; set; } = false;
        public int totalRounds = 5;
        public UnityEvent OnComplete { get { return sequenceCompletable.OnComplete; } }
        public UnityEvent OnReset { get { return sequenceCompletable.OnReset; } }

        int difficulty = 1;

        bool simonRoundActive = false;
        protected virtual void Start()
        {
            sequenceCompletable.OnComplete.AddListener(SequenceComplete);
            StartSimonRound();
        }

        [Button]
        public bool GuessNumber(int number)
        {
            Debug.Log("Guess: " + number);
            // return sequenceCompletable.InputNumber(number);
            return false;
        }

        void SequenceComplete()
        {
            if (simonRoundActive == false)
            {
                return;
            }

            OnSequenceComplete();
            simonRoundActive = false;

            if (difficulty == totalRounds)
            {
                (this as ICompletable).Complete();
                return;
            }


            difficulty++;
            StartSimonRound();
        }
        void StartSimonRound()
        {
            simonRoundActive = true;
            GenerateSequence(difficulty + 2);

            OnStartSimonRound();
        }



        void GenerateSequence(int numberOfNumbers)
        {
            int[] targetSequence = new int[numberOfNumbers];
            for (int i = 0; i < numberOfNumbers; i++)
            {
                int number = Random.Range(0, 4);
                targetSequence[i] = number;
            }

            // sequenceCompletable.InititializeSequence(targetSequence);
            OnGenerateSequence(targetSequence);
        }

        public virtual void Internal_OnComplete()
        {
            Debug.Log("Simon Memory Puzzle Complete");
        }

        protected virtual void OnSequenceComplete()
        {

        }

        protected virtual void OnStartSimonRound()
        {

        }

        protected virtual void OnGenerateSequence(int[] sequence)
        {

        }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void Internal_OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}
