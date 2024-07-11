using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class ScoreCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PipeConfiguration pipeConfiguration))
            {
                FloppyBird.ScoreUI.IncrementScore(1);
            }
        }
    }
}
