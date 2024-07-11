using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using ToyBox.Minigames.FeedingFrenzy;
using UnityEngine;

namespace ToyBox
{
    public class Eater : MonoBehaviour
    {
        public float detectionRange = 1.0f;
        public float eatRate = 1.0f;
        public TriggerDetector triggerDetector;

        public void Eat()
        {
            triggerDetector.ForEach((entity) =>
            {
                if (entity is Eated eated)
                {
                    eated.Eat(this);
                }
            });
        }
    }
}
