using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    public class UseGravityPadTutorialStep : CosmicTutorialStep
    {
        [SerializeField] CosmicGravityPad gravityPad = null;
        // Start is called before the first frame update
        void Start()
        {
            gravityPad.OnCollideWithStar.AddListener(OnStarEnter);
        }

        // Update is called once per frame  
        void OnStarEnter(CosmicStar star)
        {
            DetachObjects();
            CompleteTutorialStep();
        }
    }
}
