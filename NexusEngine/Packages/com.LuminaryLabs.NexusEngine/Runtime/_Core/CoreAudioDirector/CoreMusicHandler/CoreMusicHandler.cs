using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class CoreMusicHandler : MonoSequence<CoreMusicHandlerData>
    {
        private AudioSource audioSource;
        private List<AudioClip> audioClips;
        private int currentClipIndex;

        protected override UniTask Initialize(CoreMusicHandlerData currentData)
        {
            // Initialize audio source and audio clips from the data

            if (currentData.audioSource)
                audioSource = currentData.audioSource;

            if (currentData.audioClips != null && currentData.audioClips.Count > 0)
                audioClips = currentData.audioClips;

            currentClipIndex = 0;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Logic to execute when the sequence begins
            PlayNextClip();
        }

        public void PlayNextClip()
        {
            if (audioClips != null && audioClips.Count > 0 && audioSource != null)
            {
                // Play the current audio clip
                audioSource.clip = audioClips[currentClipIndex];
                audioSource.Play();

                // Move to the next clip, looping back to the first if at the end
                currentClipIndex = (currentClipIndex + 1) % audioClips.Count;
            }
        }

        public void PlayClip(int index)
        {
            if (audioClips != null && audioClips.Count > 0 && audioSource != null)
            {
                // Play the audio clip at the specified index
                if (index >= 0 && index < audioClips.Count)
                {
                    audioSource.clip = audioClips[index];
                    audioSource.Play();
                }
            }
        }

        public void PlayClip(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                // Play the specified audio clip
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        protected override UniTask Unload()
        {
            // Cleanup logic here, if necessary
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }

            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class CoreMusicHandlerData
    {
        // List of audio clips to be played
        public List<AudioClip> audioClips;

        // Audio source to play the clips on
        public AudioSource audioSource;
    }
}
