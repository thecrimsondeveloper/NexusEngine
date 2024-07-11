using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Sequences
{
    public abstract class ScriptableSequence : ScriptableObject, IAsyncDataSequence
    {

        public object currentData
        {
            get;
            set;
        }
        public Guid guid
        {
            get;
            set;
        }
        public IBaseSequence superSequence
        {
            get;
            set;
        }

        public UniTask OnFinish_Async() { return Finish(); }

        protected abstract UniTask Finish();

        public UniTask OnLoad_Async() { return WhenLoad(); }

        protected abstract UniTask WhenLoad();

        public UniTask UnLoad_Async()
        {
            return Unload();
        }

        protected abstract UniTask Unload();


        public void OnSequenceLoad()
        {
            AfterLoad();
        }

        protected virtual void AfterLoad() { }

        public void OnSequenceStart()
        {
            OnStart();
        }

        protected virtual void OnStart() { }

        public void OnSequenceFinished()
        {
            OnFinished();
        }


        protected virtual void OnFinished() { }


        public void OnSequenceUnload()
        {
            OnUnload();
        }


        protected virtual void OnUnload() { }

    }
}
