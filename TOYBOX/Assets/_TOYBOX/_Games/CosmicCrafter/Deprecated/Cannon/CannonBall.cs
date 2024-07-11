using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class CannonBall : MonoBehaviour
    {
        [SerializeField] Rigidbody rigidbody;
        [SerializeField] float lifeTime = 5f;
        [SerializeField] BallStats ballStats = new BallStats();

        public UnityEvent<BallStats> OnDie;

        public float Airtime => ballStats.airTime;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            ballStats.airTime += Time.deltaTime;

            //update the bounce height
            if (transform.position.y > ballStats.bounceHeight)
            {
                ballStats.bounceHeight = transform.position.y;
            }
        }



        public void Reflect(Vector3 normal, float forceMultiplier = 1f)
        {
            //calculate the new direction
            Vector3 newDirection = Vector3.Reflect(rigidbody.velocity.normalized, normal);
            //apply the force
            rigidbody.AddForce(newDirection * forceMultiplier, ForceMode.Acceleration);
        }

        public void AddForce(Vector3 force)
        {
            rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ground cannonBall))
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            OnDie.Invoke(ballStats);
        }
    }

    public struct BallStats
    {
        public float airTime;
        public float bounceHeight;
        public int bounces;
    }
}
