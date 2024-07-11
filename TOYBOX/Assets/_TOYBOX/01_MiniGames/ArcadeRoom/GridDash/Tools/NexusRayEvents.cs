using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusRayEvents : NexusBlock
    {
        [SerializeField] float radius = 0.5f;
        [SerializeField] NexusRay inputRay;

        #region Static Properties
        public static bool IsHandInVolume = false;
        #endregion

        public NexusEventBlock OnEnterZone;
        public NexusEventBlock OnExitZone;

        private bool wasHandInVolume = false;

        private void OnDrawGizmos()
        {
            if (inputRay)
            {
                //draw target volume
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, radius);


                //draw ray from input ray   
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(inputRay.origin, inputRay.direction);
                Gizmos.DrawWireSphere(inputRay.origin, 0.1f);


            }
        }

        private void Update()
        {
            UpdateHandInVolumeStatus();
        }

        private void UpdateHandInVolumeStatus()
        {
            IsHandInVolume = IsPosInVolume(inputRay.origin);

            if (IsHandInVolume && !wasHandInVolume)
            {
                Debug.Log("Enter Zone");
                OnEnterZone.InvokeBlock();
            }
            else if (!IsHandInVolume && wasHandInVolume)
            {
                Debug.Log("Exit Zone");
                OnExitZone.InvokeBlock();
            }

            wasHandInVolume = IsHandInVolume;
        }

        public bool IsPosInVolume(Vector3 position)
        {
            return Vector3.Distance(transform.position, position) < radius;
        }
    }
}
