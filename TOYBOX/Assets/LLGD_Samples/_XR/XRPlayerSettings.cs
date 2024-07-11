using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.XR
{
    [CreateAssetMenu(fileName = "XRPlayerSettings", menuName = "Toolkit/XR/Player Settings")]
    public class XRPlayerSettings : ScriptableObject
    {
        public float neckHeight = 0.05f;
        public float punchSpeed = 0.5f;
        public float timeToTrackAverages = 0.5f;
        public float gazeAnchorHoverDistance = 1f;

        public Vector3 leftHandOffset = Vector3.zero;
        public Vector3 rightHandOffset = Vector3.zero;
        public Vector3 leftHandPositionOffset = Vector3.zero;
        public Vector3 rightHandPositionOffset = Vector3.zero;
    }
}
