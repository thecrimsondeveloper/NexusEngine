using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.Sequences
{
    public abstract class MonoSequence : MonoBehaviour, ISequence
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

    public abstract class MonoSequence<T> : MonoSequence
    {
        public new T currentData { get; set; }

        public override UniTask Initialize(object currentData = null)
        {
            if (currentData is T data)
            {
                this.currentData = data;
                return Initialize(data);
            }
            return UniTask.CompletedTask;
        }

        public abstract UniTask Initialize(T currentData);
    }
}
