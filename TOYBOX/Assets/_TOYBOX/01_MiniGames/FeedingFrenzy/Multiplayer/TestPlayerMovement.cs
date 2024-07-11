using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

namespace ToyBox
{
    public class TestPlayerMovement : NetworkBehaviour
    {
        [SerializeField] Rigidbody rb;
        //create a simple movement script with wasd controls for the player
        public float moveSpeed = 5;

        private void Update()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.velocity = move * moveSpeed;
        }




    }
}
