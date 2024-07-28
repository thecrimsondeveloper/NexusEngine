using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using LuminaryLabs.Samples;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class Pipe : MonoSequence<PipeData>
    {
        [SerializeField] MoveHandler movementHandler = null;
        [SerializeField] TransformMoveHandlerData movementHandlerData = null;

        public override UniTask Initialize(PipeData currentData)
        {
            if (currentData.movementHandlerPrefab) movementHandler = currentData.movementHandlerPrefab;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Sequence.Run(movementHandler, new SequenceRunData
            {
                superSequence = this,
                sequenceData = movementHandlerData
            }).Forget();
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }


    public class PipeData
    {
        public Vector3 slide;
        public MoveHandler movementHandlerPrefab;
    }
}

