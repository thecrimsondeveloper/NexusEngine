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
        OscillationMovementHandlerData oscillationMovementHandlerData;

        MonoSequence movementHandler = null;

        protected override UniTask Initialize(OscillationEntityData currentData = null)
        {
            Debug.Log(currentData);
            oscillationMovementHandlerData = currentData.movementHandlerData;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Debug.Log("Oscillation Entity Begin");
            // Sequence.Run<OscillationMovementHandler>(new SequenceRunData
            // {
            //     superSequence = this,
            //     sequenceData = oscillationMovementHandlerData,
            //     onBegin = OnMovementHandlerBegin
            // });
        }

        void OnMovementHandlerBegin(ISequence sequence)
        {
            movementHandler = sequence as MonoSequence;
        }

        protected override async UniTask Unload()
        {

            if(movementHandler)
            {
                await Sequence.Stop(movementHandler);
            }
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class OscillationEntityData : SequenceData
    {
        public OscillationMovementHandlerData movementHandlerData;
    }
}
