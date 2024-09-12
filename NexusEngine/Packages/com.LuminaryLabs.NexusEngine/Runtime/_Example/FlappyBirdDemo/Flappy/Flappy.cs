using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class Flappy : MonoSequence<FlappyData>
    {

        protected override async UniTask Initialize(FlappyData currentData)
        {

            await UniTask.NextFrame();
        }

        protected override void OnBegin()
        {
            Debug.Log("Flappy OnBegin");
            // Sequence.Run(moveHandler, new SequenceRunData
            // {
            //     superSequence = this,
            //     sequenceData = moveHandlerData

            // });
        }
        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }


    }

    [System.Serializable]
    public class FlappyData : SequenceData
    {
        public MoveHandler moveHandlerPrefab;

    }
}
