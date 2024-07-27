using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class PipeDirector : MonoSequence<PipeDirectorData>
    {
        [SerializeField] private Pipe pipePrefab = null;
        [SerializeField] Transform pipeSpawnPoint = null;

        public override UniTask Initialize(PipeDirectorData currentData)
        {
            if (currentData.pipe) pipePrefab = currentData.pipe;
            return UniTask.CompletedTask;
        }

        public override void OnBegin()
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
            }).Forget();
        }

        public override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class PipeDirectorData
    {
        public Pipe pipe = null;
        public MoveHandler pipeMoveHandlerPrefab = null;
    }

}