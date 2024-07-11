using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class BombCollectable : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] float horizontalMoveSpeed = 5f;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.playerAmmoVisuals.AddAmmo(1);
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            rb.velocity = new Vector2(-horizontalMoveSpeed, 0);
        }
    }
}
