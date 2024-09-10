using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;


namespace LuminaryLabs.NexusEngine
{
    public abstract class CoreSequence<T> : MonoSequence where T : CoreSequenceData
    {
        [SerializeField, HideLabel, BoxGroup("CORE DATA")]
        private T _currentData;

        public new T currentData
        {
            get => base.currentData != null ? (T)base.currentData : _currentData;
            set => _currentData = value;
        }

        protected virtual void Start()
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
    public class CoreSequenceData
    {
        [FoldoutGroup("Core")]
        public string name;



    }


}
