using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusRay : NexusBlock
    {

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction);
        }
        public NexusVector3 origin;
        public NexusVector3 direction;
    }
}
