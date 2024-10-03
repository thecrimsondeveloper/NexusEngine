using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.Example.FPSGame
{
    public class OscillationEntity : EntitySequence<OscillationEntityData>
    {
        OscillationMovementHandler oscillationMovementHandler;
        OscillationMovementHandlerData oscillationMovementHandlerData;

        protected override UniTask Initialize(OscillationEntityData currentData = null)
        {
            oscillationMovementHandler = currentData.movementHandler;
            oscillationMovementHandlerData = currentData.movementHandlerData;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Sequence.Run(oscillationMovementHandler, new SequenceRunData
            {
                superSequence = this,
                sequenceData = oscillationMovementHandlerData
            });
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class OscillationEntityData : SequenceData
    {
        public OscillationMovementHandler movementHandler;
        public OscillationMovementHandlerData movementHandlerData;
    }
}
