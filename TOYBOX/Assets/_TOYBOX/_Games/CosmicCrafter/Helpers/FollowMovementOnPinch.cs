using System;
using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class FollowMovementOnPinch : MonoBehaviour
    {
        Vector3 startPosition = Vector3.zero;
        public bool isPinching = false;

        Vector3 targetPosition = Vector3.zero;
        float reactiveMultiplier = 1;

        private void Start()
        {
            startPosition = transform.position;
            targetPosition = transform.position;
        }

        void FixedUpdate()
        {
            //if left hand is pinching
            bool isLeftPinching = XRPlayer.LeftHand.indexPinchStrength > 0.9;
            //if right hand is pinching
            bool isRightPinching = XRPlayer.RightHand.indexPinchStrength > 0.9;

            isPinching = isLeftPinching || isRightPinching;

            Vector3 movementDelta = Vector3.zero;
            //if left hand is pinching

            reactiveMultiplier = MathF.Pow(1 + (XRPlayer.LeftHand.velocity.magnitude / 5), 2);
            Vector3 leftHandReactiveDelta = (XRPlayer.LeftHand.fixedDeltaPosition * reactiveMultiplier);
            reactiveMultiplier = MathF.Pow(1 + (XRPlayer.RightHand.velocity.magnitude / 5), 2);
            Vector3 rightHandReactiveDelta = (XRPlayer.RightHand.fixedDeltaPosition * reactiveMultiplier);

            movementDelta += isLeftPinching ? leftHandReactiveDelta : Vector3.zero;
            movementDelta += isRightPinching ? rightHandReactiveDelta : Vector3.zero;

            //clamp the movement delta
            movementDelta = Vector3.ClampMagnitude(movementDelta, 0.1f);

            Vector3 directionToStart = startPosition - transform.position;

            float dotToStart = Vector3.Dot(directionToStart.normalized, movementDelta.normalized);
            float distanceToStart = directionToStart.magnitude;
            Debug.Log(distanceToStart + " " + dotToStart);
            if (distanceToStart > 10)
            {
                //if the dir is facing the start position
                bool pullingAwayFromStart = dotToStart < 0;

                //if delta dir is facing the start position and the distance is greater than 5 
                if (pullingAwayFromStart)
                {

                    movementDelta *= dotToStart;
                    Debug.Log("pulling away from start: " + movementDelta + " " + dotToStart + " " + distanceToStart + " " + directionToStart.normalized + " " + movementDelta.normalized);
                }

            }

            Debug.DrawLine(startPosition, transform.position, Color.red, 0.1f);

            if (isPinching)
            {
                AddPosition(movementDelta);
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 20);
            }
        }

        void AddPosition(Vector3 position)
        {
            targetPosition += position;
        }



    }
}
