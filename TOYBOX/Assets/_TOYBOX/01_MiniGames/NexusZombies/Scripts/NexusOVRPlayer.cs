using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

namespace ToyBox
{
    public class NexusOVRPlayer : MonoBehaviour
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] CharacterController characterController;
        [SerializeField] float moveSpeed = 4;
        [SerializeField] float slowSpeedMultiplier = 0.5f;
        [SerializeField] float slowSpeedJoystickThresh = 0.5f;
        [SerializeField] float gravity = 9.81f;
        [SerializeField] float standingHeight = 1.6f;
        [SerializeField] float crouchingHeight = 0.8f;
        [SerializeField] float crouchSpeedMultiplier = 0.5f;

        bool isRotateReset = true;
        bool isCrouching = false;
        private void Update()
        {
            Vector2 leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            Vector2 rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            float speedMultiplier = moveSpeed;


            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
            {
                characterController.height = crouchingHeight;
                isCrouching = true;
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick))
            {
                characterController.height = standingHeight;
                isCrouching = false;
            }


            Vector3 moveDirection = new Vector3(leftJoystick.x, 0, leftJoystick.y);
            moveDirection = cameraTransform.forward * moveDirection.z + cameraTransform.right * moveDirection.x;
            moveDirection.y = 0;

            speedMultiplier *= isCrouching ? crouchSpeedMultiplier : 1;
            speedMultiplier *= leftJoystick.magnitude > 0.5f ? 1 : slowSpeedMultiplier;
            leftJoystick.Normalize();
            Debug.Log("Left Joystick Normalized: " + speedMultiplier + " " + leftJoystick.magnitude + " Direction: " + moveDirection);
            characterController.Move(moveDirection * Time.deltaTime * speedMultiplier);
            characterController.Move(Vector3.down * gravity * Time.deltaTime);







            if (rightJoystick.x > 0.5f && isRotateReset)
            {
                transform.Rotate(0, 45, 0);
                isRotateReset = false;
            }
            else if (rightJoystick.x < -0.5f && isRotateReset)
            {
                transform.Rotate(0, -45, 0);
                isRotateReset = false;
            }
            else if (rightJoystick.x < 0.5f && rightJoystick.x > -0.5f)
            {
                isRotateReset = true;
            }
        }

    }
}
