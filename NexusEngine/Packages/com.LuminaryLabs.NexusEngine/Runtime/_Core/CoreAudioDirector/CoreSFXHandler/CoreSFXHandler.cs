using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class CoreSFXHandler : MonoSequence<CoreSFXHandlerData>
    {
        protected override UniTask Initialize(CoreSFXHandlerData currentData)
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
    public class CoreSFXHandlerData
    {
        // Define data fields here
    }
}
