using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class RigidbodyMoveController : NexusBlock
    {
        [SerializeField] NexusDirection gravity;
        [SerializeField] float jumpForce;
        [SerializeField] Rigidbody rb;

        private void FixedUpdate()
        {
            rb.AddForce(gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        public void Jump()
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jumping");
        }


        public void Jump(float force)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            Debug.Log("Jumping");
        }
    }
}
