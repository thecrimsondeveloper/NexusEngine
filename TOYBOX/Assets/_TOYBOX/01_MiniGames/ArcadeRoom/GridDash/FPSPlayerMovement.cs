using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Extras
{
    using UnityEngine;

    public class FPSPlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private Transform headTransform;

        [SerializeField]
        private Vector2 movementInput;

        [SerializeField]
        private Vector2 rotationInput;

        [SerializeField]
        private Vector3 moveDirection;

        [SerializeField]
        private float jumpForce = 5f;

        [SerializeField]
        private float gravity = 9.81f;

        [SerializeField]
        private float speed = 5f;

        [SerializeField]
        private float lookSpeed = 2f;

        [SerializeField]
        private float lookXLimit = 60f;

        [SerializeField]
        private float rotationX = 0f;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
          
        }

        private void Update()
        {
            // Calculate movement direction
            float taperedInputX = Mathf.Lerp(movementInput.x, 0f, Time.deltaTime);
            float taperedInputY = Mathf.Lerp(movementInput.y, 0f, Time.deltaTime);
            moveDirection = transform.TransformDirection(new Vector3(taperedInputX, 0f, taperedInputY));
            moveDirection *= speed;

            // Apply gravity
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the character controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Rotate the player horizontally
            transform.Rotate(Vector3.up * rotationInput.x * lookSpeed);

            // Rotate the head vertically
            rotationX -= rotationInput.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            headTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        }

        public void SetMovementInput(Vector2 input)
        {
            movementInput = input;
        }

        public void SetRotationInput(Vector2 input)
        {
            rotationInput = input;
        }

        public void Jump(float force)
        {
            // Apply jump force
            moveDirection.y = force;
        }

        public void SetHeadForward(Vector3 forward)
        {
            headTransform.forward = forward;
        }
    }
}
