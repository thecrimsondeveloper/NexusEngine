using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Minigames.BeatEmUp
{
    public class SpaceBoyExternalSession : ExternalSession
    {
        [SerializeField] SpaceBoyExternalSessionData sessionData = null;
        [SerializeField] Animation animation = null;
        protected override ExternalSessionData ExternalSessionData
        {
            get => sessionData;
            set => sessionData = value as SpaceBoyExternalSessionData;
        }


        public async void PlayExitAnimation()
        {
            animation.Play();
            // Wait for the animation to finish
            await UniTask.WaitWhile(() => animation.isPlaying);
            SessionEnd();
        }

        protected override async UniTask OnUnload()
        {
            await UniTask.CompletedTask;
        }

        protected override void OnSessionEnd()
        {
            Debug.Log("Session Ended");
        }

        protected override void OnSessionStart()
        {
            Debug.Log("Session Started");
        }
    }
}
