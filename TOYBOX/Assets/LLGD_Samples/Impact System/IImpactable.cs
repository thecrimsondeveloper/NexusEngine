using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit
{
    public interface IImpactable
    {
        void OnImpact(ImpactInfo impactInfo);


    }

    public struct ImpactInfo
    {
        public Impactor impactor;
        public Vector3 point;
        public Vector3 direction;
        public Vector3 normal;
        public float force;

    }
}
