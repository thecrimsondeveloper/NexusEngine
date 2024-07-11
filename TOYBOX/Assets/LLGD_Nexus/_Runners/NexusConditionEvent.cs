using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusConditionEvent : NexusEventBlock
    {
        public bool condition;

        public void Set(bool condition)
        {
            this.condition = condition;
        }

        public static implicit operator bool(NexusConditionEvent a)
        {
            return a.condition;
        }

        public static bool operator ==(NexusConditionEvent a, bool b)
        {
            return a.condition == b;
        }

        public static bool operator !=(NexusConditionEvent a, bool b)
        {
            return a.condition != b;
        }

        public static bool operator ==(bool a, NexusConditionEvent b)
        {
            return a == b.condition;
        }

        public static bool operator !=(bool a, NexusConditionEvent b)
        {
            return a != b.condition;
        }


        //convert to int
        public static implicit operator int(NexusConditionEvent a)
        {
            return a.condition ? 1 : 0;
        }

        public static bool operator ==(NexusConditionEvent a, int b)
        {
            return (a.condition ? 1 : 0) == b;
        }

        public static bool operator !=(NexusConditionEvent a, int b)
        {
            return (a.condition ? 1 : 0) != b;
        }



    }
}
