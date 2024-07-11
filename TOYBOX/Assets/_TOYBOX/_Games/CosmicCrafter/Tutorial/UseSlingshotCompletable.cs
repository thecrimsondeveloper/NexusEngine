using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.CosmicCrafter
{
    public class UseSlingshotCompletable : CosmicTutorialStep
    {
        private void Start()
        {
            StarSlingshot.OnStartPull.AddListener(OnStartPull);
        }

        private void OnStartPull(CosmicStar star)
        {
            CompleteTutorialStep();
        }
    }
}
