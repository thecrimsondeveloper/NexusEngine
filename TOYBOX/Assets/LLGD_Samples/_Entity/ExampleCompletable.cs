using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class ExampleCompletable : MonoBehaviour, ICompletable
    {
        public UnityEvent OnComplete { get; } = new UnityEvent();
        public UnityEvent OnReset { get; } = new UnityEvent();
        public bool IsComplete { get; set; } = false;

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
            throw new System.NotImplementedException();
        }

        public void Internal_OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}
