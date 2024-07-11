using System.Collections;
using System.Collections.Generic;
using Toolkit.Pooling;
using ToyBox;
using UnityEngine;

namespace Toolkit.Extras
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] Transform shootPoint;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] PoolerController pooler;
        public float bulletSpeed = 10f;
        public float fireRate = 0.5f;
        private float nextFire = 0.0f;

        private void Start()
        {
            pooler.AddObjects(bulletPrefab, 100);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Fire();
                Debug.Log("Fire!");
            }
        }

        private void Fire()
        {
            SpawnData data = new SpawnData
            {
                prefab = bulletPrefab,
                position = shootPoint.position,
                rotation = shootPoint.rotation
            };
            //get a random object from the pool
            GameObject obj = pooler.Spawn(data);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.Shoot(bulletSpeed, 5f);
            Destroy(obj, 5f); //destroy the object when we are done with it, only IPoolable objects will be returned to pool
        }
    }
}
