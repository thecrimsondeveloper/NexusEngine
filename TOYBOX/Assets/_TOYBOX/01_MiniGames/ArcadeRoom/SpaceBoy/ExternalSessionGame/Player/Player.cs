using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerMovement playerMovement;
        [SerializeField] PlayerHealthVisuals playerHealth;


        [SerializeField] int totalHealth = 6;
        [SerializeField] int currentHealth = 6;
        [SerializeField] float takeDamageCooldown = 2f;

        public PlayerAmmoVisuals playerAmmoVisuals;

        float lastTimeDamageTaken = 0f;

        private void Start()
        {
            currentHealth = totalHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time - lastTimeDamageTaken < takeDamageCooldown)
            {
                return;
            }

            if (other.gameObject.TryGetComponent(out Obstacle obstacle))
            {
                if (obstacle != null)
                {
                    TakeDamage(1);
                    lastTimeDamageTaken = Time.time;
                }
            }
        }

        private void TakeDamage(int damage)
        {
            currentHealth -= damage;
            playerHealth.TakeDamage(damage);
            playerMovement.animator.Play("TakeDamage");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
            //play death animation
            //disable player movement
            //disable player health visuals

            //show game over screen
        }

    }
}
