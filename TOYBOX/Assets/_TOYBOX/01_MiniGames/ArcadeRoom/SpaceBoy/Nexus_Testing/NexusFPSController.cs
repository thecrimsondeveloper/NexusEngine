using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public enum RotationMode
    {
        Mouse,
        ArrowKeys
    }

    public class NexusFPSController : MonoBehaviour
    {
        private CharacterController characterController;
        public float movementSpeed = 5f;
        public float rotationSpeed = 3f;
        public float lookUpLimit = 60f; // The maximum angle the player can look up
        public float lookDownLimit = -60f; // The maximum angle the player can look down
        private float currentRotationX = 0f; // The current rotation around the X-axis
        public float gravity = 9.8f; // Gravity force
        public float drag = 0.1f; // Drag force

        public Transform headTransform; // The transform for head rotation

        public RotationMode rotationMode = RotationMode.Mouse; // Rotation mode
        public Vector3 velocity;

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();

            // Lock the cursor
            if (rotationMode == RotationMode.Mouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                // Cursor.visible = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Handle input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleRotationMode();
            }
        }

        // FixedUpdate is called at a fixed interval
        void FixedUpdate()
        {
            // Move the player
            MovePlayer();
            // Rotate the player
            if (rotationMode == RotationMode.Mouse)
            {
                RotatePlayer();
            }
            else if (rotationMode == RotationMode.ArrowKeys)
            {
                RotatePlayerWithArrowKeys();
            }
            // Apply gravity
            ApplyGravity();

            //drag velocity
            velocity *= 1 - Mathf.Clamp01(Time.deltaTime * drag);
            characterController.Move(velocity * Time.fixedDeltaTime);
        }

        void AddForce(Vector3 force)
        {
            velocity += force;
        }

        void Jump()
        {
            //add jump force
            float jumpForce = 5f;
            velocity = new Vector3(velocity.x, jumpForce, velocity.z);
        }

        private void MovePlayer()
        {
            float horizontalInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
            float verticalInput = Input.GetKey(KeyCode.S) ? -1f : Input.GetKey(KeyCode.W) ? 1f : 0f;

            Vector3 movement = transform.forward * verticalInput + transform.right * horizontalInput;
            AddForce(movement * movementSpeed);
        }

        float mouseXDelta = 0f;
        private void RotatePlayer()
        {
            float mouseX = Input.GetAxis("Mouse X");
            // Rotate around the Y-axis (left and right)
            transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.fixedDeltaTime);
        }

        private void RotatePlayerWithArrowKeys()
        {
            float horizontalRotation = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
            float verticalRotation = Input.GetKey(KeyCode.DownArrow) ? -1f : Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;

            // Rotate around the Y-axis (left and right)
            transform.Rotate(Vector3.up, horizontalRotation * rotationSpeed);

            // Rotate around the X-axis (up and down)
            currentRotationX -= verticalRotation * rotationSpeed;
            currentRotationX = Mathf.Clamp(currentRotationX, lookDownLimit, lookUpLimit);
            headTransform.localRotation = Quaternion.Euler(currentRotationX, headTransform.localRotation.eulerAngles.y, 0f);
        }

        private void ApplyGravity()
        {
            if (!characterController.isGrounded)
            {
                velocity += Vector3.down * gravity * Time.fixedDeltaTime;
            }
        }

        private void ToggleRotationMode()
        {
            if (rotationMode == RotationMode.Mouse)
            {
                rotationMode = RotationMode.ArrowKeys;
            }
            else if (rotationMode == RotationMode.ArrowKeys)
            {
                rotationMode = RotationMode.Mouse;
            }
        }
    }
}
