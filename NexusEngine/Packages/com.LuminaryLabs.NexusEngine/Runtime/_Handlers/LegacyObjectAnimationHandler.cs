using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class LegacyObjectAnimationHandler : EntitySequence<LegacyObjectAnimationHandlerData>
    {
        private Animation animationComponent;
        private bool addedAnimationComponent = false;

        protected override UniTask Initialize(LegacyObjectAnimationHandlerData currentData)
        {
            // Get or add the legacy Animation component to the target object
            animationComponent = currentData.targetObject.GetComponent<Animation>();

            if (animationComponent == null)
            {
                animationComponent = currentData.targetObject.AddComponent<Animation>();
                addedAnimationComponent = true;
            }

            // Add the animation clip to the animation component
            if (currentData.animationClip != null)
            {
                animationComponent.AddClip(currentData.animationClip, currentData.animationClip.name);
            }

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Play the specified animation clip
            if (animationComponent != null && currentData.animationClip != null)
            {
                animationComponent.Play(currentData.animationClip.name);
            }
            else
            {
                Complete();
            }
        }

        void Update()
        {
            // Check if the animation has finished playing
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
            // If the Animation component was added by this handler, remove it
            if (addedAnimationComponent && animationComponent != null)
            {
                Destroy(animationComponent);
            }

            // Clean up by destroying the GameObject when the sequence is done
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class LegacyObjectAnimationHandlerData : SequenceData
    {
        public GameObject targetObject;    // The object to which the Animation component will be added
        public AnimationClip animationClip;  // The animation clip to play
    }
}
