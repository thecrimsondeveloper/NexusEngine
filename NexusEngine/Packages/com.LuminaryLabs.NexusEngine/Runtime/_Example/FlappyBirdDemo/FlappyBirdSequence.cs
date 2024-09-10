using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using LuminaryLabs.NexusEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class FlappyBirdSequence : CoreSequence<FlappyBirdSequenceData>
    {
        protected override UniTask Initialize(FlappyBirdSequenceData currentData)
        {
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {


            Sequence.Run(currentData.pipeDirector, new SequenceRunData
            {
                superSequence = this,
                sequenceData = currentData.pipeDirectorData
            });
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class FlappyBirdSequenceData : CoreSequenceData
    {


        public PipeDirector pipeDirector;
        public PipeDirectorData pipeDirectorData;

    }
}
