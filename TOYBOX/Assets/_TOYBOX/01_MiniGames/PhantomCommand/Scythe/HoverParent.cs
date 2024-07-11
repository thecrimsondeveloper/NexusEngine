using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox.Extras
{
    public class HoverParent : MonoBehaviour
    {
        [SerializeField] float hoverDistance = 0.6f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 dir = XRPlayer.HeadPose.forward;
            Vector3 pos = XRPlayer.HeadPose.position;

            Vector3 hoverPosition = pos + (dir * hoverDistance);

            transform.position = hoverPosition;
        }
    }
}
