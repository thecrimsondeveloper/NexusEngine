using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Extras;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusCharacterControllerMovement : NexusMovement
    {
        [Title("Settings")]
        public NexusFloat moveSpeedMultiplier;
        public NexusFloat rotationSpeedMultiplier;
        public NexusFloat drag;
        public NexusVector3 gravity;


        [Title("State")]
        public NexusVector3 velocity;


        [Title("Input")]
        public NexusVector2 moveAxis;
        public NexusVector2 lookAxis;

        [Title("References")]
        public CharacterController controller;
        public Transform head = null;


        protected override void OnInitializeBlock(NexusEntity entity)
        {
            base.OnInitializeBlock(entity);

            if (moveSpeedMultiplier == null)
            {
                moveSpeedMultiplier = ScriptableObject.CreateInstance<NexusFloat>();
                moveSpeedMultiplier.Set(1);
            }

            if (rotationSpeedMultiplier == null)
            {
                rotationSpeedMultiplier = ScriptableObject.CreateInstance<NexusFloat>();
                rotationSpeedMultiplier.Set(1);
            }

            if (drag == null)
            {
                drag = ScriptableObject.CreateInstance<NexusFloat>();
                drag.Set(0.1f);
            }

            if (gravity == null)
            {
                gravity = ScriptableObject.CreateInstance<NexusVector3>();
                gravity.Set(new Vector3(0, -9.81f, 0));
            }

            if (velocity == null)
            {
                velocity = ScriptableObject.CreateInstance<NexusVector3>();
                velocity.Set(Vector3.zero);
            }

            if (moveAxis == null)
            {
                moveAxis = ScriptableObject.CreateInstance<NexusVector2>();
                moveAxis.Set(Vector2.zero);
            }

            if (lookAxis == null)
            {
                lookAxis = ScriptableObject.CreateInstance<NexusVector2>();
                lookAxis.Set(Vector2.zero);
            }


            if (ResolveComponent(ref controller))
            {
                controller = GetComponent<CharacterController>();
            }

            moveSpeedMultiplier.InitializeObject();
            rotationSpeedMultiplier.InitializeObject();
            drag.InitializeObject();
            velocity.InitializeObject();
            moveAxis.InitializeObject();
            lookAxis.InitializeObject();
            gravity.InitializeObject();



            //lock the mouse
            Cursor.lockState = CursorLockMode.Locked;
            //hide the mouse
            Cursor.visible = false;
        }


        private void Update()
        {
            moveAxis.Set(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            lookAxis.Set(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

            //add the movement to the velocity
            Vector3 addition = new Vector3(moveAxis.value.x, 0, moveAxis.value.y) * moveSpeedMultiplier.value * Time.deltaTime;
            //convert to be relative to the camera
            addition = transform.TransformDirection(addition);

            //add the gravity
            addition += gravity * Time.deltaTime;

            velocity.Add(addition);


            //add the look to the controller
            controller.transform.Rotate(Vector3.up, lookAxis.value.x * rotationSpeedMultiplier * Time.deltaTime);

            //add the look to the head
            head.Rotate(Vector3.right, Mathf.Clamp(-lookAxis.value.y, -1, 1) * rotationSpeedMultiplier * Time.deltaTime);

            //clamp the head rotation
            Vector3 angles = head.localEulerAngles;
            head.localEulerAngles = angles;
        }

        private void FixedUpdate()
        {
            //apply drag to the velocity
            velocity.Set(velocity * (1 - Mathf.Clamp01(Time.deltaTime * drag)));

            //move the controller
            controller.Move(velocity.value);
        }

    }
}
