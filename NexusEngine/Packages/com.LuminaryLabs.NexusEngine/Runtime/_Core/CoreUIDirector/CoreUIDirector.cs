using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class CoreUIDirector : MonoSequence<CoreUIDirectorData>
    {
        protected override UniTask Initialize(CoreUIDirectorData currentData)
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
    public class CoreUIDirectorData
    {
        // Define data fields here
    }
}
