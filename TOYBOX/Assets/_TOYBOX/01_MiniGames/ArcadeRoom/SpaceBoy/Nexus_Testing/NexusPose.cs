using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusPose : NexusPrimitive<Pose>
    {

        public override Pose value { get; protected set; }

        public Vector3 Position
        {
            get { return value.position; }
            set
            {
                Pose pose = this.value;
                pose.position = value;
                this.value = pose;
            }
        }

        public Quaternion Rotation
        {
            get { return value.rotation; }
            set
            {
                Pose pose = this.value;
                pose.rotation = value;
                this.value = pose;
            }
        }
    }

}
