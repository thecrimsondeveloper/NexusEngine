using System;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox.Games.SunshineAndRainbows
{
    public class SunshineAndRainbowsSequence : MonoSequence
    {
        [SerializeField] private Animation animation;
        [SerializeField] private Transform wheelChairSeat;

        protected override UniTask Finish()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            return UniTask.Delay(1000)
                .ContinueWith(() =>
                {
                    XRPlayer.DisablePassthrough();
                    XRPlayer.SetParent(wheelChairSeat, false, false);
                    return UniTask.Delay(1000);
                });
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Logic to execute after load
        }

        protected override void OnStart()
        {
            animation.Play();
        }

        protected override void OnFinished()
        {
            // Logic to execute when sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic to execute on sequence unload
        }
    }
}
