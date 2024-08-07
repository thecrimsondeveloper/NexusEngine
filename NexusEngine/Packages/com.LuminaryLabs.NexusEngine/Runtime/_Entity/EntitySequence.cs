using System;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Samples;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class EntitySequence : MonoSequence
    {
        [SerializeField] private MoveHandler movementHandler;
        [SerializeField] private VisualsHandler visualsHandler;


        protected override UniTask Initialize(object currentData = null)
        {
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {

        }

        protected override UniTask Unload()
        {


            return UniTask.CompletedTask;
        }


    }

    public class EntitySequenceData
    {
        public MoveHandler movementHandlerPrefab;
        public MoveHandlerData movementHandlerData;
        public VisualsHandler visualsHandlerPrefab;
    }
}
