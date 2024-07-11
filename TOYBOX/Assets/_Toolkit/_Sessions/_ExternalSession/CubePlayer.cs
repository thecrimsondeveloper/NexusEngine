using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class CubePlayer : MonoBehaviour
    {
        public float jumpForce = 10f;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector2.up * jumpForce;
            }
        }
    }
}
