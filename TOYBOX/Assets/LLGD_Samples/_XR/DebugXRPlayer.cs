using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.XR
{
    public class DebugXRPlayer : XRPlayer
    {
        protected override void FixedUpdate()
        {


            HeadPose = new Pose(head.position, head.rotation);
            NeckPose = new Pose(HeadPose.position + (Vector3.down * settings.neckHeight), HeadPose.rotation);
            FixedUpdateHand(LeftHand, leftHand);
            FixedUpdateHand(RightHand, rightHand);
            // if (leftHandOVR == null || rightHandOVR == null)
            // {
            // }
            // else
            // {
            //     UpdateHandValues(LeftHand, leftHand, leftHandOVR);
            //     UpdateHandValues(RightHand, rightHand, rightHandOVR);
            // }

        }
    }
}
