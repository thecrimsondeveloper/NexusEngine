using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    public class CollectStarsTutorialStep : CosmicTutorialStep
    {
        private void Start()
        {
            StarCollectable.OnAnyStarCollected.AddListener(OnStarCollected);
        }

        private void OnStarCollected(StarCollectable star, CosmicStar cosmicStar)
        {
            CompleteTutorialStep();
        }
    }
}
