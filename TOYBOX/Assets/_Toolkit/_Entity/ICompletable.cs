using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Entity
{
    public interface ICompletable : IEntity
    {
        public bool IsComplete { get; set; }
        public UnityEvent OnComplete { get; }
        public UnityEvent OnReset { get; }
        void Internal_OnComplete();
        void Internal_OnReset();


        public void Complete()
        {
            if (IsComplete) return;
            IsComplete = true;
            OnComplete.Invoke();
            Internal_OnComplete();
        }

        public void Reset()
        {
            if (!IsComplete) return;
            IsComplete = false;
            OnReset.Invoke();
            Internal_OnReset();
        }
    }
}
