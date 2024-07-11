using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusHandReference : NexusBlock
    {
        public enum HandType
        {
            Left,
            Right
        }
        public HandType handType;
        public NexusRay directionToSet;

        void Update()
        {
            if (handType == HandType.Left)
            {
                directionToSet.origin.Set(XRPlayer.LeftHand.Position);
                directionToSet.direction.Set(XRPlayer.LeftHand.FingerTipDirection);
            }
            else
            {
                directionToSet.origin.Set(XRPlayer.RightHand.Position);
                directionToSet.direction.Set(XRPlayer.RightHand.FingerTipDirection);
            }
        }
    }
}
