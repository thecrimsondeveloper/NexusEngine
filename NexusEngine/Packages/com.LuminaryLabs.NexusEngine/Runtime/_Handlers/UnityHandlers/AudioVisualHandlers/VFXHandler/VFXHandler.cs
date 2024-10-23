using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class VFXHandler : EntitySequence<VFXHandlerData>
    {
        public enum PlaybackMode
        {
            PlayForDuration,
            Endless
        }

        private VisualEffect _vfx;
        private PlaybackMode _playbackMode;
        private float _duration;

        protected override UniTask Initialize(VFXHandlerData currentData)
        {
            // Assign VFX, duration, and playback mode
            _vfx = currentData.visualEffect;
            _duration = currentData.duration;
            _playbackMode = currentData.playbackMode;

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (_vfx == null)
            {
                Sequence.Stop(this);
                return;
            }

            // Play the VFX
            _vfx.Play();

            // Handle the playback based on the mode
            switch (_playbackMode)
            {
                case PlaybackMode.PlayForDuration:
                    // Wait for the duration to finish
                    await UniTask.Delay((int)(_duration * 1000));
                    _vfx.Stop();
                    Sequence.Finish(this);
                    Sequence.Stop(this);
                    break;

                case PlaybackMode.Endless:
                    // VFX runs endlessly until manually stopped
                    break;
            }
        }

        protected override UniTask Unload()
        {
            // Stop the VFX if it is still playing
            if (_vfx != null && _vfx.isActiveAndEnabled)
            {
                _vfx.Stop();
            }

            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class VFXHandlerData : SequenceData
    {
        public VisualEffect visualEffect;
        public float duration;
        public VFXHandler.PlaybackMode playbackMode;
    }
}
