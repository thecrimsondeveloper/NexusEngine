using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using ToyBox.Minigames.CosmicCrafter;
using UnityEngine;

using DG.Tweening;

namespace ToyBox
{
    public class CosmicLauncherDesk : MonoBehaviour
    {
        [SerializeField] bool isOnFlatPlane = false;
        public bool canMove = true;
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float hoverDistance = 1f;
        [SerializeField] float maintainDistanceSpeed = 1f;
        [SerializeField] float yOffset = 0f;
        [SerializeField] Rigidbody rb = null;



        Vector3 targetPos = Vector3.zero;


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.02f);

            Gizmos.DrawLine(XRPlayer.HeadPose.position, XRPlayer.HeadPose.position + XRPlayer.HeadPose.forward * hoverDistance);


        }

        private void Update()
        {
            if (rb == null)
            {
                return;
            }
            //hover this game object at the forward vector of the player head
            // if (hoverDistance < 0.1f)
            // {
            //     hoverDistance = 0.1f;
            // }

            SetTargetPos();


            //set the velocity of the rigidbody to move towards the target position
            rb.velocity = (targetPos - transform.position) * moveSpeed;
        }

        void SetTargetPos()
        {
            if (isOnFlatPlane)
            {
                targetPos = XRPlayer.HeadPose.position + XRPlayer.HeadPose.forward * hoverDistance;
                targetPos.y = XRPlayer.HeadPose.position.y + yOffset;
            }
            else
            {
                targetPos = XRPlayer.HeadPose.position + XRPlayer.HeadPose.forward * hoverDistance;
                targetPos.y = XRPlayer.HeadPose.position.y + yOffset;
            }
        }
    }
}

