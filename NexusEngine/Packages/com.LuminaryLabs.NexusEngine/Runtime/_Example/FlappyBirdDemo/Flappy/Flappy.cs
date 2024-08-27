using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Samples.FlappyBird
{
    public class Flappy : MonoSequence<FlappyData>
    {
        [SerializeField] MoveHandler moveHandler = null;
        [SerializeField] RidigbodyMoveHandlerData moveHandlerData = null;

        protected override async UniTask Initialize(FlappyData currentData)
        {
            if (currentData.moveHandlerPrefab)
            {
                moveHandler = Instantiate(currentData.moveHandlerPrefab, transform);
            }

            await UniTask.NextFrame();
        }

        protected override void OnBegin()
        {
            Debug.Log("Flappy OnBegin");
            Sequence.Run(moveHandler, new SequenceRunData
            {
                superSequence = this,
                sequenceData = moveHandlerData

            });
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveHandler.Jump();
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }


    }

    [System.Serializable]
    public class FlappyData
    {
        public MoveHandler moveHandlerPrefab;

    }
}
