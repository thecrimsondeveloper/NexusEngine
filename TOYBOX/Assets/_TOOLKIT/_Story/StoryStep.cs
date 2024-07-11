using System;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Stories
{
    public abstract class StoryStep : ScriptableSequence
    {
        [SerializeField] StoryStep nextStep = null;

        protected override UniTask Finish()
        {
            OnStepEnd();

            if (nextStep != null)
            {
                Sequence.Run(nextStep);
            }

            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            return OnLoadAsync();
        }

        protected override UniTask Unload()
        {
            return UnLoadAsync();
        }

        protected override void AfterLoad()
        {
            OnSequenceLoad();
        }

        protected override void OnStart()
        {
            OnStepStart();
        }

        protected override void OnFinished()
        {
            OnStepEnd();
            if (nextStep != null)
            {
                Sequence.Run(nextStep);
            }
        }

        protected override void OnUnload()
        {
            OnSequenceUnload();
        }
        public UniTask OnLoadAsync()
        {
            return LoadStep();
        }
        public UniTask UnLoadAsync()
        {
            return UnloadStep();
        }

        public abstract void OnSequenceLoad();
        public abstract void OnStepStart();
        public abstract void OnStepEnd();
        public abstract UniTask LoadStep();
        public abstract UniTask UnloadStep();

    }
}
