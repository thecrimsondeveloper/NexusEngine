using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction.Input;
using Sirenix.Utilities;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;

        [SerializeField] float verticalMoveSpeed = 5f;

        public Animator animator;

        [SerializeField] VisualEffect jetEffect;

        [SerializeField] Vector2 direction;

        public Vector2 Direction => direction;

        private void Update()
        {
            GetInput();
        }

        float timeHeld = 0f;

        private void GetInput()
        {
            if (XRPlayer.RightHand.direction == XRPlayer.HandDirection.PalmUp ||
                XRPlayer.LeftHand.direction == XRPlayer.HandDirection.PalmUp)
            {
                if (direction.y != 1)
                {
                    timeHeld = 0;
                    //rotate the jet effect to face up and play the jet up animation
                    jetEffect.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    jetEffect.Play();
                    animator.Play("JetUp");
                }
                direction.y = 1;
                timeHeld += Time.deltaTime;

            }
            else if (XRPlayer.RightHand.direction == XRPlayer.HandDirection.PalmDown ||
                    XRPlayer.LeftHand.direction == XRPlayer.HandDirection.PalmDown)
            {
                if (direction.y != -1)
                {
                    timeHeld = 0;
                    //rotate the jet effect to face down and play the jet down animation
                    jetEffect.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                    jetEffect.Play();
                    animator.Play("JetDown");
                }
                direction.y = -1;
                timeHeld += Time.deltaTime;
            }
            else
            {
                animator.Play("Idle");
                direction.y = 0;
                timeHeld = 0;
            }

            direction.x = 0;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            //lerp the vertical move speed depending on the time held
            rb.velocity = new Vector2(direction.x, direction.y * verticalMoveSpeed * timeHeld * Time.deltaTime);
        }


    }
}
