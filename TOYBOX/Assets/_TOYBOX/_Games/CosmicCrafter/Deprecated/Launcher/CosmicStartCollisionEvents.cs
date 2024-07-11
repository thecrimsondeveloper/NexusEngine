using System.Collections;
using System.Collections.Generic;
using ToyBox.Minigames.CosmicCrafter;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicStartCollisionEvents : MonoBehaviour
    {
        public UnityEvent<CosmicStar> OnCollideWithStar = new UnityEvent<CosmicStar>();

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out CosmicStar star))
            {
                OnCollideWithStar.Invoke(star);
            }
        }
    }
}
