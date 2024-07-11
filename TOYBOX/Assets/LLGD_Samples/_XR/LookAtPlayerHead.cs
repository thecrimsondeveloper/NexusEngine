using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.Extras
{
    public class LookAtPlayerHead : LookAt
    {
        protected override Pose targetPose { get; set; }


        public override void OnDataUpdate()
        {
            targetPose = new Pose(XRPlayer.HeadPose.position, XRPlayer.HeadPose.rotation);
        }
    }
}
