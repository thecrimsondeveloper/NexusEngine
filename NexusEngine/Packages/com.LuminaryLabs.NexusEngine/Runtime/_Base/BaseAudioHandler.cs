using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseAudioHandler : BaseSequence<BaseAudioHandlerData>
    {
        private AudioSource audioSource;
        private AudioClip audioClip;

        protected override UniTask Initialize(BaseAudioHandlerData currentData)
        {
            audioSource = currentData.audioSource;
            audioClip = currentData.audioClip;
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (audioSource == null || audioClip == null)
            {
                Debug.LogWarning("AudioSource or AudioClip is null. Cannot play audio.");
                this.Complete();
                return;
            }

            audioSource.clip = audioClip;
            audioSource.Play();

            // Wait until the audio clip is done playing
            await UniTask.Delay((int)(audioClip.length * 1000));
            this.Complete();
        }
    }

    [System.Serializable]
    public class BaseAudioHandlerData : BaseSequenceData
    {
        public AudioSource audioSource;
        public AudioClip audioClip;
    }
}
