using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class CoreOutroDirector : MonoSequence<CoreOutroDirectorData>
    {
        protected override UniTask Initialize(CoreOutroDirectorData currentData)
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
    public class CoreOutroDirectorData
    {
        // Define data fields here
    }
}
