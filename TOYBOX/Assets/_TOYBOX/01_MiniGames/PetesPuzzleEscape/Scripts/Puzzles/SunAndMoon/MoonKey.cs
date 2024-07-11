using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Samples;
using Oculus.Interaction;

namespace ToyBox
{
    public class MoonKey : MonoBehaviour
    {
        public void LockInPlace(Pose pose)
        {
            transform.position = pose.position;
            transform.rotation = pose.rotation;

            GetComponent<Grabbable>().enabled = false;  // Disable the Grabbable component
        }
    }
}
