using System;
using Cysharp.Threading.Tasks;

namespace Toolkit.Sequences
{
    public abstract class NexusSequence : ISequence
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
}
