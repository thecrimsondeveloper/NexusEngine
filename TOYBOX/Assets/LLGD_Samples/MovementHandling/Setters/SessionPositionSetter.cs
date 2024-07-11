using System.Collections;
using System.Collections.Generic;
using Extras.Playspace;
using PositionHandling;
using UnityEngine;

namespace Toolkit.Playspace
{
    public class SessionPositionSetter : PositionSetter
    {
        protected override Vector3 GetPosition()
        {
            return Playspace.pose.position + Settings.offset.position;
        }

        protected override Quaternion GetRotation()
        {
            return Playspace.pose.rotation * Settings.offset.rotation;
        }

        protected override void OnUpdateSettings(PositionSettings settings)
        {

        }
    }
}
