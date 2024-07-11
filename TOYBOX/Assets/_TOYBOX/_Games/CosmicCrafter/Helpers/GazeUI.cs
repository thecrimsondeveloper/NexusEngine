using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ToyBox
{
    public class GazeUI : MonoBehaviour
    {
        Vector3 targetPosition = Vector3.zero;

        [SerializeField] Vector3 localPositionOffset = new Vector3(0, -1, 0);
        [SerializeField] float snapDistance = 2f;
        [SerializeField] bool enableSnap = true;

        // Update is called once per frame
        Pose headPose;
        void FixedUpdate()
        {
            headPose = XRPlayer.HeadPose;

            targetPosition = headPose.position + headPose.forward * snapDistance;

            targetPosition += headPose.right * localPositionOffset.x;
            targetPosition += headPose.up * localPositionOffset.y;
            targetPosition += headPose.forward * localPositionOffset.z;

            if (enableSnap)
            {
                float distance = Vector3.Distance(transform.position, targetPosition);
                if (distance <= snapDistance)
                {
                    transform.position = targetPosition;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
            }
        }

        private void OnEnable()
        {
            transform.position = targetPosition;
        }

        private void OnDisable()
        {
            transform.position = Vector3.zero;
        }
    }
}
