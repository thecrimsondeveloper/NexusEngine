using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class SnapPoint : MonoBehaviour
    {
        [SerializeField] Pose snapPose = new Pose();
        [SerializeField] bool isOccupied = false;

        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
        public Pose SnapPose
        {
            get => snapPose;
            set
            {
                snapPose = value;
            }
        }
    }
}
