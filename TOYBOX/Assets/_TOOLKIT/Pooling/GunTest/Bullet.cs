using UnityEngine;

namespace Toolkit.Extras
{
    public class Bullet : MonoBehaviour
    {
        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }


        public void Shoot(float speed, float destructionDelay)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            Destroy(gameObject, destructionDelay);
        }


        // Reset bullet properties when it's disabled (returned to the object pool)
        private void OnDisable()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
