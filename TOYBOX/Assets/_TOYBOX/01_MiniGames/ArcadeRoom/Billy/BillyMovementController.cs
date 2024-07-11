using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox
{
    public class BillyMovementController : MonoBehaviour
    {
        enum PositionType { Front, Back, Middle }
        [SerializeField, BoxGroup("State")] PositionType positionType = PositionType.Middle;

        [Title("Movement Settings")]
        [SerializeField, Range(0, 1), BoxGroup("Settings")] float drag = 0.1f;
        [SerializeField, BoxGroup("Settings")] float gravity = -9.81f;
        [SerializeField, BoxGroup("Settings")] float jumpForce = 5f;
        [SerializeField, FoldoutGroup("References")] Rigidbody rb;
        [SerializeField, FoldoutGroup("References")] Transform frontPosition;
        [SerializeField, FoldoutGroup("References")] Transform middlePosition;
        [SerializeField, FoldoutGroup("References")] Transform backPosition;

        Vector3 targetPosition;

        private void OnDrawGizmos()
        {
            if (frontPosition == null || middlePosition == null || backPosition == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(frontPosition.position, 0.01f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(middlePosition.position, 0.01f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(backPosition.position, 0.01f);
        }


        private void FixedUpdate()
        {
            targetPosition = positionType switch
            {
                PositionType.Front => frontPosition.position,
                PositionType.Middle => middlePosition.position,
                PositionType.Back => backPosition.position,
                _ => targetPosition
            };

            Vector3 direction = targetPosition - transform.position;

            Vector3 vel = direction * 5;
            rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
        }

        [Button]
        public void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }


    }
}
