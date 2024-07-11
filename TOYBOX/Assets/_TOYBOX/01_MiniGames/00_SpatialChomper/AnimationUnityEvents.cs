using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class AnimationUnityEvents : MonoBehaviour
    {
        [SerializeField] List<UnityEvent> animationEvents = new List<UnityEvent>();
        public void PlayAnimationEvent(int index)
        {
            if (index < animationEvents.Count)
            {
                animationEvents[index].Invoke();
            }
        }

        public void PlayEvent()
        {
            if (animationEvents.Count > 0)
                animationEvents[0].Invoke();
        }
    }
}
