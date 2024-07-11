using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace ToyBox.Extras
{
    public class MoveAndRotateGrabbable : TransformGrabbable
    {
        [SerializeField] ParentConstraint parentConstraint;


        public override void OnGrab(PointerEvent evt)
        {
            base.OnGrab(evt);
            SetupParentConstraint();
        }

        public override void OnRelease(PointerEvent evt)
        {
            base.OnRelease(evt);


            Destroy(parentConstraint);
        }



        void SetupParentConstraint()
        {
            if (parentConstraint == null)
            {
                parentConstraint = AddComponentToParent<ParentConstraint>();
            }

            if (parentConstraint == null)
            {
                Debug.LogError("Parent Constraint is null");
                return;
            }


            if (parentConstraint.sourceCount == 0)
            {
                parentConstraint.AddSource(new ConstraintSource()
                {
                    sourceTransform = transform,
                    weight = 1
                });
            }

            parentConstraint.SetTranslationOffset(0, -relativePoseWhenGrabFromParent.position);
            parentConstraint.constraintActive = true;
        }
    }
}
