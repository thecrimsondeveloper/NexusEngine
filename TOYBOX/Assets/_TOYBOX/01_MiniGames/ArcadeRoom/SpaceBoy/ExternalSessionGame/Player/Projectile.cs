using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        public PlayerMovement playerMovement;
        [SerializeField] float moveSpeed = 10f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
            {
                obstacle.DestroySelf(1f);
            }
        }

        void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            //get an offset from the player depending on which direction they are moving in vertically
            Vector2 offset = new Vector2(0, playerMovement.Direction.y * 0.5f);
            Vector2 moveDirection = new Vector2(moveSpeed * 20, offset.y);
            rb.AddForce(moveDirection);
        }
    }
}
