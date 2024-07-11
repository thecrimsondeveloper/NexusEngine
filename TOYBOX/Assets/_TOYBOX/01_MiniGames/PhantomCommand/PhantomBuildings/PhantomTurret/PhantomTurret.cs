using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomTurret : PhantomBuilding
    {
        [Title("Turret Dependencies")]
        [SerializeField, Required] private PhantomTurretProjectile projectilePrefab;
        [SerializeField, Required] private Transform projectileSpawnPoint;
        [SerializeField, Required] private AudioSource audioSource;
        [SerializeField, Required] private AudioClip shootSound;

        [Title("Turret Settings")]
        [SerializeField] private float shootRate = 1f;
        [SerializeField] private float shootRange = 10f;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private float projectileStrength = 1f;

        private PhantomUnit currentTarget;
        private PhantomTurretProjectile currentProjectile;
        private float timer = 0f;
        public UnityEvent OnFire = new UnityEvent();

        public float Strength => projectileStrength;

        private List<PhantomUnit> UnitsInRange = new List<PhantomUnit>();

        private void Awake()
        {
            SetupProjectile();
        }

        private void Start()
        {
            InvokeRepeating(nameof(RefreshTargetsInRange), 0f, 0.1f);
        }

        private void Update()
        {
            currentTarget = GetClosestTarget();
            if (currentTarget == null)
            {
                return;
            }

            SetTowerRotation();

            timer += Time.deltaTime;
            if (timer >= shootRate)
            {
                Fire(currentTarget.transform);
            }
        }

        private void SetTowerRotation()
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        private void RefreshTargetsInRange()
        {
            UnitsInRange.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, shootRange);
            foreach (var collider in colliders)
            {
                PhantomUnit unit = collider.GetComponent<PhantomUnit>();
                if (unit != null)
                {
                    UnitsInRange.Add(unit);
                }
            }
        }

        private PhantomUnit GetClosestTarget()
        {
            return UnitsInRange.OrderBy(u => Vector3.Distance(u.transform.position, transform.position)).FirstOrDefault();
        }

        private void Fire(Transform target)
        {
            Debug.Log("Fire");
            SetupProjectile();

            if (currentProjectile != null)
            {
                // Calculate direction to the target
                Vector3 directionToTarget = (target.position - projectileSpawnPoint.position).normalized;

                // Set the projectile's rotation to face the target
                currentProjectile.transform.rotation = Quaternion.LookRotation(directionToTarget);

                currentProjectile.rb.isKinematic = false;
                currentProjectile.turret = this;
                currentProjectile.rb.AddForce(directionToTarget * projectileSpeed, ForceMode.Impulse);

                timer = 0f;
                audioSource.PlayOneShot(shootSound);
                OnFire.Invoke();
                currentProjectile = null; // Reset the current projectile
            }
        }

        private void SetupProjectile()
        {
            currentProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            if (shootRate > 0)
            {
                currentProjectile.transform.localScale = Vector3.zero;
                currentProjectile.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), shootRate).SetEase(Ease.OutCubic);
            }
        }
    }
}