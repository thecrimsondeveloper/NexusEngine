using UnityEngine;

using Toolkit.Playspace;
using Toolkit.XR;

namespace PositionHandling
{
    public class PointPositionSetter : XRPositionSetter
    {

        protected override Vector3 GetPosition()
        {
            if (Type == PositionType.LeftHand)
            {
                return XRPlayer.LeftHand.Position;
            }
            else if (Type == PositionType.RightHand)
            {
                return XRPlayer.RightHand.Position;
            }
            else if (Type == PositionType.Chest)
            {
                return XRPlayer.HeadPose.position;
            }
            else if (Type == PositionType.Gaze)
            {
                return XRPlayer.HeadPose.position + XRPlayer.HeadPose.forward * 2;
            }
            else if (Type == PositionType.Playspace)
            {
                return Playspace.pose.position;
            }

            return Vector3.zero;
        }

        protected override Quaternion GetRotation()
        {
            if (Type == PositionType.LeftHand)
            {
                return XRPlayer.LeftHand.Rotation;
            }
            else if (Type == PositionType.RightHand)
            {
                return XRPlayer.RightHand.Rotation;
            }
            else if (Type == PositionType.Chest)
            {
                return XRPlayer.HeadPose.rotation;
            }

            return Quaternion.identity;
        }
    }
}
