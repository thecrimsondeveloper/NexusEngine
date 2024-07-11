using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.VFX;


namespace ToyBox.Minigames.BeatEmUp
{
    public class Billy : MonoBehaviour, IAttacker, ICollidable
    {
        [SerializeField, BoxGroup("Punching")] SphereCollider punchCollider = null;
        [Range(0, 0.5f)] public float xrToBillyRatio = 0.1f;
        [Range(0, 0.5f)] public float upperArmLength = 0.1f;
        [Range(0, 0.5f)] public float upperArmRatioToArmLength = 0.4f;
        [SerializeField, BoxGroup("Movement")] BillyMovementController movementController = null;
        [SerializeField, BoxGroup("Rotation")] Transform leftArm = null;
        [SerializeField, BoxGroup("Rotation")] Transform rightArm = null;

        [SerializeField, BoxGroup("Elbow Positioning")] Transform leftForeArm = null;
        [SerializeField, BoxGroup("Elbow Positioning")] Transform rightForeArm = null;
        [SerializeField, BoxGroup("Elbow Positioning")] Transform leftUpperArm = null;
        [SerializeField, BoxGroup("Elbow Positioning")] Transform rightUpperArm = null;
        [SerializeField] VisualEffect billyVX = null;




        [SerializeField] float neckHeight = 0.05f;
        [SerializeField] int _strength = 5;

        public int strength { get => _strength; }

        private void Awake()
        {
            XRPlayer.LeftHand.OnPunch.AddListener(OnPunch);
            XRPlayer.RightHand.OnPunch.AddListener(OnPunch);
        }

        [Button("OnPunch")]
        void OnPunch(Vector3 dir)
        {
            //if pointing up
            float dotWithUp = Vector3.Dot(dir, Vector3.up);
            float dotWithHeadForward = Vector3.Dot(dir, XRPlayer.HeadPose.forward);

            if (dotWithUp > 0.5f)
            {
                Debug.Log("Punching up");
                if (movementController != null)
                {
                    movementController.Jump();
                }
            }
            else if (dotWithUp <= 0.5f && dotWithUp > -0.5f)
            {
                //attack if punch is not too high or too low
                Debug.Log("Punching forward");
                Attack();
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            SetArmRotation(leftArm, XRPlayer.LeftHand);
            SetArmRotation(rightArm, XRPlayer.RightHand);

            UpdateArmBends(leftArm, leftUpperArm, leftForeArm, upperArmLength, XRPlayer.LeftHand.distanceFromNeck);
            UpdateArmBends(rightArm, rightUpperArm, rightForeArm, upperArmLength, XRPlayer.RightHand.distanceFromNeck);
        }

        void SetArmRotation(Transform arm, XRPlayer.XRPlayerHand hand)
        {
            float horizontalDistance = hand.horizontalDistanceFromNeck;
            float heightDifferentFromNeck = hand.verticalDistanceFromNeck;

            float armRotation = Mathf.Atan2(-heightDifferentFromNeck, horizontalDistance) * Mathf.Rad2Deg;
            arm.localEulerAngles = new Vector3(armRotation, arm.localEulerAngles.y, arm.localEulerAngles.z);
        }

        void UpdateArmBends(Transform arm, Transform upperArmAnchor, Transform foreArmAnchor, float upperArmLength, float handdistance = 0.5f)
        {
            float billyArmLength = xrToBillyRatio * handdistance;
            float lengthToCrossPoint = billyArmLength * upperArmRatioToArmLength;


            Vector3 handPosition = arm.position + arm.forward * billyArmLength;
            Vector3 crossPoint = arm.position + arm.forward * lengthToCrossPoint;

            Debug.DrawLine(arm.position, crossPoint, Color.red, 0.5f);
            Debug.DrawLine(crossPoint, handPosition, Color.green, 0.5f);

            float crossPointToElbowLength = Mathf.Sqrt(Mathf.Pow(upperArmLength, 2) - Mathf.Pow(lengthToCrossPoint, 2));
            Debug.Log("CrossPointToElbowLength: " + crossPointToElbowLength);
            Vector3 elbowPoint = crossPoint + -arm.up * crossPointToElbowLength;
            //set the upper arm anchor to point towward where the elbow should be

            Debug.DrawLine(crossPoint, elbowPoint, Color.blue, 0.5f);
            upperArmAnchor.LookAt(elbowPoint, arm.up);

            //set the forarm position to be the elbow position
            foreArmAnchor.position = upperArmAnchor.position + upperArmAnchor.forward * upperArmLength;
            foreArmAnchor.LookAt(handPosition, arm.up);
        }

        [Button("Attack")]
        void Attack()
        {

            Vector3 localPunchPos = punchCollider.transform.position;
            Debug.DrawLine(transform.position, localPunchPos, Color.green, 1);

            Collider[] hits = Physics.OverlapSphere(localPunchPos, punchCollider.radius);
            foreach (var hit in hits)
            {
                IAttackable attackable = hit.GetComponent<IAttackable>();
                if (attackable != null)
                {
                    Debug.Log("Attacking: " + attackable);
                    attackable.Attack(this);
                }

                Debug.DrawLine(localPunchPos, hit.transform.position, Color.red, 1);
            }
        }


        public void Die()
        {
            Destroy(gameObject, 4);
            if (billyVX) billyVX.SendEvent("OnDie");
            // if (Session.CurrentSession is RoundBasedSession roundSession)
            // {
            //     roundSession.NextRound();
            // }
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out ICollidable collidable))
            {
                if (collidable is Barrel barrel)
                {
                    Die();
                }
            }
        }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }
    }
}
