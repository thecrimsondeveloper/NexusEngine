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
    private new T _currentData;

    // Override the base property to ensure consistent access
    public override object currentData
    {
        get => _currentData;
        set
        {
            base.currentData = value;
            
            if (value is T typedValue)
            {
                _currentData = typedValue;
            }
            else
            {
                Debug.LogError($"Invalid type assignment for currentData. Expected {typeof(T)}, got {value?.GetType()}");
            }
        }
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
            Nexus.Log("Init Entity Sequence: " + name);
            if(currentData != null) 
            {
                Nexus.Log("Init Entity Given Data Type: " + currentData.GetType().ToString());  
            }

            if (currentData is T data)
            {
                Nexus.Log("Init Entity Given Data: " + name);  
                return Initialize(data);
            }
            else if(this.currentData is T)
            {
                Nexus.Log("Init Entity Current Data: " + name);
                return Initialize(this.currentData);
            }

            Nexus.Log("Init Entity New Data(): " + name);
            return Initialize(new SequenceData() as T);
        }

        protected abstract UniTask Initialize(T currentData);
    }

    
    [System.Serializable]
    public class EntitySequenceData : SequenceData
    {

    }

}
