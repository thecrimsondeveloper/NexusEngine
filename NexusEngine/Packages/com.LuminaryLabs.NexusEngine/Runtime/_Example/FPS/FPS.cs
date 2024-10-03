using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Example.FPSGame
{

    public class FPS : CoreSequence<FPSData>
    {
        protected override UniTask Initialize(FPSData currentData)
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
    [System.Serializable]
    public class FPSData : CoreSequenceData
    {

    }
}
