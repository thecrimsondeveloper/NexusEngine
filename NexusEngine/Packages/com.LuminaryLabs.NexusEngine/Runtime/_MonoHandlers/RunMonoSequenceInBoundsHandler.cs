using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
namespace LuminaryLabs.NexusEngine
{

    public class RunMonoSequenceInBoundsHandler : EntitySequence<RunMonoSequenceInBoundsHandlerData>
    {
        private MonoSequence targetSequence;
        private float radius = 1;

        private ISequence runningSequence;
        protected override UniTask Initialize(RunMonoSequenceInBoundsHandlerData currentData)
        {
            targetSequence = currentData.targetSequence;
            radius = currentData.radius;
            return UniTask.CompletedTask;
        }   

        protected override void OnBegin()
        {
            Vector3 spawnPosition = Random.insideUnitSphere * radius;

            Sequence.Run(targetSequence, new SequenceRunData()
            {
                superSequence = this,
                spawnPosition = spawnPosition,
                spawnSpace = Space.Self,
                onBegin = OnTargetBegin,
                onUnload = OnTargetUnload,
            });
        }

        void OnTargetBegin(ISequence sequence)
        {
            runningSequence = sequence;
        }

        void OnTargetUnload(ISequence sequence)
        {
            runningSequence = null;
        }

        protected override async UniTask Unload()
        {

            if(runningSequence != null)
            {
                await Sequence.Stop(runningSequence);
            }
        }
    }

    [Serializable]
    public class RunMonoSequenceInBoundsHandlerData : SequenceData
    {
        public MonoSequence targetSequence;
        public float radius = 1;
    }
}


