using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class HeartCollectable : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] float horizontalMoveSpeed = 5f;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.GetComponent<PlayerHealthVisuals>().RestoreHealth(1);
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            rb.velocity = new Vector2(-horizontalMoveSpeed, 0);
        }
    }
}
