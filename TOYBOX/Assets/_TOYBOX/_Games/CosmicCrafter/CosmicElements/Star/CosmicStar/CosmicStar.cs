using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicStar : MonoBehaviour
    {
        [SerializeField] Rigidbody rb = null;
        [SerializeField] SphereCollider bounceCollider;
        [SerializeField] float maxSpeed = 10;
        [SerializeField] Transform contentParent = null;

        public List<StarCollectable> starCollectables = new List<StarCollectable>();
        public int NumberOfCollectables => starCollectables.Count;
        public float FloatTime => totalFloatTime;

        bool isFloating = false;
        float boostTime = 0;
        float boostTimer = 0;
        float totalFloatTime = 0;
        Vector3 boostDirection = Vector3.zero;
        public Transform centerTarget = null;
        AnimationCurve boostForceCurve = null;

        StarStats workingStarStats = new StarStats();
        public UnityEvent<StarStats> OnDie;
        bool trackAirTime = false;

        private void Awake()
        {
            contentParent.localScale = Vector3.zero;
        }
        private void Start()
        {
            if (Session.CurrentSession is CosmicCrafterSession cosmicCrafterSession)
            {
                centerTarget = cosmicCrafterSession.cosmicCenterOrb;
            }

            //scale up
            contentParent.DOScale(Vector3.one, 0.5f);
            bounceCollider.enabled = false;
        }

        private void OnDestroy()
        {
            OnDie?.Invoke(workingStarStats);
        }

        bool isDieing = false;

        private void Update()
        {
            if (trackAirTime)
            {
                workingStarStats.airTime += Time.deltaTime;
            }

        }
        private void FixedUpdate()
        {
            if (boostTimer < boostTime)
            {
                boostTimer += Time.deltaTime;
                float ratio = boostTimer / boostTime;
                float force = boostForceCurve != null ? boostForceCurve.Evaluate(ratio) : 0;
                rb.AddForce(boostDirection * 50 * Time.deltaTime * force);
            }
            if (isFloating)
            {
                totalFloatTime += Time.deltaTime;
            }

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
            if (centerTarget && Vector3.Distance(transform.position, centerTarget.position) > 0.75)
            {
                if (isDieing)
                    return;
                isDieing = true;

                transform.DOScale(0, 0.5f).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            }

        }

        public void Reflect(Vector3 direction, float reflectionMultiplier)
        {
            Vector3 reflected = Vector3.Reflect(rb.velocity, direction);
            rb.velocity = reflected * reflectionMultiplier;
        }


        public void Bounce(Vector3 direction, float bounceMultiplier)
        {
            rb.AddForce(direction * bounceMultiplier, ForceMode.Impulse);
        }

        public void FreezeVelocity()
        {
            rb.velocity = Vector3.zero;
        }

        public void SetKinematic(bool value)
        {
            rb.isKinematic = value;
        }

        [Button]
        public void Push(Vector3 direction, float force)
        {
            rb.AddForce(direction * force, ForceMode.Acceleration);
        }

        public void ReleaseStar(Vector3 direction, float boostTime, AnimationCurve forceCurve)
        {
            trackAirTime = true;
            bounceCollider.enabled = true;
            rb.isKinematic = false;
            rb.AddForce(direction);
            this.boostForceCurve = forceCurve;
            this.boostTime = boostTime;
            this.boostDirection = direction;
            isFloating = false;
        }

        public void GravitateTowards(Vector3 position, float force)
        {
            Vector3 dir = position - transform.position;

            float distance = dir.magnitude;

            float gravity = force / (distance * distance);

            rb.AddForce(dir.normalized * gravity);
        }



        public async UniTask AsyncShrink(float time = 0.5f)
        {
            Shrink(time);
            await UniTask.Delay((int)(time * 1000f));
        }
        public async UniTask AsyncGrow(float time = 0.5f)
        {
            Grow(time);
            await UniTask.Delay((int)(time * 1000f));
        }

        public void Shrink(float time = 0.5f)
        {
            contentParent.DOScale(0, time);
        }

        public void Grow(float time = 0.5f)
        {
            contentParent.DOScale(1, time);
        }

        public void OnConsumedByBlackHole(CosmicBlackHole blackHole)
        {
            //loop through each collectable star and move it to the black hole
            foreach (StarCollectable starCollectable in starCollectables)
            {
                starCollectable.OnCollectedByBlackHole(blackHole);
            }
        }
    }

    public struct StarStats
    {
        public float airTime;
        public int bounces;
    }
}
