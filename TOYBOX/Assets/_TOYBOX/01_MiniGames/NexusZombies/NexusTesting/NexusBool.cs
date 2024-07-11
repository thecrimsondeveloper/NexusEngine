using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusBool : NexusPrimitive<bool>
    {
        [SerializeField] private bool _value;
        public override bool value { get => _value; protected set => _value = value; }
    }
}