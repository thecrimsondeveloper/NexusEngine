using System;
using Cysharp.Threading.Tasks;
        #if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    [System.Serializable]
    public abstract class BaseSequence : ISequence
    {

        public ISequence superSequence { get; set; }
        public Guid guid { get; set; }
        protected object _currentData;

        public virtual object currentData
        {
            get => _currentData;
            set => _currentData = value;
        }
        
        public Phase phase { get; set; }

        // public string name => (this as ISequence).name;

        public UniTask InitializeSequence(object currentData = null)
        {
            Nexus.Log("Init Base Sequence: " + (this as ISequence).name);

            return Initialize(currentData);
        }
        public virtual void OnBeginSequence()
        {
            OnBegin();
        }

        public virtual UniTask UnloadSequence()
        {
            return Unload();
        }
        public virtual UniTask FinishSequence()
        {
            return Finish();
        }
        public virtual void OnFinishedSequence()
        {
            OnFinished();
        }
        public virtual void OnUnloadedSequence()
        {
            OnUnloaded();
        }


        protected virtual UniTask Initialize(object currentData) 
        {
            // Nexus.Log("Init Mono: " + name);
            return UniTask.CompletedTask;
        }
        protected abstract void OnBegin();
        protected virtual UniTask Unload(){return UniTask.CompletedTask;}

        protected virtual UniTask Finish() { return UniTask.CompletedTask; }
        protected virtual void OnFinished() { }
        protected virtual void OnUnloaded() { }

    }


   public abstract class BaseSequence<T> : BaseSequence where T : BaseSequenceData
    {


    // #if ODIN_INSPECTOR
    //         [BoxGroup("ENTITY DATA"), SerializeField]
    // #else
    //         [SerializeField]
    // #endif
    //         // private NexusSequenceData nexusData;




        #if ODIN_INSPECTOR
            [BoxGroup("RUN DATA"), SerializeField]
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

        private bool completeAfterBegin = false;


        protected override UniTask Initialize(object currentData = null)
        {
            if(currentData != null) 
            {
                Nexus.Log("Init Entity Given Data Type: " + currentData.GetType().ToString());  
            }


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

        protected async virtual void Complete()
        {
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }
    }

    [System.Serializable]
    public class BaseSequenceData : SequenceData
    {

    }   
}
