using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Extras
{
    public class OrientationGrabbable : TransformGrabbable
    {
        [Title("Orientation Grabbable")]
        [SerializeField, Range(0, 1)] float dampening = 0.1f;
        [SerializeField] protected AlignmentAxis parentAlignmentAxis = AlignmentAxis.FORWARD;
        [SerializeField] protected bool useLineRenderer = true;

        [ShowIf(nameof(useLineRenderer))]
        [SerializeField] LineRenderer lineRenderer;

        protected override void OnDrawGizmos()
        {
            if (transform.parent != null)
            {
                if (parentAlignmentAxis == AlignmentAxis.RIGHT)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.parent.right);
                }
                else if (parentAlignmentAxis == AlignmentAxis.UP)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.parent.up);
                }
                else if (parentAlignmentAxis == AlignmentAxis.FORWARD)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.parent.forward);
                }
            }

        }

        public override void OnGrab(PointerEvent evt)
        {
            base.OnGrab(evt);
            if(lineRenderer)
            {
            lineRenderer.enabled = useLineRenderer;
            }
        }

        public override void OnRelease(PointerEvent evt)
        {
            base.OnRelease(evt);
            if(lineRenderer)
            {
            lineRenderer.enabled = false;
            }
        }



        protected override void OnUpdateParent(Transform parent)
        {

            Vector3 dirToParent = parent.position - transform.position;
            float magnitue = dirToParent.magnitude;

            if (dampening <= 0)
            {
                SetParentOrientation(parentAlignmentAxis, dirToParent);
            }
            else
            {
                LerpParentOrientation(parentAlignmentAxis, dirToParent, 1 / dampening * 10);
            }

            float lineLength = Mathf.Clamp(magnitue * 5, 0.5f, 3);


            Vector3 dir = parentAlignmentAxis switch
            {
                AlignmentAxis.RIGHT => parent.right,
                AlignmentAxis.UP => parent.up,
                AlignmentAxis.FORWARD => parent.forward,
                _ => parent.forward,
            };


            lineRenderer.SetPosition(0, parent.position);
            lineRenderer.SetPosition(1, parent.position + dir * lineLength);
        }


    }
}
