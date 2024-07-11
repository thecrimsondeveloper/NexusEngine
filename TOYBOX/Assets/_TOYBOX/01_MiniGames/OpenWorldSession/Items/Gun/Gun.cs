using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{

    public class Gun : Item
    {
        [SerializeField] Transform shootPoint = null;
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] float fireRate = 10;

        private void OnDrawGizmos()
        {
            if (shootPoint == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(shootPoint.position, shootPoint.forward * 10);
        }
        public override void Use(List<EntityObject> entityToAffect)
        {
            Debug.Log("Use");
            Shoot();
        }

        float timeLastShot = 0;

        void Shoot()
        {

            // float timeSinceLastShot = Time.time - timeLastShot;
            // if (timeLastShot < 1 / fireRate)
            // {
            //     return;
            // }

            timeLastShot = Time.time;
            Debug.Log("Shoot");

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(shootPoint.forward * 10, ForceMode.Impulse);
            }
        }


    }
}