using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using ToyBox.Games.OpenWorld;
using ToyBox.Minigames.OpenWorld;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Crafting
{
    public class CraftInteractable : MonoBehaviour
    {
        public enum Direction { FORWARD, UP, LEFT, RIGHT, DOWN, BACKWARD }
        [SerializeField] Item item = null;
        public Item Item => item;
        [SerializeField] bool requireDirectionalCollision = false;
        [SerializeField] Direction direction = Direction.FORWARD;
        [SerializeField] float dotThreshold = 0.5f;
        [SerializeField] float slamVelocityThreshold = 5.0f; // Threshold for slamming together

        Vector3 impactDirection => direction switch
        {
            Direction.FORWARD => transform.forward,
            Direction.UP => transform.up,
            Direction.LEFT => transform.right,
            Direction.RIGHT => -transform.right,
            Direction.DOWN => -transform.up,
            Direction.BACKWARD => -transform.forward,
            _ => transform.forward
        };

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, impactDirection.normalized * 0.5f);
        }

        bool isInteracting = false;
        private void OnCollisionEnter(Collision other)
        {
            // Check if the other object is another craft interactable
            if (other.gameObject.TryGetComponent(out CraftInteractable otherCraftInteractable))
            {
                if (isInteracting)
                {
                    return;
                }

                isInteracting = true;
                // Get the collision direction
                Vector3 collisionDirection = other.contacts[0].point - transform.position;
                collisionDirection.Normalize();

                Debug.DrawRay(transform.position, collisionDirection, Color.green, 5.0f);

                // Calculate relative velocity
                Rigidbody thisRigidbody = GetComponent<Rigidbody>();
                Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

                Vector3 relativeVelocity = thisRigidbody.velocity - otherRigidbody.velocity;

                // Check if the relative velocity exceeds the threshold
                if (relativeVelocity.magnitude > slamVelocityThreshold)
                {
                    // Check directional collision if required
                    if (requireDirectionalCollision)
                    {
                        float dotProduct = Vector3.Dot(collisionDirection, impactDirection.normalized);
                        if (dotProduct > dotThreshold)
                        {
                            Debug.Log("Creating craft with directional collision");
                            Crafter.Create(this, otherCraftInteractable);
                        }
                    }
                    else
                    {
                        Debug.Log("Creating craft without directional collision");
                        Crafter.Create(this, otherCraftInteractable);
                    }
                }

                Debug.Log("Collision");
            }

            Debug.Log("After Collision");
        }
    }
}
