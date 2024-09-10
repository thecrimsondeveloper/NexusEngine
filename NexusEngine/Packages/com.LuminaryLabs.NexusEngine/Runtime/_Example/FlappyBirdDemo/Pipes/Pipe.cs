using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using LuminaryLabs.Samples;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class Pipe : EntitySequence<PipeData>
    {
        [SerializeField] MoveHandler movementHandler = null;
        [SerializeField] TransformMoveHandlerData movementHandlerData = null;

        protected override UniTask Initialize(PipeData currentData)
        {
            if (currentData.movementHandlerPrefab) movementHandler = currentData.movementHandlerPrefab;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Sequence.Run(movementHandler, new SequenceRunData
            {
                superSequence = this,
                sequenceData = movementHandlerData,
                onGenerated = (handler) =>
                {
                    movementHandler = handler as MoveHandler;
                }
            });
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }


    public class PipeData : SequenceData
    {
        public Vector3 slide;
        public MoveHandler movementHandlerPrefab;
    }
}

