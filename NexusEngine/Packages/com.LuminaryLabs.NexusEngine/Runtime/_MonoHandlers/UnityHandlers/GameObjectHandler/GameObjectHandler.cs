using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class GameObjectHandler : EntitySequence<GameObjectHandlerData>
    {
        public enum UseCase
        {
            Enable,
            Disable
        }
        private UseCase _useCase;
        private List<GameObject> gameObjects;
        
        protected override UniTask Initialize(GameObjectHandlerData currentData)
        {
            _useCase = currentData.useCase;
            if(currentData.gameObjects != null)
                gameObjects = currentData.gameObjects;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            switch (_useCase)
            {
                case UseCase.Enable:
                    foreach (var go in gameObjects)
                    {
                        go.SetActive(true);
                    }
                    break;
                case UseCase.Disable:
                    foreach (var go in gameObjects)
                    {
                        go.SetActive(false);
                    }
                    break;
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class GameObjectHandlerData : SequenceData
    {
        public List<GameObject> gameObjects;
        public GameObjectHandler.UseCase useCase;
    }
}

