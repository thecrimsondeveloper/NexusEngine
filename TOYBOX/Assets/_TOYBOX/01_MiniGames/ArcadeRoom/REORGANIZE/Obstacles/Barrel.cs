using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Minigames.BeatEmUp
{
    public class Barrel : Obstacle
    {
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            rb.AddForce(transform.right * horizontalSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            // rb.AddForce(Vector3.down * verticalSpeed * Time.fixedDeltaTime * (isGrounded ? 0.5f : 1), ForceMode.VelocityChange);
        }
    }
}
