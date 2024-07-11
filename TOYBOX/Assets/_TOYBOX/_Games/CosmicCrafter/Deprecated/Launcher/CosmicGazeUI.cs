using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox.Minigames.CosmicCrafter
{
    public class CosmicGazeUI : MonoBehaviour
    {
        [SerializeField] bool isOnFlatPlane = false;
        public bool canMove = true;
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float hoverDistance = 1f;
        [SerializeField] float maintainDistanceSpeed = 1f;
        [SerializeField] float yOffset = 0f;
        [SerializeField] AnimationCurve speedToDistanceMultiplier;



        Vector3 targetPos = Vector3.zero;


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.02f);

            Gizmos.DrawLine(XRPlayer.HeadPose.position, XRPlayer.HeadPose.position + XRPlayer.HeadPose.forward * hoverDistance);


        }

        private void Update()
        {
            //hover this game object at the forward vector of the player head
            // if (hoverDistance < 0.1f)
            // {
            //     hoverDistance = 0.1f;
            // }

            SetTargetPos();


            //calculate the speed multiplier based on the distance to the player head and use hoverDistance as the max distance for the curve
            float distanceSpeedMultiplier = speedToDistanceMultiplier.Evaluate(Vector3.Distance(transform.position, targetPos) / hoverDistance);

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed * distanceSpeedMultiplier);


            //if distance less than hoverDistance, add movement away from the player head
            if (Vector3.Distance(transform.position, XRPlayer.HeadPose.position) < hoverDistance)
            {
                Vector3 dirAway = (transform.position - XRPlayer.HeadPose.position).normalized;
                transform.Translate(dirAway * maintainDistanceSpeed * Time.deltaTime, Space.World);
            }
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
