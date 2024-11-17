
using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
    public interface ISequenceDefinition<TSequence,TData, TModification> where TSequence : ISequence
    {
        TSequence SequenceToRun { get; set; }
        TData SequenceData { get; set; }
        TModification ModificationObject { get; set; }
        

        void RunDefinition()
        {
            SequenceRunData sequenceRunData = new SequenceRunData();
            


            Sequence.Run(SequenceToRun, sequenceRunData);
        }


        
        void ModifyDefinitionData(TData data);
    }


    public class RayCastHitSequenceDefinition : ISequenceDefinition<RayCastHitHandler, RayCastHitHandlerData, RaycastHit>
    {
        [SerializeReference]
        private RayCastHitHandler sequenceToRun;

        public RayCastHitHandler SequenceToRun { get => sequenceToRun; set => sequenceToRun = value; }

        [SerializeReference]

        private RayCastHitHandlerData sequenceData;

        public RayCastHitHandlerData SequenceData { get => sequenceData; set => sequenceData = value; }

        public RaycastHit ModificationObject { get; set; }

        public void ModifyDefinitionData(RayCastHitHandlerData data)
        {
            data.raycastHit = ModificationObject;   
        }
    }


    [System.Serializable]
    public class BaseSequenceDefinition
    {

        [SerializeReference]
        public BaseSequence sequenceToRun;

        [SerializeReference]
        public BaseSequenceData sequenceData;

    }





}