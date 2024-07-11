using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusPrimitive<T> : NexusObject
    {
        public abstract T value { get; protected set; }

        public void Set(T value)
        {
            this.value = value;
        }

        public static implicit operator T(NexusPrimitive<T> primitive)
        {
            return primitive.value;
        }

    }

}
