using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace LuminaryLabs.NexusEngine
{
    public class BaseDelayHandler : BaseSequence<BaseDelayHandlerData>
    {
        private float delay;
        protected override UniTask Initialize(BaseDelayHandlerData currentData)
        {
            delay = currentData.delay;
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            await UniTask.Delay((int)(delay * 1000));
            this.Complete();
        }
        
    }

    [System.Serializable]
    public class BaseDelayHandlerData : BaseSequenceData
    {
        public float delay;
    }
}

