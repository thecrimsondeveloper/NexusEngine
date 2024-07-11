using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Toolkit.Samples;
using UnityEngine;

namespace ToyBox
{
    public class SunKey : MonoBehaviour
    {
        public void LockInPlace(Pose pose)
        {
            transform.position = pose.position;
            transform.rotation = pose.rotation;

            GetComponent<Grabbable>().enabled = false;  // Disable the Grabbable component
        }
    }
}
