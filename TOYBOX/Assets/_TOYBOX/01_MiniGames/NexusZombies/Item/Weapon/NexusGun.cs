using System.Collections;
using System.Collections.Generic;
using Toolkit.Entity;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusGun : NexusWeapon
    {
        [SerializeField] float reloadTime = 1f;
        [SerializeField] int bulletsPerReload = 10;
        [SerializeField] int numberOfBulletsPerShot = 1;
        [SerializeField] ParticleSystem shootEffect;
        [SerializeField] Transform shootPoint;
        [SerializeField, Range(0, 1)] float bulletSpread = 0.1f;


        int bulletsInClip = 0;
        protected override void OnAttack()
        {
            Shoot();
        }

        void Shoot()
        {
            for (int i = 0; i < numberOfBulletsPerShot; i++)
            {
                FireBullet();
            }
        }
        protected virtual void FireBullet()
        {


            float spreadToAngle = bulletSpread * 90;
            float yAngle = Random.Range(-spreadToAngle, spreadToAngle);
            float xAngle = Random.Range(-spreadToAngle, spreadToAngle);

            Ray ray = new Ray(shootPoint.position, shootPoint.forward);

            Quaternion spreadRotation = Quaternion.Euler(xAngle, yAngle, 0f);
            ray.direction = ray.direction + (spreadRotation * shootPoint.forward);

            ParticleSystem newEffect = Instantiate(shootEffect, shootPoint.position, shootPoint.rotation, shootPoint.transform);
            newEffect.transform.forward = ray.direction;
            //if the current particle system is playing, stop it

            //set the forward of the shoot effeft to the direction of the ray


            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {

                if (hit.collider.TryGetComponent(out IAttackable attackable))
                {
                    Debug.Log(hit.collider.name);
                    attackable.Attack(this);
                }
            }
        }

        protected override void OnPickup(NexusPlayer player)
        {

        }

        protected override void OnDrop(NexusPlayer player)
        {

        }
    }
}
