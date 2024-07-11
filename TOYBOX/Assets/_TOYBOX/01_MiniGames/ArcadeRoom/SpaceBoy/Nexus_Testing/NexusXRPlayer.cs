using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusXRPlayer : NexusBlock
    {
        bool isPinching = false;
        bool wasPinchingLastFrame = false;
        public NexusRay LeftHandRayToUpdate;
        public NexusRay RightHandRayToUpdate;
        public NexusEventBlock OnPinch = new NexusEventBlock();
        public NexusEventBlock OnUnpinch = new NexusEventBlock();



        private void Update()
        {
            float pinchValue = Mathf.Max(XRPlayer.LeftHand.indexPinchStrength, XRPlayer.RightHand.indexPinchStrength);
            isPinching = pinchValue > 0.8f;

            if (isPinching && !wasPinchingLastFrame)
            {
                Debug.Log("Pinch");
                OnPinch.InvokeBlock();
            }
            else if (!isPinching && wasPinchingLastFrame)
            {
                Debug.Log("Unpinch");
                OnUnpinch.InvokeBlock();
            }

            wasPinchingLastFrame = isPinching;




        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        void FixedUpdate()
        {
            if (LeftHandRayToUpdate != null)
            {
                LeftHandRayToUpdate.origin.Set(XRPlayer.LeftHand.Position);
                LeftHandRayToUpdate.direction.Set(XRPlayer.LeftHand.FingerTipDirection);
            }

            if (RightHandRayToUpdate != null)
            {
                RightHandRayToUpdate.origin.Set(XRPlayer.RightHand.Position);
                RightHandRayToUpdate.direction.Set(XRPlayer.RightHand.FingerTipDirection);
            }
        }
    }
}
