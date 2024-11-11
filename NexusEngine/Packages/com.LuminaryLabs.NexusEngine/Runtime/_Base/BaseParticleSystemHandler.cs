using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseParticleSystemHandler : BaseSequence<BaseParticleSystemHandlerData>
{
        private ParticleSystem particleSystemToPlay;
        private float overrideDuration = -1;
        private ParticleSystem.MainModule mainModule;

        protected override UniTask Initialize(BaseParticleSystemHandlerData currentData)
        {
            // Set private variables from data
            particleSystemToPlay = currentData.particleSystemToPlay;
            overrideDuration = currentData.overrideDuration;
            mainModule = particleSystemToPlay.main;
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (particleSystemToPlay == null)
            {
                Debug.LogWarning("Particle System is missing.");
                Complete();
                return;
            }

            // Play the Particle System
            particleSystemToPlay.Play();

            // Wait for the specified duration

          

            await UniTask.Delay((int)(overrideDuration == -1 ? mainModule.duration : overrideDuration * 1000));

            // Complete the sequence after the delay
            Complete();
        }
}

[System.Serializable]
public class BaseParticleSystemHandlerData : BaseSequenceData
{
    public ParticleSystem particleSystemToPlay;
    public float overrideDuration = -1;
}
