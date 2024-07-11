using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class PinchDetector : MonoBehaviour
    {
        bool isPinching = false;
        bool wasPinchingLastFrame = false;
        public UnityEvent OnPinch = new UnityEvent();
        public UnityEvent OnUnpinch = new UnityEvent();



        private void Update()
        {
            float pinchValue = Mathf.Max(XRPlayer.LeftHand.indexPinchStrength, XRPlayer.RightHand.indexPinchStrength);
            isPinching = pinchValue > 0.8f;

            if (isPinching && !wasPinchingLastFrame)
            {
                Debug.Log("Pinch");
                OnPinch.Invoke();
            }
            else if (!isPinching && wasPinchingLastFrame)
            {
                Debug.Log("Unpinch");
                OnUnpinch.Invoke();
            }

            wasPinchingLastFrame = isPinching;
        }
    }
}
