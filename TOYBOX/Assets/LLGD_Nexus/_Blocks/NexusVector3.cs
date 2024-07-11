using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusVector3 : NexusPrimitive<Vector3>
    {
        public NexusBool ClampMagnitude;
        public NexusFloat maxMagnitude;
        public NexusFloat minMagnitude;

        protected override void OnInitializeObject()
        {
            base.OnInitializeObject();

            if (ClampMagnitude == null)
            {
                ClampMagnitude = CreateInstance<NexusBool>();
                ClampMagnitude.Set(false);
            }
            if (maxMagnitude == null)
            {
                maxMagnitude = CreateInstance<NexusFloat>();
                maxMagnitude.Set(1);

            }
            if (minMagnitude == null)
            {
                minMagnitude = CreateInstance<NexusFloat>();
                minMagnitude.Set(0);
            }

            ClampMagnitude.InitializeObject();
            maxMagnitude.InitializeObject();
            minMagnitude.InitializeObject();
        }


        [SerializeField] Vector3 _value;
        public override Vector3 value
        {
            get => _value; protected set
            {
                if (ClampMagnitude)
                {
                    value = Vector3.ClampMagnitude(value, maxMagnitude);
                }
                _value = value;
            }
        }

        public void Add(Vector3 value)
        {
            this.value += value;
        }

        //override the * for float and vector3
        public static Vector3 operator *(NexusVector3 a, float b)
        {
            return a.value * b;
        }





        public static Vector3 operator -(NexusVector3 a, NexusVector3 b)
        {
            return a.value - b.value;
        }





    }

}
