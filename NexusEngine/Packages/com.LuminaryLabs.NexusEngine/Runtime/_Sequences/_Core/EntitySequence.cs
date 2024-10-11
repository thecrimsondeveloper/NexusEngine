using System;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Samples;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace LuminaryLabs.NexusEngine
{
    public abstract class EntitySequence<T> : MonoSequence where T : SequenceData
    {
        public enum RunType
        {
            Start,
            Awake,
            Manual
        }
#if ODIN_INSPECTOR
        [BoxGroup("ENTITY DATA"), SerializeField]
#else
        [SerializeField]
#endif
        private RunType _runType = RunType.Manual;


// #if ODIN_INSPECTOR
//         [BoxGroup("ENTITY DATA"), SerializeField]
// #else
//         [SerializeField]
// #endif
//         // private NexusSequenceData nexusData;




#if ODIN_INSPECTOR
        [BoxGroup("ENTITY DATA"), SerializeField]
#else
        [SerializeField]
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
                onBegin = OnEntitySequenceBegin
            });
        }

        void OnEntitySequenceBegin(ISequence sequence)
        {
            // Debug.Log("NexusData: " + nexusData.GetSequence().GetType());

            // Sequence.Run(new NexusSequence(), new SequenceRunData{
            //     superSequence = this,
            //     sequenceData = nexusData
            // });     
        }

        protected override UniTask Initialize(object currentData = null)
        {
            if (currentData is T data)
            {
                return Initialize(data);
            }
            else if(this.currentData is T)
            {
                return Initialize(this.currentData);
            }

            return Initialize(new SequenceData() as T);
        }

        protected abstract UniTask Initialize(T currentData);
    }

    public sealed class EntitySequence : EntitySequence<EntitySequenceData>
    {
        protected override UniTask Initialize(EntitySequenceData currentData)
        {
            Debug.Log("INIT ENTITY");
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {

        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class EntitySequenceData : SequenceData
    {

    }

}
