using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public abstract class MonoSequence : MonoBehaviour, ISequence
    {
        public ISequence superSequence { get; set; }
        public Guid guid { get; set; }

        public object currentData { get; set; }
        public Phase phase { get; set; }

        public UniTask InitializeSequence(object currentData = null)
        {
            return Initialize(currentData);
        }
        public virtual void OnBeginSequence()
        {
            OnBegin();
        }

        public virtual UniTask UnloadSequence()
        {
            return Unload();
        }
        public virtual UniTask FinishSequence()
        {
            return Finish();
        }
        public virtual void OnFinishedSequence()
        {
            OnFinished();
        }
        public virtual void OnUnloadedSequence()
        {
            OnUnloaded();
        }


        protected abstract UniTask Initialize(object currentData);
        protected abstract void OnBegin();
        protected abstract UniTask Unload();

        protected virtual UniTask Finish() { return UniTask.CompletedTask; }
        protected virtual void OnFinished() { }
        protected virtual void OnUnloaded() { }

      
    }

    public abstract class MonoSequence<T> : MonoSequence
    {
        public new T currentData
        {
            get => (T)base.currentData;
            set => base.currentData = value;
        }

        protected override UniTask Initialize(object currentData = null)
        {
            if (currentData is T data)
            {
                return Initialize(data);
            }
            return UniTask.CompletedTask;
        }

        protected abstract UniTask Initialize(T currentData);
    }
}
