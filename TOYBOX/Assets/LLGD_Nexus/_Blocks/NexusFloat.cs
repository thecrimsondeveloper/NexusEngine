using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusFloat : NexusPrimitive<float>
    {

        [SerializeField] private float _value;
        public override float value { get => _value; protected set => _value = value; }

        public static float operator *(NexusFloat a, float b)
        {
            return a.value * b;
        }



    }
}
