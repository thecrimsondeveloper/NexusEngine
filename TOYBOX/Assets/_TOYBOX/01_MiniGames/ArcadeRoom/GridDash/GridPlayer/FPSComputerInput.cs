using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Extras
{
    public class FPSComputerInput : MonoBehaviour
    {
        [SerializeField] FPSPlayerMovement playerMovement;
        private void Start()
        {
            if (playerMovement == null)
                playerMovement = GetComponent<FPSPlayerMovement>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            playerMovement.SetMovementInput(new Vector2(horizontal, vertical));

            

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Pass mouse delta to player movement
            playerMovement.SetRotationInput(new Vector2(mouseX,mouseY));
        }


    }
}
