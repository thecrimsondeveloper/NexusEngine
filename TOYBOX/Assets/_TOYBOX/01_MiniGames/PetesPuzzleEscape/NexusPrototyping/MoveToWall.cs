using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "SetPositionToWall", menuName = "Nexus/Actions/SetPositionToWall")]
    public class MoveToWall : NexusAction
    {
        public enum Target
        {
            Index,
            Random,
            Player,
        }

        public enum Type
        {
            Farthest,
            Closest
        }

        [SerializeField] private Target target = Target.Index;


        [SerializeField, ShowIf(nameof(target), Target.Index)] private int wallIndex = 0;
        [SerializeField, ShowIf(nameof(target), Target.Player)] private Type type = Type.Farthest;




        private void MoveToWallIndex(MonoBehaviour mono)
        {
            if (XRPlayspace.Info.TryGetWallPose(wallIndex, out var pose))
            {
                mono.transform.position = pose.position;
                mono.transform.rotation = pose.rotation;
            }
        }

        private void MoveToPlayer(MonoBehaviour mono)
        {
            if (type == Type.Farthest)
            {
                Pose pose = XRPlayspace.Info.GetFarthestWallPose(XRPlayer.HeadPose.position);
                mono.transform.position = pose.position;
                mono.transform.rotation = pose.rotation;
            }
            else if (type == Type.Closest)
            {
                Pose pose = XRPlayspace.Info.GetClosestWallPose(XRPlayer.HeadPose.position);
                mono.transform.position = pose.position;
                mono.transform.rotation = pose.rotation;
            }
        }

        private void MoveToRandomWall(MonoBehaviour mono)
        {
            mono.transform.position = XRPlayspace.Info.RandomWallPosition;
        }

        public override void SetupAction()
        {

        }

        public override void OnExecute()
        {
            if (target == Target.Index)
            {
                MoveToWallIndex(mono);
            }
            else if (target == Target.Player)
            {
                MoveToPlayer(mono);
            }
            else if (target == Target.Random)
            {
                MoveToRandomWall(mono);
            }
        }
    }
}
