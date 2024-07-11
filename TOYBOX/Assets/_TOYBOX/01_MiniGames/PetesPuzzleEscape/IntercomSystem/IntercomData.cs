using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    [CreateAssetMenu(fileName = "IntercomData", menuName = "TOYBOX/IntercomData", order = 1)]
    public class IntercomData : ScriptableObject
    {
        public string intercomName;

        [TextArea(3, 10)]
        public string intercomText;
        public AudioClip intercomAudio;
    }
}
