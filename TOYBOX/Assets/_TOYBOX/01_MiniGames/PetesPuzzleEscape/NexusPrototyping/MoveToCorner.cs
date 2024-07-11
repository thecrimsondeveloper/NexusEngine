using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "MoveToCorner", menuName = "Nexus/Actions/Move To Corner")]
    public class MoveToCorner : NexusAction
    {
        public enum Type
        {
            index,
            farthest,
            closest
        }

        public enum DistanceTarget
        {
            Tag,
            Player,
            NexusPosition
        }

        public Type type = Type.index;


        [ShowIf(nameof(type), Type.farthest)]
        public DistanceTarget distanceTarget = DistanceTarget.Player;


        [ShowIf(nameof(distanceTarget), DistanceTarget.Tag)]
        public string tag = "";

        [ShowIf(nameof(type), Type.index)]
        public int cornerIndex = 0;

        public override void SetupAction()
        {

        }

        public override void OnExecute()
        {
            void MoveTo(Vector3 position)
            {
                mono.transform.position = position;
            }

            if (type == Type.index)
            {
                if (XRPlayspace.Info.TryGetTopCornerPosition(0, out var position))
                {
                    MoveTo(position);
                }
            }
            else if (type == Type.farthest)
            {
                if (distanceTarget == DistanceTarget.Player)
                {
                    MoveTo(XRPlayspace.Info.GetFarthestTopCornerFrom(XRPlayer.HeadPose.position));
                }
                else if (distanceTarget == DistanceTarget.Tag)
                {
                    MoveTo(XRPlayspace.Info.GetFarthestTopCornerFrom(GameObject.FindWithTag(tag).transform.position));
                }
            }
            else if (type == Type.closest)
            {
                if (distanceTarget == DistanceTarget.Player)
                {
                    MoveTo(XRPlayspace.Info.GetClosestTopCornerFrom(XRPlayer.HeadPose.position));
                }
                else if (distanceTarget == DistanceTarget.Tag)
                {
                    MoveTo(XRPlayspace.Info.GetClosestTopCornerFrom(GameObject.FindWithTag(tag).transform.position));
                }
                else if (distanceTarget == DistanceTarget.NexusPosition)
                {
                    MoveTo(XRPlayspace.Info.GetClosestTopCornerFrom(mono.transform.position));
                }
            }
        }

    }


}

