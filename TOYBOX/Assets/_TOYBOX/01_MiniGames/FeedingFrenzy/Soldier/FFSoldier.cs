using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Oculus.Interaction.Input;
using Sirenix.OdinInspector;
using Toolkit.XR;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox
{
    public class FFSoldier : NetworkBehaviour
    {
        string playerName = "Player";
        [Title("Soldier BodyParts")]
        [SerializeField] private Rigidbody head;
        [SerializeField] private Rigidbody rightHand;

        [SerializeField] private Rigidbody leftHand;


        bool isRotateReset = true;
        private Vector3 moveDirection = Vector3.zero;

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority == false) return;
            if (HasInputAuthority == false) return;
            FollowPlayerRig(); // Ensure this is called after Move()
        }

        private void FollowPlayerRig()
        {
            transform.position = XRPlayer.Position;
            transform.rotation = XRPlayer.Rotation;

            if (head)
            {
                Vector3 dirToHead = XRPlayer.HeadPose.position - head.position;
                // Smoothly move the head Rigidbody to match the XRPlayer's head position
                head.velocity = dirToHead * 10;
            }

            if (rightHand)
            {
                Vector3 dirToRightHand = XRPlayer.RightHand.Position - rightHand.position;
                // Smoothly move the right hand Rigidbody to match the XRPlayer's right hand position
                rightHand.velocity = dirToRightHand * 10;
            }

            if (leftHand)
            {
                Vector3 dirToLeftHand = XRPlayer.LeftHand.Position - leftHand.position;
                // Smoothly move the left hand Rigidbody to match the XRPlayer's left hand position
                leftHand.velocity = dirToLeftHand * 10;
            }



        }

    }
}
