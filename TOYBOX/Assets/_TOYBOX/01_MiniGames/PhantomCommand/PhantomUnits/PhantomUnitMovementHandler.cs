using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomUnitMovementHandler : MonoBehaviour
    {

        enum MoveType
        {
            Destination,
            Target,
            Idle
        }

        [Title("State")]
        [SerializeField] MoveType moveType;
        [SerializeField] bool overrideFollowWhenSetDestination = false;

        [Title("NavMesh")]
        [SerializeField] NavMeshAgent agent;

        [Title("Character Controller")]
        [SerializeField] CharacterController characterController;
        [SerializeField] float speedMultiplier = 1;

        [LabelText("Speed Over Slope (-90 to 90 degrees)")]
        [SerializeField] AnimationCurve speedOverSlope;
        [SerializeField] float rotationSpeed = 1;
        [SerializeField] float currentInclineAngle = 0;
        [SerializeField] float additionAmount = 0;

        PhantomUnitMovementHandler unitToFollow;

        float currentSpeed;

        public void SetTargetPosition(Vector3 targetPosition)
        {
            if (overrideFollowWhenSetDestination && moveType == MoveType.Target)
            {
                StopFollowing();
            }
            else if (moveType == MoveType.Target)
            {
                return;
            }

            agent.SetDestination(targetPosition);
            moveType = MoveType.Destination;
        }

        public void SetUnitToFollow(PhantomUnitMovementHandler unitToFollow)
        {
            this.unitToFollow = unitToFollow;
            moveType = MoveType.Target;
            // Follow unitToFollow
            agent.enabled = false;
            characterController.enabled = true;
        }

        public void StopFollowing()
        {
            // Stop following
            agent.enabled = true;
            characterController.enabled = false;

            //set the move type to idle
            moveType = MoveType.Idle;
        }

        public void StopAllMovement()
        {
            agent.isStopped = true;
            characterController.enabled = false;
            moveType = MoveType.Idle;
        }

        void FixedUpdate()
        {
            RefreshInclineAngle();


            if (moveType == MoveType.Target)
            {
                if (unitToFollow == null)
                {
                    StopAllMovement();
                    return;
                }
                Vector3 moveDir = transform.forward;

                float angle = Vector3.Angle(Vector3.up, transform.forward);

                currentSpeed = speedMultiplier * speedOverSlope.Evaluate(angle);
                CollisionFlags flags = characterController.Move(moveDir * Time.deltaTime * currentSpeed);
                switch (flags)
                {
                    case CollisionFlags.Below:
                        OnHitBottom();
                        break;
                    case CollisionFlags.Above:
                        OnHitTop();
                        break;
                    case CollisionFlags.Sides:
                        OnHitSides();
                        break;
                }

                //rotate towards target
                Vector3 targetDir = unitToFollow.transform.position - transform.position;

                //lerp the rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * rotationSpeed);

                //zero the x and z rotation
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            else if (moveType == MoveType.Destination)
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StopAllMovement();
                }
            }
        }

        void OnHitBottom()
        {

        }

        void RefreshInclineAngle()
        {
            // get the slope angle
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
            {
                // get the normal of the surface
                Vector3 normal = hit.normal;

                // Project the normal onto the plane defined by the right vector
                Vector3 forward = transform.forward;
                Plane forwardPlane = new Plane(Vector3.up, transform.position);

                Vector3 projectedForward = Vector3.ProjectOnPlane(forward, forwardPlane.normal).normalized;

                Debug.DrawRay(transform.position, projectedForward, Color.red, 0.1f);

                // Calculate the angle between the projected normal and the forward vector
                Vector3 left = -transform.right;
                //flatten the left vector
                left.y = 0;
                Plane leftNormalPlane = new Plane(left.normalized, transform.position);

                Vector3 projectedNormal = Vector3.ProjectOnPlane(normal, leftNormalPlane.normal).normalized;

                Debug.DrawRay(transform.position, projectedNormal, Color.green, 0.1f);

                float inclineAngle = Vector3.SignedAngle(projectedForward, projectedNormal, transform.right);


                currentInclineAngle = -(inclineAngle + additionAmount);
            }
        }


        void OnHitTop()
        {

        }

        void OnHitSides()
        {

        }

    }
}
