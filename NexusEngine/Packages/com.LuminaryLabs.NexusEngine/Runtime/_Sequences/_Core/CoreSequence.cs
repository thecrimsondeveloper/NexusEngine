using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public abstract class CoreSequence<T> : MonoSequence where T : CoreSequenceData
    {

        [SerializeField]
#if ODIN_INSPECTOR
        [BoxGroup("CORE DATA")]
#endif
        bool runOnStart = true;

        [SerializeField]
#if ODIN_INSPECTOR
        [HideLabel, BoxGroup("CORE DATA")]
#endif
        private T _currentData;
        



        public new T currentData
        {
            get => base.currentData != null ? (T)base.currentData : _currentData;
            set => _currentData = value;
        }

        protected virtual void Start()
        {
            if (runOnStart)
            {
                Sequence.Run(this, new SequenceRunData
                {
                    sequenceData = currentData,
                    superSequence = null,
                });
            }
        }

        protected override UniTask Initialize(object currentData = null)
        {
            if (currentData is T data)
            {
                return Initialize(data);
            }
            return UniTask.CompletedTask;
        }

        public override void OnBeginSequence()
        {
            base.OnBeginSequence();

            foreach(var monoSequence in currentData.monoSequencesToRun)
            {
                Sequence.Run(monoSequence, new SequenceRunData()
                {
                    superSequence = this,
                });
            }
        }

        protected abstract UniTask Initialize(T currentData);
    }

    [System.Serializable]
    public class CoreSequenceData
    {
#if ODIN_INSPECTOR
        [FoldoutGroup("Core")]
#endif
        public string name;

        #if ODIN_INSPECTOR
        [FoldoutGroup("Core")]
#endif
        public List<MonoSequence> monoSequencesToRun = new List<MonoSequence>();
    }
}
