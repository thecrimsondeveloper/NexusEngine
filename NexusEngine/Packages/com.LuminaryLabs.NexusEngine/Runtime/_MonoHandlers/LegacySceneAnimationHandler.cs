using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class LegacySceneAnimationHandler : EntitySequence<LegacySceneAnimationHandlerData>
    {
        private Animation animationComponent;
        private AnimationClip animationClip;  // Store the animation clip as a private variable

        protected override UniTask Initialize(LegacySceneAnimationHandlerData currentData)
        {
            // Use the provided Animation component from the current data
            animationComponent = currentData.animationComponent;
            animationClip = currentData.animationClip;

            if (animationComponent == null)
            {
                Debug.LogError("Animation component is null.");
            }

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Play the stored animation clip
            if (animationComponent != null && animationClip != null)
            {
                animationComponent.clip = animationClip;
                animationComponent.Play();
            }
            else
            {
                Complete();
            }
        }

        void Update()
        {
            // Check if the animation has finished
            if (animationComponent != null && !animationComponent.isPlaying)
            {
                Complete();
            }
        }

        private void Complete()
        {
            Sequence.Finish(this).Forget();
            Sequence.Stop(this).Forget();
        }

        protected override UniTask Unload()
        {
            // No need to destroy any GameObjects or components
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class LegacySceneAnimationHandlerData : SequenceData
    {
        public Animation animationComponent;  // The Animation component to play the clip on
        public AnimationClip animationClip;   // The animation clip to play
    }
}
