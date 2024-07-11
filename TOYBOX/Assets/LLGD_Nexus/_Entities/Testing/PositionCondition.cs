using System.Collections;
using System.Collections.Generic;
using Toolkit.Extras;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class PositionCondition : NexusConditionEvent
    {
        [SerializeField] float targetY = 0;
        [SerializeField] float invokeInterval = 0;
        [SerializeField] NexusEventBlock[] targetBlocks;
        float timeLastInvoke = 0;

        protected override void OnInvokeBlock()
        {
            foreach (var block in targetBlocks)
            {
                block.InvokeBlock();
            }
        }




    }
}
