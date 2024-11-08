using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace LuminaryLabs.NexusEngine
{
    public class BaseVFXHandler : BaseSequence<BaseVFXHandlerData>
    {
        private VisualEffect visualEffect;
        private float effectDuration;

        protected override UniTask Initialize(BaseVFXHandlerData currentData)
        {
            // Set private variables from data
            visualEffect = currentData.visualEffect;
            effectDuration = Mathf.Max(0.1f, currentData.effectDuration); // Clamp to a minimum of 0.1 seconds
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (visualEffect == null)
            {
                Debug.LogWarning("VisualEffect is missing.");
                Complete();
                return;
            }

            // Play the Visual Effect
            visualEffect.Play();

            // Wait for the specified duration
            await UniTask.Delay((int)(effectDuration * 1000));

            // Complete the sequence after the delay
            Complete();
        }
    }

    [System.Serializable]
    public class BaseVFXHandlerData : BaseSequenceData
    {
        [Tooltip("The VisualEffect component to play")]
        public VisualEffect visualEffect;

        [Tooltip("Duration to play the effect (must be greater than zero)")]
        public float effectDuration = 1f;
    }
}
