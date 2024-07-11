using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

namespace ToyBox
{
    public class GrabGlowingOrbsTutorialStep : CosmicTutorialStep
    {
        [SerializeField] PointableUnityEventWrapper[] grabbablesToListenTo = null;

        private void Start()
        {
            foreach (var glowingOrb in grabbablesToListenTo)
            {
                glowingOrb.WhenSelect.AddListener(OnGlowingOrbGrabbed);
            }
        }

        private void OnGlowingOrbGrabbed(PointerEvent evt)
        {
            CompleteTutorialStep();

            foreach (var glowingOrb in grabbablesToListenTo)
            {
                glowingOrb.WhenSelect.RemoveListener(OnGlowingOrbGrabbed);
            }
        }
    }
}
