using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.Sequences
{
    public abstract class ScriptableSequence : ScriptableObject, ISequence
    {

        public ISequence superSequence { get; set; }
        public Guid guid { get; set; }
        public object currentData { get; set; }

        public abstract UniTask Initialize(object currentData = null);
        public abstract void OnBegin();
        public abstract UniTask Unload();

        public virtual UniTask Finish() { return UniTask.CompletedTask; }
        public virtual void OnFinished() { }
        public virtual void OnUnloaded() { }

    }

    public abstract class ScriptableSequence<T> : ScriptableSequence
    {
        public T data => (T)currentData;

        public override UniTask Initialize(object currentData)
        {
            //if is the correct type then call the correct initialize
            if (currentData is T)
            {
                return Initialize((T)currentData);
            }
            return UniTask.CompletedTask;
        }

        public abstract UniTask Initialize(T currentData);
    }
}
