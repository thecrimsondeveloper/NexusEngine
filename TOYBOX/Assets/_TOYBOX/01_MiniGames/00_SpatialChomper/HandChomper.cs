using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class HandChomper : RangeEvents
    {
        [SerializeField, FoldoutGroup("Hand Chomper")] Rigidbody rb;
        [SerializeField, FoldoutGroup("Hand Chomper")] float closedForce = 10f;
        [SerializeField, FoldoutGroup("Hand Chomper")] float openForce = 10f;
        [SerializeField, FoldoutGroup("Hand Chomper")] float rotationSpeed = 10f;
        [SerializeField, FoldoutGroup("Hand Chomper")] Vector3 rotationOffset = new Vector3(0, 0, 0);
        [SerializeField, FoldoutGroup("Hand Chomper")] Animation animation;

        Pose targetPose;
        private async void Start()
        {
            //wait 3 seconds
            await UniTask.Delay(3000);
            //move the chomper using dotween to the hand
            targetPose = new Pose(XRPlayer.LeftHand.Position, XRPlayer.LeftHand.Rotation);

            transform.DOMove(targetPose.position, 1f);
        }

        float handOpenness = 0;
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            targetPose.position = XRPlayer.LeftHand.Position;
            targetPose.rotation = XRPlayer.LeftHand.Rotation;

            targetPose.rotation *= Quaternion.Euler(rotationOffset);

            Debug.DrawRay(transform.position, targetPose.forward * 10, Color.red, 0.05f);
            Debug.DrawRay(transform.position, transform.forward * 10, Color.blue, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetPose.rotation, Time.deltaTime * rotationSpeed);


            handOpenness = XRPlayer.LeftHand.indexPinchStrength + XRPlayer.LeftHand.middlePinchStrength + XRPlayer.LeftHand.ringPinchStrength + XRPlayer.LeftHand.pinkyPinchStrength;
            handOpenness /= 4;

            SetOpenAmount(handOpenness);
        }

        protected override void OnClosed()
        {
            animation.Play();
        }

        public void BoostForward()
        {
            rb.AddForce(transform.forward * closedForce, ForceMode.Impulse);
        }

        protected override void OnOpened()
        {
            // rb.AddForce(Vector3.forward * openForce, ForceMode.Impulse);
        }
    }
}
