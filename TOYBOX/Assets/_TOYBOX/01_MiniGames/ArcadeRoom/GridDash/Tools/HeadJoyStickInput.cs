using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.Extras
{
    public class HeadJoyStickInput : MonoBehaviour
    {
        [SerializeField] AnimationCurve speedCurve;
        Vector3 headForward;
        Vector3 headRight;
        Vector3 headUp;
        public static Vector3 Value = Vector3.zero;
        public static float HeadTilt = 0;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, headForward * 2);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, headRight * 2);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * 2);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.right * 2);
        }

        private void Update()
        {
            headForward = XRPlayer.HeadPose.forward;
            headRight = XRPlayer.HeadPose.right;
            headUp = XRPlayer.HeadPose.up;

            UpdateHorizontalInput();
            UpdateForwardRotationInput();
            // UpdateTiltInput();
        }

        //look left and right

        void UpdateHorizontalInput()
        {
            float dotWithForward = Vector3.Dot(headForward, transform.forward);
            float dotWithRight = Vector3.Dot(headForward, transform.right);

            if (dotWithForward < 0)
            {
                Value.x = 0;
                return;
            }

            float inputValue = Mathf.Abs(speedCurve.Evaluate(Mathf.Abs(dotWithRight)));

            // Evaluate the sign of dotWithRight to determine whether to use inputValue or -inputValue
            Value.x = Mathf.Sign(dotWithRight) * inputValue;
        }

        //look up and down
        void UpdateForwardRotationInput()
        {
            float dotWithForward = Vector3.Dot(headForward, transform.forward);
            float dotWithUp = Vector3.Dot(headForward, transform.up);

            if (dotWithForward < 0)
            {
                Value.y = 0;
                return;
            }

            float inputValue = Mathf.Abs(speedCurve.Evaluate(Mathf.Abs(dotWithUp)));

            // Evaluate the sign of dotWithUp to determine whether to use inputValue or -inputValue
            Value.y = Mathf.Sign(dotWithUp) * inputValue;
        }


        //tilt head left and right
        void UpdateTiltInput()
        {
            float dotWithRight = Vector3.Dot(headUp, transform.right);
            float dotWithUp = Vector3.Dot(headUp, transform.up);

            //if the head is not facing in the same up direction as the transform
            if (dotWithUp < 0)
            {
                Value.z = 0;
                return;
            }

            float inputValue = Mathf.Abs(speedCurve.Evaluate(Mathf.Abs(dotWithRight)));
            // Evaluate the sign of dotWithRight to determine whether to use inputValue or -inputValue
            HeadTilt = Mathf.Sign(dotWithRight) * inputValue;
        }
    }
}
