using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class NexusBranch : MonoBehaviour
    {
        public List<BranchOption> options;


        [System.Serializable]
        public class BranchOption
        {
            NexusCondition condition;
            public NexusEventBlock action;

            public void Set(NexusCondition condition, NexusEventBlock action)
            {
                this.condition = condition;
                this.action = action;
            }

        }
    }
}
