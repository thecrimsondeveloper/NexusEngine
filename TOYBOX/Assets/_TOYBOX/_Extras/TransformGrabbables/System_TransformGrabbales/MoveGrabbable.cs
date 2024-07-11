using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Extras
{
    public class MoveGrabbable : TransformGrabbable
    {

        protected override void OnUpdateParent(Transform parent)
        {
            parent.position = transform.position - relativePoseWhenGrabFromParent.position;
        }
    }
}
