using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox
{
    public class CosmicCrafterToyVisuals : MonoBehaviour
    {
        [SerializeField] VisualEffect visualEffect = null;

        private void Update()
        {
            Vector3 leftHandPosition = XRPlayer.LeftHand.Position;
            Vector3 rightHandPosition = XRPlayer.RightHand.Position;

            visualEffect.SetVector3("LeftHandPosition", leftHandPosition);
            visualEffect.SetVector3("RightHandPosition", rightHandPosition);
        }
    }
}
