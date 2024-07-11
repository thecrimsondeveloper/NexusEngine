using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusVector2 : NexusPrimitive<Vector2>
    {
        [SerializeField] Vector2 _value;

        public override Vector2 value
        {
            get => _value;
            protected set => _value = value;
        }

        //override the * for float and vector3
        public static Vector2 operator *(NexusVector2 a, float b)
        {
            return a.value * b;
        }
    }
}
