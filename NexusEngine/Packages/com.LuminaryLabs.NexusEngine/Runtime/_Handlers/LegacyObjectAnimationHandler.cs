using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class LegacyObjectAnimationHandler : EntitySequence<LegacyObjectAnimationHandlerData>
    {
        private Animation animationComponent;
        private bool addedAnimationComponent = false;

        public GameObject targetObject;    // The object to which the Animation component will be added
        public AnimationClip animationClip;  // The animation clip to play

        protected override UniTask Initialize(LegacyObjectAnimationHandlerData currentData)
        {
            //set the target object and animation clip
            targetObject = currentData.targetObject;
            animationClip = currentData.animationClip;

            // Get or add the legacy Animation component to the target object
            animationComponent = targetObject.GetComponent<Animation>();

            if (animationComponent == null)
            {
                animationComponent = targetObject.AddComponent<Animation>();
                addedAnimationComponent = true;
            }

            // Add the animation clip to the animation component
            if (currentData.animationClip != null)
            {
                animationComponent.AddClip(animationClip, animationClip.name);

            }



            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Play the specified animation clip
            if (animationComponent != null && animationClip != null)
            {
                animationComponent.Play(animationClip.name);
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
