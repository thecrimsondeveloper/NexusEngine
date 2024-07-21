using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sequences
{
    public interface IBaseSequence
    {
        IBaseSequence superSequence { get; set; }
        object currentData { get; set; }
        Guid guid { get; set; }


        public virtual void SequenceStart()
        {
            OnSequenceStart();
        }
        void OnSequenceStart();
    }

    public interface ISequence : IBaseSequence
    {
        public virtual void Finish()
        {
            Sequence.Finish(this);
        }
        //event functions
        public virtual void Load()
        {
            OnSequenceLoad();
        }
        void OnSequenceLoad();




        public virtual void SequenceFinished()
        {
            OnSequenceFinished();
        }
        void OnSequenceFinished();


        public virtual void Unload()
        {
            OnSequenceUnload();
        }
        void OnSequenceUnload();
    }

    public interface IDataSequence : IBaseSequence
    {
        public object currentData { get; set; }

        public void SetCurrentData(object data)
        {
            currentData = data;
        }
        //event functions
        public virtual void Load(object data)
        {
            SetCurrentData(data);
            OnSequenceLoad();
        }
        void OnSequenceLoad();




        public virtual void SequenceFinished()
        {
            OnSequenceFinished();
        }
        void OnSequenceFinished();


        public virtual void Unload()
        {
            OnSequenceUnload();
        }
        void OnSequenceUnload();
    }

    public interface IAsyncDataSequence : IDataSequence
    {
        public virtual UniTask LoadSequence(object data)
        {
            SetCurrentData(data);
            return OnLoad_Async();
        }
        UniTask OnLoad_Async();

        public virtual UniTask FinishSequence()
        {
            return OnFinish_Async();
        }
        UniTask OnFinish_Async();

        public virtual UniTask UnloadSequence()
        {
            return UnLoad_Async();
        }
        UniTask UnLoad_Async();

    }

    public interface IAsyncSequence : ISequence
    {
        public virtual UniTask LoadSequence()
        {
            return OnLoad_Async();
        }
        UniTask OnLoad_Async();

        public virtual UniTask FinishSequence()
        {
            return OnFinish_Async();
        }
        UniTask OnFinish_Async();

        public virtual UniTask UnloadSequence()
        {
            return UnLoad_Async();
        }
        UniTask UnLoad_Async();

    }


}
