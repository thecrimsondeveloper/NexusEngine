using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] Projectile bulletPrefab;
        [SerializeField] Transform bulletSpawnPoint;
        [SerializeField] PlayerAmmoVisuals ammoVisuals;

        [SerializeField] float spawnCooldown = 0.5f;
        [SerializeField] int maxBullets = 3;
        public int currentBullets = 3;

        float currCooldown = 0;
        private void Update()
        {
            if (currCooldown < 0)
            {
                currCooldown = 0;
            }
            else
            {
                currCooldown -= Time.deltaTime;
                return;
            }

            XRPlayer.XRPlayerHand hand = XRPlayer.RightHand;
            if (hand.indexPinchStrength > 0.5f)
            {
                SpawnBullet();
            }
        }

        public void SpawnBullet()
        {
            if (currentBullets <= 0 || currCooldown != 0)
            {
                return;
            }

            Projectile bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.playerMovement = GetComponent<PlayerMovement>();
            currentBullets--;
            currCooldown = spawnCooldown;

            ammoVisuals.SubtractAmmo(1);
        }
    }
}
