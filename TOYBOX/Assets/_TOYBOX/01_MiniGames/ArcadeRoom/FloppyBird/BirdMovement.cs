using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Throw;
using Sirenix.OdinInspector;
using Toolkit.XR;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox
{
    public class BirdMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] float flopForce = 0f;
        [SerializeField] float flopCooldown = 0f;
        [SerializeField] AnimationCurve flopCurve;
        [SerializeField] Animator animator;
        [SerializeField] FloppyBird floppyBird;

        //get set floppy bird
        public FloppyBird FloppyBird { get => floppyBird; set => floppyBird = value; }

        float cooldown = 0f;

        private void Update()
        {
            //PC INPUTS
            // if (Input.GetKeyDown(KeyCode.RightArrow))
            // {
            //     FlopRight();
            // }
            // if (Input.GetKeyDown(KeyCode.LeftArrow))
            // {
            //     FlopLeft();
            // }

            // VR INPUTS
            XRPlayer.XRPlayerHand rightHand = XRPlayer.RightHand;
            XRPlayer.XRPlayerHand leftHand = XRPlayer.LeftHand;

            if (rightHand.indexPinchStrength > 0.9f)
            {
                FlopLeft();
            }
            if (leftHand.indexPinchStrength > 0.9f)
            {
                FlopRight();
            }

            // Reset cooldown
            cooldown = flopCooldown;
        }

        private void FixedUpdate()
        {
            if (transform.localPosition.y > 5)
            {
                Die();
            }
            else if (transform.localPosition.y < -5)
            {
                Die();
            }

            if (rb.velocity.y > 0)
            {
                //lerp the birds rotation upwards to 45 degrees (X AXIS)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-45, 0, 0), 0.2f);
            }
            else if (rb.velocity.y < 0)
            {
                //lerp the birds rotation downwards to -45 degrees (X AXIS)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(45, 0, 0), 0.2f);
            }

            ApplyGravityForce();
        }

        [Button("Flop Right")]
        public void FlopRight()
        {
            rb.velocity = Vector2.zero;

            animator.Play("FlapRight");

            StartCoroutine(FlopRightCoroutine());
        }

        private IEnumerator FlopRightCoroutine()
        {
            float flopForceValue = flopCurve.Evaluate(0f);
            float targetFlopForceValue = flopCurve.Evaluate(1f);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.fixedDeltaTime / flopCooldown;
                flopForceValue = Mathf.Lerp(flopForceValue, targetFlopForceValue, t);
                //add force in two force adds, one right and one up
                rb.AddForce(transform.TransformDirection(Vector2.right) * flopForce / 2 * flopForceValue, ForceMode.Impulse);
                rb.AddForce(transform.TransformDirection(Vector2.up) * flopForce * flopForceValue, ForceMode.Impulse);
                yield return new WaitForFixedUpdate();
            }
        }

        [Button("Flop Left")]
        public void FlopLeft()
        {
            rb.velocity = Vector2.zero;

            animator.Play("FlapLeft");

            StartCoroutine(FlopLeftCoroutine());
        }

        private IEnumerator FlopLeftCoroutine()
        {
            float flopForceValue = flopCurve.Evaluate(0f);
            float targetFlopForceValue = flopCurve.Evaluate(1f);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.fixedDeltaTime / flopCooldown;
                flopForceValue = Mathf.Lerp(flopForceValue, targetFlopForceValue, t);
                //add force in two force adds, one left and one up
                rb.AddForce(transform.TransformDirection(Vector2.left) * flopForce / 2 * flopForceValue, ForceMode.Impulse);
                rb.AddForce(transform.TransformDirection(Vector2.up) * flopForce * flopForceValue, ForceMode.Impulse);
                yield return new WaitForFixedUpdate();
            }
        }

        private void ApplyGravityForce()
        {
            rb.AddForce(Vector2.down * 20f, ForceMode.Acceleration);
        }

        public void Die()
        {
            Debug.Log("You died");
            rb.velocity = Vector2.zero;
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            floppyBird.Reset();
        }
    }
}
