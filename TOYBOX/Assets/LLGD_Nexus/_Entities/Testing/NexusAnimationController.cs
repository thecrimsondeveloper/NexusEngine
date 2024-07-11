using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusAnimationController : NexusBlock
    {
        [SerializeField] Animation anim;
        [SerializeField] List<AnimationClip> clips;


        public void Play(int index)
        {
            if (index < clips.Count)
            {
                anim.Play(clips[index].name);
            }
        }

        public void Play(AnimationClip clip)
        {
            if (clips.Contains(clip))
            {
                anim.Play(clip.name);
            }
        }
    }
}
