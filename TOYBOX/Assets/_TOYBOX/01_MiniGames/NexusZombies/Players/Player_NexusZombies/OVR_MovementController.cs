using UnityEngine;

namespace ToyBox
{
    public class OVR_MovementController : MonoBehaviour
    {
        public CharacterController characterController;
        public OVRCameraRig ovrCameraRig;

        private Vector3 moveDirection = Vector3.zero;

        void Update()
        {
            // Get input from the Oculus controllers
            Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            // Calculate the movement direction based on input and camera orientation
            Vector3 forward = ovrCameraRig.centerEyeAnchor.rotation * Vector3.forward;
            Vector3 right = ovrCameraRig.centerEyeAnchor.rotation * Vector3.right;
            moveDirection = forward * input.y + right * input.x;

            // Apply movement to the character controller
            characterController.SimpleMove(moveDirection * Time.deltaTime);
        }
    }
}
