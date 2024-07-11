using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class CosmicSessionOffset : MonoBehaviour
    {
        public bool shouldOffsetOnStart;

        [SerializeField] Vector3 offsetDirectionFromPlayer;
        [SerializeField] float offsetDistanceFromPlayer;

        private void Start()
        {
            if (shouldOffsetOnStart)
            {
                OffsetFromPlayer(offsetDirectionFromPlayer, offsetDistanceFromPlayer);
            }
        }

        private void OffsetFromPlayer(Vector3 direction, float distance)
        {
            transform.position = XRPlayer.HeadPose.position + direction.normalized * distance;
        }
    }
}
