using System;
using Cysharp.Threading.Tasks;

namespace LuminaryLabs.Sequences
{
    [System.Serializable]
    public abstract class NexusSequence : ISequence
    {
        public ISequence superSequence { get; set; }
        public Guid guid { get; set; }
        public object currentData { get; set; }


        public UniTask InitializeSequence(object currentData)
        {
            return Initialize(currentData);
        }
        public void OnBeginSequence()
        {
            OnBegin();
        }

        public UniTask UnloadSequence()
        {
            return Unload();
        }
        public UniTask FinishSequence()
        {
            return Finish();
        }
        public void OnFinishedSequence()
        {
            OnFinished();
        }
        public void OnUnloadedSequence()
        {
            OnUnloaded();
        }

        protected abstract UniTask Initialize(object currentData = null);
        protected abstract void OnBegin();
        protected abstract UniTask Unload();

        protected virtual UniTask Finish() { return UniTask.CompletedTask; }
        protected virtual void OnFinished() { }
        protected virtual void OnUnloaded() { }
    }


    public abstract class NexusSequence<T> : NexusSequence
    {
        public T data => (T)currentData;

        protected override UniTask Initialize(object currentData)
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
