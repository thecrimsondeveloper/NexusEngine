using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.Stories
{
    [CreateAssetMenu(fileName = "StoryAnimationStep", menuName = "Toolkit/Story/Story Animation Step")]
    public class StoryAniationStep : StoryStep
    {
        [SerializeField] Animation storyController = null;
        [SerializeField] public AnimationClip storyClip = null;



        public override void OnSequenceLoad()
        {

        }

        public override void OnStepStart()
        {
            storyController.clip = storyClip;
            storyController.Play();
        }

        public override void OnStepEnd()
        {

        }

        public override UniTask LoadStep()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask UnloadStep()
        {
            return UniTask.CompletedTask;
        }
    }
}
