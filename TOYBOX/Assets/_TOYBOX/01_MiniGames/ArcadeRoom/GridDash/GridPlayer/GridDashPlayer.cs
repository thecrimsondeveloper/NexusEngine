using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Extras;
using Toolkit.XR;

namespace ToyBox
{
    public class GridDashPlayer : MonoBehaviour
    {
        [SerializeField] FPSPlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GetComponent<FPSPlayerMovement>();
        }

        private void Update()
        {
            //playerMovement.SetRotationInput(HeadJoyStickInput.Value);
            //playerMovement.SetMovementInput(HandInputVolume.AverageHorizontalAxis);



        }
    }
}
