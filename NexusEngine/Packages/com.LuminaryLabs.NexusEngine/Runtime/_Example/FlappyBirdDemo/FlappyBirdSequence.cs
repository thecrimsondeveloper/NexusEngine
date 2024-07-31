using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class FlappyBirdSequence : MonoSequence<FlappyBirdSequenceData>
    {
        [SerializeField] FlappyBirdSequenceData data = null;

        protected override UniTask Initialize(FlappyBirdSequenceData currentData)
        {
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Sequence.Run(data.flappy, new SequenceRunData
            {
                superSequence = this,
                sequenceData = data.flappyData
            });

            Sequence.Run(data.pipeDirector, new SequenceRunData
            {
                superSequence = this,
                sequenceData = data.pipeDirectorData
            });
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class FlappyBirdSequenceData
    {
        public Flappy flappy;
        public FlappyData flappyData;

        public PipeDirector pipeDirector;
        public PipeDirectorData pipeDirectorData;

    }
}
