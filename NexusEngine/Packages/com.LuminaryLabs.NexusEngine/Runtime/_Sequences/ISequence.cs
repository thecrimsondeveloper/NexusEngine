using System;
using Cysharp.Threading.Tasks;

namespace Toolkit.Sequences
{
    public interface ISequence
    {
        ISequence superSequence { get; set; }
        Guid guid { get; set; }
        object currentData { get; set; }

        UniTask Initialize(object currentData = null);
        void OnBegin();
        UniTask Finish();
        UniTask Unload();
        void OnFinished();
        void OnUnloaded();
    }
}
