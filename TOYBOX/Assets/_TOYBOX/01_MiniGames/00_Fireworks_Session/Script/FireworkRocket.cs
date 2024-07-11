using System.Collections;
using UnityEngine;

namespace ToyBox.Minigames.Fireworks
{
    public class FireworkRocket : MonoBehaviour
    {
        [SerializeField] GameObject flameTrail = null;
        [SerializeField] GameObject explosion = null;
        [SerializeField] Rigidbody rb = null;
        [SerializeField] MeshRenderer groundedMesh;
        [SerializeField] MeshRenderer flyingMesh;

        [Header("Launch Parameters")]
        [SerializeField] float launchForce = 10f;
        [SerializeField] float turbulence = 1f;

        private void Start()
        {
            Launch(Vector3.forward);
        }

        public void PlayFlameTrail()
        {
            flameTrail.SetActive(true);
            groundedMesh.enabled = false;
        }

        public void PlayExplosion()
        {
            explosion.SetActive(true);
            flyingMesh.enabled = false;
        }

        public void StopFlameTrail()
        {
            flameTrail.SetActive(false);
        }

        public void StopExplosion()
        {
            explosion.SetActive(false);
        }

        public void Launch(Vector3 direction)
        {
            // Enable flame trail on launch
            PlayFlameTrail();


            // Apply launch force and trajectory
            rb.AddForce(direction * launchForce, ForceMode.Impulse);
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);



            // Start the launch coroutine for turbulence
            StartCoroutine(LaunchCoroutine());
        }

        IEnumerator LaunchCoroutine()
        {
            float time = 0f;

            while (time <= 1f)
            {
                // Apply turbulence to the position
                Vector3 turbulenceForce = new Vector3(Random.Range(-turbulence, turbulence), 0f, Random.Range(-turbulence, turbulence));
                rb.AddForce(turbulenceForce, ForceMode.Impulse);

                // Increment time
                time += Time.deltaTime;

                yield return null;
            }

            // Disable the flame trail
            StopFlameTrail();

            //wait until the rocket is at the peak of its flight
            yield return new WaitUntil(() => rb.velocity.y <= 0.1f && flameTrail.activeSelf == false);

            Explode();
        }

        public void Explode()
        {
            // Play the explosion
            PlayExplosion();

            // Destroy the rocket after a delay
            Destroy(gameObject, 2f);
        }
    }
}
