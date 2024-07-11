using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class HippoHand : MonoBehaviour
    {

        [SerializeField] Transform bottomJaw;
        [SerializeField] Transform topJaw;

        [SerializeField] bool isOpened = false;


        private void OnValidate()
        {
            RefreshState();
        }


        [SerializeField] Pose topJawOpenPose;
        [SerializeField] Pose bottomJawOpenPose;
        [SerializeField] Pose topJawClosedPose;
        [SerializeField] Pose bottomJawClosedPose;

        private void Update()
        {
            // if (XRPlayer.LeftHand.GetAveragePinchStrength() > 0.5f)
            // {
            //     isOpened = false;
            // }
            // else
            // {
            //     isOpened = true;
            // }
        }


        void RefreshState()
        {
            if (isOpened)
            {
                topJaw.localPosition = topJawOpenPose.position;
                topJaw.localRotation = topJawOpenPose.rotation;

                bottomJaw.localPosition = bottomJawOpenPose.position;
                bottomJaw.localRotation = bottomJawOpenPose.rotation;
            }
            else
            {
                topJaw.localPosition = topJawClosedPose.position;
                topJaw.localRotation = topJawClosedPose.rotation;

                bottomJaw.localPosition = bottomJawClosedPose.position;
                bottomJaw.localRotation = bottomJawClosedPose.rotation;
            }
        }
    }
}
