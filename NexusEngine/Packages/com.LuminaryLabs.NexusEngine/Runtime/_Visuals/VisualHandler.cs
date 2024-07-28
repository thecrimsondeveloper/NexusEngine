using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LuminaryLabs.NexusEngine
{
    public class VisualsHandler : MonoSequence
    {
        public float scaleDuration = 1f;
        private Vector3 originalScale;


        protected override async UniTask Initialize(object currentData = null)
        {
            if (currentData is VisualsHandlerData data)
            {
                if (data.scaleDuration != -1) scaleDuration = data.scaleDuration;
            }


            originalScale = transform.localScale;
            transform.localScale = Vector3.zero;

            //lerp the scale of the player back to the original scale
            for (float i = 0; i < scaleDuration; i += Time.deltaTime)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, i / scaleDuration);
                await UniTask.NextFrame();
            }

            transform.localScale = originalScale;
        }


        protected override void OnBegin()
        {

        }

        protected override async UniTask Unload()
        {
            await UniTask.CompletedTask;
        }
    }

    public class VisualsHandlerData
    {
        public float scaleDuration = -1;
    }
}

