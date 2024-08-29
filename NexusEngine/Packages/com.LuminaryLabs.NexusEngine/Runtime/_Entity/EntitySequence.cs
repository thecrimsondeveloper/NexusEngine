using System;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Samples;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace LuminaryLabs.NexusEngine
{
    public abstract class EntitySequence<T> : MonoSequence where T : EntitySequenceData
    {
        public enum RunType
        {
            Start,
            Awake,
            Manual
        }

        [SerializeField, BoxGroup("ENTITY DATA")]
        private RunType _runType = RunType.Manual;

#if ODIN_INSPECTOR 
        [SerializeField, BoxGroup("ENTITY DATA")]
#endif


        private T _currentData;

        public new T currentData
        {
            get => base.currentData != null ? (T)base.currentData : _currentData;
            set => _currentData = value;
        }

        protected virtual void Awake()
        {
            if (_runType == RunType.Awake)
            {
                Run();
            }
        }

        protected virtual void Start()
        {
            if (_runType == RunType.Start)
            {
                Run();
            }
        }

        void Run()
        {
            Sequence.Run(this, new SequenceRunData
            {
                sequenceData = currentData,
                superSequence = null,
            });
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

    [System.Serializable]
    public class EntitySequenceData
    {

    }

}
