using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit;
using ToyBox.WackAMole;
using UnityEngine;

namespace ToyBox
{
    public abstract class Mole : MonoBehaviour, IImpactable
    {
        protected const string ANIM_SPAWN_NAME = "Spawn";
        protected const string ANIM_IDLE_NAME = "Idle";
        protected const string ANIM_HIT_NAME = "Hit";

        [SerializeField] protected Animation animation = null;
        [SerializeField] protected Rigidbody bodyRigidbody = null;

        bool hasBeenHit = false;
        public bool finishedSpawning = false;

        Pose spawnPose = Pose.identity;

        async void Awake()
        {
            spawnPose = new Pose(transform.position, transform.rotation);
            await UniTask.Delay(2000);
            finishedSpawning = true;

        }

        void Start()
        {
            MoleSpawner.AddMole(this);

            //look at camera
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                lookAtTarget = mainCamera.transform;
            }
            else
            {
                GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
                if (cameras.Length > 0)
                {
                    lookAtTarget = cameras[0].transform;
                }
            }

            if (lookAtTarget != null)
            {
                transform.forward = -(lookAtTarget.position - transform.position).normalized;
            }
        }

        Transform lookAtTarget = null;

        protected virtual void Update()
        {
            if (finishedSpawning == false)
            {
                return;
            }

            float distance = Vector3.Distance(transform.position, spawnPose.position);

            if (distance > 0.2f)
            {
                Vector3 dir = (spawnPose.position - transform.position).normalized;
                bodyRigidbody.AddForce(dir * Time.deltaTime * 0.5f, ForceMode.Impulse);
            }
            else
            {
                Vector3 randomDir = Random.insideUnitSphere * 0.1f;
                bodyRigidbody.AddForce(randomDir * Time.deltaTime, ForceMode.Impulse);
            }

            OnUpdate();
        }



        private void OnDestroy()
        {
            MoleSpawner.RemoveMole(this);
        }

        public void DestroyMole(float time = 0)
        {
            Destroy(gameObject, time);
        }

        public void PlaySpawnAnimation()
        {
            animation.Play(ANIM_SPAWN_NAME);
        }

        public void PlayIdleAnimation()
        {
            animation.Play(ANIM_IDLE_NAME);
        }

        public void PlayHitAnimation()
        {
            animation.Play(ANIM_HIT_NAME);
        }

        protected abstract void OnInitialize();
        protected abstract void OnHit();
        protected abstract void OnUpdate();



        public void OnImpact(ImpactInfo info)
        {
            if (hasBeenHit)
            {
                return;
            }


            Debug.Log("IMPACTING");
            if (info.force > 1f)
            {
                OnHit();
                hasBeenHit = true;
                DestroyMole(2);
                bodyRigidbody.useGravity = true;
                bodyRigidbody.isKinematic = false;

                Vector3 forceDir = info.direction * 10f;

                Debug.DrawRay(transform.position, forceDir, Color.red, 5f);

                bodyRigidbody.AddForce(forceDir, ForceMode.Impulse);
            }
        }
    }
}
