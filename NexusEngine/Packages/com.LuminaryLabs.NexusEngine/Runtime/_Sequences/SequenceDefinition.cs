
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    [System.Serializable]
    public class SequenceDefinition
    {
        public virtual ISequence GetSequence() { return null; }
        public virtual SequenceRunData UpdateData(SequenceRunData inputData) { return inputData; }
    }

    public class MonoSequenceDefinition : SequenceDefinition
    {
        #if ODIN_INSPECTOR
        [Required]
        [Title("Required")]
        #endif
        public MonoSequence monoSequence;

         #if ODIN_INSPECTOR
        [Title("Optional")]
        #endif
        public Transform spawnParent;

        [SerializeReference]
        public SequenceData sequenceData;


        public override SequenceRunData UpdateData(SequenceRunData inputData)
        {
            inputData.parent = spawnParent;
            inputData.spawnPosition = Vector3.zero;
            inputData.spawnRotation = Quaternion.identity;
            inputData.spawnSpace = Space.Self;
            inputData.sequenceData = this.sequenceData;

            return base.UpdateData(inputData);
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

        public override SequenceRunData UpdateData(SequenceRunData inputData)
        {
            inputData.sequenceData = this.sequenceData;
            return base.UpdateData(inputData);
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

        public override SequenceRunData UpdateData(SequenceRunData inputData)
        {
            inputData.sequenceData = this.sequenceData;
            return base.UpdateData(inputData);
        }

        public override ISequence GetSequence()
        {
            return baseSequence;
        }
    }







}