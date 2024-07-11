using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class TransformMoveController : NexusBlock
    {
        [SerializeField] Space space;
        [SerializeField] NexusDirection moveDirection;
        void FixedUpdate()
        {
            entity.transform.Translate(moveDirection.value * Time.deltaTime, space);
        }
    }
}
