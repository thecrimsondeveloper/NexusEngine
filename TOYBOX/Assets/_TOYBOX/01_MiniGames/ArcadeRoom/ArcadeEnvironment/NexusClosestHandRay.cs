using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusClosestHandRay : NexusRay
    {
        public void Update()
        {
            if (entity == null)
            {
                return;
            }
            Vector3 leftHandPosition = XRPlayer.LeftHand.Position;
            Vector3 rightHandPosition = XRPlayer.RightHand.Position;

            float leftHandDistance = Vector3.Distance(leftHandPosition, entity.transform.position);
            float rightHandDistance = Vector3.Distance(rightHandPosition, entity.transform.position);

            if (leftHandDistance < rightHandDistance)
            {
                origin.Set(leftHandPosition);
            }
            else
            {
                origin.Set(rightHandPosition);
            }
        }
    }
}
