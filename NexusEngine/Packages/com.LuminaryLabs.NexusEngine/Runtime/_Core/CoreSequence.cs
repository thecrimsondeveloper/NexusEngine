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
                superSequence = this
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


        [FoldoutGroup("Core/Audio")]
        public CoreAudioDirector audioDirector;

        [FoldoutGroup("Core/Audio/Data")]
        public CoreAudioDirectorData audioDirectorData;

        [FoldoutGroup("Core/Initialization")]
        public CoreInitializationDirector initializationDirector;

        [FoldoutGroup("Core/Initialization/Data")]
        public CoreInitializationDirectorData initializationDirectorData;

        [FoldoutGroup("Core/Interaction")]
        public CoreInteractionHandler interactionHandler;

        [FoldoutGroup("Core/Interaction/Data")]
        public CoreInteractionHandlerData interactionHandlerData;

        [FoldoutGroup("Core/Intro")]
        public CoreIntroDirector introDirector;

        [FoldoutGroup("Core/Intro/Data")]
        public CoreIntroDirectorData introDirectorData;

        [FoldoutGroup("Core/Outro")]
        public CoreOutroDirector outroDirector;

        [FoldoutGroup("Core/Outro/Data")]
        public CoreOutroDirectorData outroDirectorData;


        [FoldoutGroup("Core/UI")]
        public CoreUIDirector uiDirector;

        [FoldoutGroup("Core/UI/Data")]
        public CoreUIDirectorData uiDirectorData;

        [FoldoutGroup("Core/Visuals")]
        public CoreVisualsDirector visualsDirector;

        [FoldoutGroup("Core/Visuals/Data")]
        public CoreVisualsDirectorData visualsDirectorData;
    }
}
