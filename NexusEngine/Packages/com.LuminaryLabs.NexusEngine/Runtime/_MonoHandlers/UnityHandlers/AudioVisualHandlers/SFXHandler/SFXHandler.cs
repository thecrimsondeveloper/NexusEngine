using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class SFXHandler : EntitySequence<SFXHandlerData>
    {
        public enum PlaybackMode
        {
            PlayOnce,
            Loop
        }

        private AudioSource _audioSource;
        private PlaybackMode _playbackMode;
        private AudioClip _clip;

        protected override UniTask Initialize(SFXHandlerData currentData)
        {
            // Assign AudioClip and playback mode
            _clip = currentData.audioClip;
            _playbackMode = currentData.playbackMode;

            // Get or add AudioSource component
            if (currentData.audioSource != null)
            {
                _audioSource = currentData.audioSource;
            }
            else
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Set the audio clip to the AudioSource
            _audioSource.clip = _clip;

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            // Play the audio based on the playback mode
            switch (_playbackMode)
            {
                case PlaybackMode.PlayOnce:
                    _audioSource.loop = false;
                    _audioSource.Play();
                    // Wait for the clip to finish playing
                    await UniTask.Delay((int)(_clip.length * 1000));
                    break;
                case PlaybackMode.Loop:
                    _audioSource.loop = true;
                    _audioSource.Play();
                    break;
            }

            // Finish the sequence if it's not looping
            if (_playbackMode == PlaybackMode.PlayOnce)
            {
                Sequence.Finish(this);
                Sequence.Stop(this);
            }
        }

        protected override UniTask Unload()
        {
            // Stop the audio when unloading
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class SFXHandlerData : SequenceData
    {
        public AudioClip audioClip;
        public AudioSource audioSource;
        public SFXHandler.PlaybackMode playbackMode;
    }
}
