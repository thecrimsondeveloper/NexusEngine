using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    public class HitBlackHoleTutorialStep : CosmicTutorialStep
    {
        [SerializeField] CosmicBlackHole blackHole;
        private void Start()
        {
            blackHole.OnHitByStar.AddListener(OnHitByStar);
        }

        private void OnHitByStar(CosmicStar star)
        {
            Debug.Log("HitBlackHoleTutorialStep: Hit by star");
            CompleteTutorialStep();
        }
    }
}
