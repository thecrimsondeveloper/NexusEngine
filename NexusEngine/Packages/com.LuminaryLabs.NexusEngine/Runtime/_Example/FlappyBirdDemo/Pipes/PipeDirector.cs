using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class PipeDirector : BaseSequence<PipeDirectorData>
    {
        [SerializeField] private Pipe pipePrefab = null;
        [SerializeField] Transform pipeSpawnPoint = null;

        protected override UniTask Initialize(PipeDirectorData currentData)
        {
            if (currentData.pipeSequence) pipePrefab = currentData.pipeSequence;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Debug.Log("PipeDirector.OnBegin: " + pipeSpawnPoint.position);
            Debug.Log("PipeDirector.OnBegin: " + pipeSpawnPoint.rotation);

            Sequence.Run(pipePrefab, new SequenceRunData
            {
                superSequence = this,
                sequenceData = new PipeData
                {
                    movementHandlerPrefab = currentData.pipeMoveHandlerPrefab
                },
                spawnPosition = pipeSpawnPoint.position,
                spawnRotation = pipeSpawnPoint.rotation
            });
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class PipeDirectorData
    {
        public Pipe pipeSequence = null;
        public MoveHandler pipeMoveHandlerPrefab = null;
    }

}
