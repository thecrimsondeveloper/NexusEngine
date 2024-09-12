using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class NexusSequence : EntitySequence<NexusSequenceData>
    {
        private List<NexusSequenceDefinition> _sequenceDefinitions = new List<NexusSequenceDefinition>();

        private ISequence _currentSequence;
        private int _currentSequenceIndex = 0;
        protected override UniTask Initialize(NexusSequenceData currentData)
        {
            _sequenceDefinitions = currentData._sequenceDefinitions;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            if (_sequenceDefinitions.Count > 0)
            {
                RunRootSequence(_currentSequenceIndex);
            }
            else
            {
                Sequence.Finish(this);
            }
        }



        void RunSubDefinition(NexusSequenceDefinition definition, ISequence super = null)
        {
            ISequence sequence = definition.GetSequence();
            object data = definition.GetData();

            Sequence.Run(sequence, new SequenceRunData
            {
                superSequence = super,
                sequenceData = data,
            });
        }
        void RunSubSequences(List<NexusSequenceDefinition> definitions, ISequence super)
        {
            foreach (var definition in definitions)
            {
                RunSubDefinition(definition, super);
            }
        }

        void RunRootSequence(int i)
        {
            NexusSequenceDefinition sequenceDefinition = _sequenceDefinitions[i];

            if (sequenceDefinition != null)
            {
                ISequence sequence = sequenceDefinition.GetSequence();
                object data = sequenceDefinition.GetData();

                Sequence.Run(sequenceDefinition.GetSequence(), new SequenceRunData
                {
                    superSequence = this,
                    sequenceData = data,
                    onFinished = OnRootSequenceFinished,
                    onBegin = (currentSequence) =>
                    {
                        RunSubSequences(sequenceDefinition.subSequences, currentSequence);
                    }
                });
            }
        }

        void OnRootSequenceFinished(ISequence sequence)
        {
            _currentSequenceIndex++;
            if (_currentSequenceIndex < _sequenceDefinitions.Count)
            {
                RunRootSequence(_currentSequenceIndex);
            }
            else
            {
                Sequence.Finish(this);
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class NexusSequenceData : SequenceData
    {
        [SerializeReference, HideReferenceObjectPicker]
        public List<NexusSequenceDefinition> _sequenceDefinitions = new List<NexusSequenceDefinition>();
    }

    [System.Serializable]
    public class NexusSequenceDefinition
    {
        [SerializeReference]
        public SequenceDefinition nexusSequence;

        public ISequence GetSequence()
        {
            return nexusSequence.GetSequence();
        }

        public object GetData()
        {
            return nexusSequence.GetData();
        }

        public List<NexusSequenceDefinition> subSequences = new List<NexusSequenceDefinition>();

    }




}