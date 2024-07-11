using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusIntQueue : NexusQueue<int>
    {
        [ShowInInspector]
        public override Queue<int> value { get; protected set; } = new Queue<int>();


    }
}
