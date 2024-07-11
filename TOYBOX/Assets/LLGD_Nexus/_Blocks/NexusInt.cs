using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusInt : NexusPrimitive<int>
    {
        [SerializeField] int _value;
        public override int value { get => _value; protected set => _value = value; }

        public void Increment()
        {
            _value++;
        }

        public void Decrement()
        {
            _value--;
        }
    }
}
