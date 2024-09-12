
using System;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    [System.Serializable]
    public class SequenceDefinition
    {
        public virtual ISequence GetSequence() { return null; }
        public virtual object GetData() { return null; }
    }

    public class MonoSequenceDefinition : SequenceDefinition
    {
        public MonoSequence monoSequence;
        [SerializeReference]
        public SequenceData sequenceData;

        public override object GetData()
        {
            return sequenceData;
        }

        public override ISequence GetSequence()
        {
            return monoSequence;
        }
    }

    public class ScriptableSequenceDefinition : SequenceDefinition
    {
        public ScriptableSequence scriptableSequence;
        [SerializeReference]
        public SequenceData sequenceData;

        public override object GetData()
        {
            return sequenceData;
        }

        public override ISequence GetSequence()
        {
            return scriptableSequence;
        }
    }

    public class BaseSequenceDefinition : SequenceDefinition
    {
        public BaseSequence baseSequence;
        [SerializeReference]
        public SequenceData sequenceData;

        public override object GetData()
        {
            return sequenceData;
        }

        public override ISequence GetSequence()
        {
            return baseSequence;
        }
    }







}