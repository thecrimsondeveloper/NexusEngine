using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction.Samples;
using OVR.OpenVR;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicOrbDirector : MonoBehaviour
    {
        [Title("Dependencies")]
        [SerializeField, Required] Transform target;
        [SerializeField, Required] GameObject arrowVisual;

        [Title("Settings")]
        [SerializeField] float activationThreshold = 30f;

        public UnityEvent OnLookAtTarget;
        public UnityEvent OnLookAwayFromTarget;

        private bool shouldFlip;

        private bool shouldShowArrow = false;

        public bool ShouldShowArrow
        {
            get => shouldShowArrow;
            set
            {
                shouldShowArrow = value;
            }
        }

        void Update()
        {
            if (shouldShowArrow)
                PointTowardsTarget(target);
        }

        void PointTowardsTarget(Transform target)
        {
            if (target == null)
            {
                Debug.LogWarning("No target assigned to CosmicOrbDirector, please assign a target in the inspector.");
                return;
            }

            Vector3 directionToTarget = target.position - arrowVisual.transform.position;
            Vector3 headDirectionToTarget = target.position - XRPlayer.HeadPose.position;
            Vector3 headForward = XRPlayer.HeadPose.forward;

            float lookAngle = Vector3.Angle(headForward, headDirectionToTarget);



            //determine if the arrow should be visible or hidden
            if (lookAngle > activationThreshold)
            {
                LookAwayFromTarget();
                OnLookAwayFromTarget.Invoke();
            }
            else
            {
                LookAtTarget();
                OnLookAtTarget.Invoke();
            }

            //follow the player's head position at the specified offset by lerping
            Vector3 targetPosition = XRPlayer.HeadPose.position + XRPlayer.HeadPose.forward * (target.position - XRPlayer.HeadPose.position).magnitude;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);

            //calculate the signed angle to determine the arrow's rotation
            float signedAngle = Vector3.SignedAngle(headForward, directionToTarget, Vector3.up);
            shouldFlip = signedAngle < 0;
            signedAngle = shouldFlip ? signedAngle + 180 : signedAngle;
            arrowVisual.transform.rotation = Quaternion.Euler(0, signedAngle, 0);
        }

        void LookAtTarget()
        {
            arrowVisual.SetActive(false);
        }

        void LookAwayFromTarget()
        {
            arrowVisual.SetActive(true);
        }
    }
}
