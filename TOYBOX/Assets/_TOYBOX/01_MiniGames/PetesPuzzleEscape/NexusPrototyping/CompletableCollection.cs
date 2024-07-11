using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class CompletableCollection : MonoBehaviour, ICompletable
    {
        [SerializeField] List<MonoBehaviour> completables = new List<MonoBehaviour>();
        List<ICompletable> registeredCompletables { get; set; } = new List<ICompletable>();

        private void OnValidate()
        {
            for (int i = 0; i < completables.Count; i++)
            {
                if (completables[i] == null)
                {
                    completables.RemoveAt(i);
                    i--;
                }

                if (completables[i] is ICompletable completable)
                {
                    //do nothing
                }
                else
                {
                    Debug.LogError($"Completable must implement ICompletable");
                    completables.RemoveAt(i);
                    i--;
                }

            }


        }

        private void Start()
        {
            foreach (var completable in completables)
            {
                if (completable is ICompletable completableInts)
                {
                    registeredCompletables.Add(completableInts);
                    completableInts.OnComplete.AddListener(CheckIfComplete);
                }
            }
        }




        public bool IsComplete { get; set; } = false;

        public UnityEvent OnComplete { get; set; } = new UnityEvent();

        public UnityEvent OnReset { get; set; } = new UnityEvent();

        [Button]
        void CheckIfComplete()
        {
            bool complete = true;
            foreach (var completable in registeredCompletables)
            {
                if (!completable.IsComplete)
                {
                    complete = false;
                    break;
                }
            }

            if (complete)
            {
                (this as ICompletable).Complete();
            }
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
