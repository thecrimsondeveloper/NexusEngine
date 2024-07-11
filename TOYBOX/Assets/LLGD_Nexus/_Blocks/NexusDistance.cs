using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusDistance : NexusFloat
    {
        float lastValue = 0;
        public override float value
        {
            get
            {
                lastValue = Vector3.Distance(pointA, pointB);
                return lastValue;
            }
            protected set
            {
                base.value = value;
            }
        }
        public NexusVector3 pointA;
        public NexusVector3 pointB;
    }
}
