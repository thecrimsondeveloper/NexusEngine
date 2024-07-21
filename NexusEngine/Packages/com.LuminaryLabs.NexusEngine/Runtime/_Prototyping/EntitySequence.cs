using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Sequences
{
    public class EntitySequence : MonoSequence
    {
        [SerializeField] private PlayerMovementHandler movementHandler;
        [SerializeField] private PlayerVisualsHandler visualsHandler;

        public IBaseSequence MovementHandler
        {
            get => movementHandler;
            set => movementHandler = value as PlayerMovementHandler;
        }

        public IBaseSequence VisualsHandler
        {
            get => visualsHandler;
            set => visualsHandler = value as PlayerVisualsHandler;
        }

        protected override async UniTask WhenLoad()
        {
            Debug.Log("EntitySequence loading...");


            if (VisualsHandler != null)
            {
                await Sequence.Run(VisualsHandler);
            }

            if (MovementHandler != null)
            {
                await Sequence.Run(MovementHandler);
            }

            Debug.Log("EntitySequence loaded.");
        }

        protected override void OnStart()
        {
            Debug.Log("EntitySequence started.");

            if (MovementHandler != null)
            {
                Sequence.Run(MovementHandler).Forget();
            }
        }

        protected override async UniTask Finish()
        {
            Debug.Log("EntitySequence finishing...");
            await UniTask.CompletedTask;
        }

        protected override async UniTask Unload()
        {
            Debug.Log("EntitySequence unloading...");
            await UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            Debug.Log("EntitySequence after load.");
        }

        protected override void OnFinished()
        {
            Debug.Log("EntitySequence finished.");
        }

        protected override void OnUnload()
        {
            Debug.Log("EntitySequence unloaded.");
        }
    }
}
