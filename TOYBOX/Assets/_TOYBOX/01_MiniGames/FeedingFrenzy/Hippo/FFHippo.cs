using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox
{
    public class FFHippo : NetworkBehaviour
    {
        [SerializeField] Transform faceAwayPoint;
        [SerializeField] PinchDetector pinchDetector;
        [SerializeField] Animation anims;
        [SerializeField] AnimationClip lunge;
        [SerializeField] Rigidbody rb;

        [SerializeField] float maxVelocity = 10;
        [SerializeField] bool isLunging = false;

        public Vector3 Velocity
        {
            get
            {
                return rb.velocity;
            }
            set
            {
                if (isLunging)
                {
                    return;
                }

                rb.velocity = value;
                //clamp the velocity to the max velocity
                if (rb.velocity.magnitude > maxVelocity)
                {
                    rb.velocity = rb.velocity.normalized * maxVelocity;
                }
            }
        }


        private void Start()
        {
            pinchDetector.OnPinch.AddListener(Lunge);
        }


        [Button]
        public async void Lunge()
        {
            if (isLunging)
            {
                return;
            }

            isLunging = true;
            anims.Play(lunge.name);
            await UniTask.WaitUntil(() => !anims.isPlaying);
            isLunging = false;
        }

        public void Bite()
        {
            Debug.Log("Bite");
        }

        public override void FixedUpdateNetwork()
        {
            if (HasInputAuthority == false)
            {
                return;
            }
            if (HasStateAuthority == false)
            {
                return;
            }

            Vector3 faceAwayPointPosition = faceAwayPoint == null ? Vector3.zero : faceAwayPoint.position;

            //face the hippo towards the face away point
            Vector3 direction = faceAwayPointPosition - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
        }
    }
}
