using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class CoreInteractionHandler : MonoSequence<CoreInteractionHandlerData>
    {
        protected override UniTask Initialize(CoreInteractionHandlerData currentData)
        {
            // Initialization logic here
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Logic to execute when the sequence begins
        }

        protected override UniTask Unload()
        {
            // Cleanup logic here
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class CoreInteractionHandlerData
    {
        // Define data fields here
    }
}
