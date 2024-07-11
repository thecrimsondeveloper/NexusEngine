using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusAction : ScriptableSequence
    {
        [SerializeField, HideInEditorMode] protected NexusCondition condition;

        public MonoBehaviour mono;

        public bool TryExecute(MonoBehaviour mono, bool force = false)
        {
            if (condition == null || force)
            {
                Sequence.Run(this, new SequenceRunData { SuperSequence = null }).Forget();
                return true;
            }

            // if (!condition.IsConditionMet(mono))
            // {
            //     return false;
            // }

            Sequence.Run(this, new SequenceRunData { SuperSequence = null }).Forget();
            return true;
        }

        protected override UniTask Finish()
        {
            // Logic for async finish
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            // Logic for async load
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            // Logic for async unload
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            SetupAction();
        }

        protected override void OnStart()
        {
            OnExecute();
        }

        protected override void OnFinished()
        {
            // Logic for finishing the sequence
        }

        protected override void OnUnload()
        {
            Cleanup();
        }

        public abstract void SetupAction();

        public abstract void OnExecute();

        public virtual void Cleanup()
        {
            // Cleanup logic
        }
    }
}
