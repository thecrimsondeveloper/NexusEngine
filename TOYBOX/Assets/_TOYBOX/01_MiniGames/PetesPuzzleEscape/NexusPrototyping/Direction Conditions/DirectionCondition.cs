using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "DirectionCondition", menuName = "Nexus/Conditions/Direction Condition")]
    public class DirectionCondition : NexusCondition
    {
        public enum Direction
        {
            PlayerFacing,
            PlayerLeftPalm,
            PlayerRightPalm,
            PlayerLeftFinderTip,
            PlayerRightFinderTip,
            PlayerLeftThumb,
            PlayerRightThumb,

        }
        public enum ConditionType
        {
            LookingAt,
            LookingAway,
        }
        public ConditionType conditionType;
        public Direction source;


        [Range(-1, 1)]
        public float dotThreshold = 0.5f;


        public override bool IsConditionMet(MonoBehaviour mono = null)
        {
            bool IsLookingAt(Vector3 position, Vector3 direction)
            {
                var forward = direction;
                var toTarget = mono.transform.position - position;

                Debug.DrawRay(position, forward, Color.blue, 1);
                toTarget.Normalize();
                return Vector3.Dot(forward, toTarget) > dotThreshold;
            }

            Vector3 GetPosition(Direction target)
            {
                switch (target)
                {
                    case Direction.PlayerFacing:
                        return XRPlayer.HeadPose.position;
                    case Direction.PlayerLeftPalm:
                        return XRPlayer.LeftHand.Position;
                    case Direction.PlayerRightPalm:
                        return XRPlayer.RightHand.Position;
                    case Direction.PlayerLeftFinderTip:
                        return XRPlayer.LeftHand.Position;
                    case Direction.PlayerRightFinderTip:
                        return XRPlayer.RightHand.Position;
                    case Direction.PlayerLeftThumb:
                        return XRPlayer.LeftHand.Position;
                    case Direction.PlayerRightThumb:
                        return XRPlayer.RightHand.Position;
                }
                return Vector3.zero;
            }

            Vector3 GetDir(Direction target)
            {
                switch (target)
                {
                    case Direction.PlayerFacing:
                        return XRPlayer.HeadPose.forward;
                    case Direction.PlayerLeftPalm:
                        return XRPlayer.LeftHand.PalmDirection;
                    case Direction.PlayerRightPalm:
                        return XRPlayer.RightHand.PalmDirection;
                    case Direction.PlayerLeftFinderTip:
                        return XRPlayer.LeftHand.FingerTipDirection;
                    case Direction.PlayerRightFinderTip:
                        return XRPlayer.RightHand.FingerTipDirection;
                    case Direction.PlayerLeftThumb:
                        return XRPlayer.LeftHand.ThumbDirection;
                    case Direction.PlayerRightThumb:
                        return XRPlayer.RightHand.ThumbDirection;
                }
                return Vector3.zero;
            }
            if (mono == null)
            {
                return false;
            }

            if (IsLookingAt(GetPosition(source), GetDir(source)))
            {
                return conditionType == ConditionType.LookingAt;
            }
            else
            {
                return conditionType == ConditionType.LookingAway;
            }
            return false;
        }




    }
}
