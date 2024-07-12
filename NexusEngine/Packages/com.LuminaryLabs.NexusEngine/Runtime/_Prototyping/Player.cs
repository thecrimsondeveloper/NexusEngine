using System;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Sequences
{
    public class Player : MonoSequence
    {
        [SerializeField] private PlayerMovementHandler movementHandler;
        [SerializeField] private PlayerVisualsHandler visualsHandler;

        protected override async UniTask Finish()
        {
            Debug.Log("PlayerSequence finished.");
            await UniTask.CompletedTask;
        }

        protected override async UniTask WhenLoad()
        {
            // Run the visual handler for the loading sequence
            Sequence.Run(visualsHandler);
            Sequence.Run(movementHandler);


            Debug.Log("PlayerSequence loaded.");
        }

        protected override async UniTask Unload()
        {
            Debug.Log("PlayerSequence unloaded.");
            await UniTask.CompletedTask;
        }

        protected override void OnStart()
        {
            // Run the movement handler
            Sequence.Run(movementHandler);
            Debug.Log("PlayerSequence started.");
        }

        protected override void AfterLoad()
        {
            Debug.Log("PlayerSequence after load.");
        }

        protected override void OnFinished()
        {
            Debug.Log("PlayerSequence finished.");
        }

        protected override void OnUnload()
        {
            Debug.Log("PlayerSequence unloaded.");
        }
    }
}
