using System.Collections;
using System.Collections.Generic;
using Toolkit.Extras;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class HoverSpin : MonoBehaviour
    {
        [SerializeField] float maxSpinSpeed = 10;
        [SerializeField] AnimationCurve overRangeMultiplier = null;
        [SerializeField, Range(0.25f, 20)] float spinChargeTime = 1;
        [SerializeField, Range(0.01f, 0.2f)] float hoverDistance = 0.1f;

        [SerializeField] Transform spinTarget;
        [SerializeField] Axis spinAxis = Axis.Y;

        enum Axis
        {
            X,
            Y,
            Z
        }


        float currentSpinSpeed = 0;
        float chargeTime = 0;
        Vector3 handPosition;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hoverDistance);
        }

        float lastTime = 0;
        private void Update()
        {
            handPosition = XRPlayer.ClosestHand(transform.position).Position;

            if (Vector3.Distance(handPosition, transform.position) < hoverDistance)
            {
                chargeTime += Time.deltaTime / spinChargeTime;
            }
            else if (chargeTime > 0)
            {
                chargeTime -= Time.deltaTime;
            }
            else
            {
                chargeTime = 0;
            }



            currentSpinSpeed = overRangeMultiplier.Evaluate(chargeTime / spinChargeTime) * maxSpinSpeed;

            switch (spinAxis)
            {
                case Axis.X:
                    spinTarget.Rotate(Vector3.right, currentSpinSpeed * Time.deltaTime, Space.Self);
                    break;
                case Axis.Y:
                    spinTarget.Rotate(Vector3.up, currentSpinSpeed * Time.deltaTime, Space.Self);
                    break;
                case Axis.Z:
                    spinTarget.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime, Space.Self);
                    break;
            }

        }
    }
}
