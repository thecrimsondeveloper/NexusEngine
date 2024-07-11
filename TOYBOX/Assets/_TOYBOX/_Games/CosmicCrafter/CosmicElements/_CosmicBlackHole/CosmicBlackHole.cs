using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Toolkit.Extras;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicBlackHole : MonoBehaviour, IRoundInitializable
    {

        [SerializeField] StarSlingshot slingshot = null;
        [SerializeField] float range = 0.1f;
        [SerializeField] float gravityMultiplier = 2f;
        [SerializeField] int numberOfStarsToComplete = 3;
        [SerializeField] AnimationCurve distanceOverRangeMultiplier = null;
        [SerializeField] VisualEffect vfx = null;
        public float CurrentCharge => currentCharge;
        public float ChargePercentage => chargeIntensity * 100;
        public UnityEvent<CosmicStar> WhenConsumedStar = new UnityEvent<CosmicStar>();


        public UnityEvent<CosmicStar> OnHitByStar = new UnityEvent<CosmicStar>();
        public static UnityEvent<StarCollectable, float> OnHitByCollectableStar = new UnityEvent<StarCollectable, float>();



        float distance = 0;
        CosmicStar star = null;

        [SerializeField] float chargeDepletion = 1;
        [SerializeField] float chargeToAdd = 2;
        [SerializeField] float chargeToComplete = 6;

        [SerializeField] float chargeIntensity = 0;
        [SerializeField] float currentCharge = 0;




        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        private void Start()
        {
            StarSlingshot.OnShootStar.AddListener(OnStarShoot);
        }
        private void FixedUpdate()
        {
            if (star)
            {
                distance = Vector3.Distance(transform.position, star.transform.position);
                float multiplier = distanceOverRangeMultiplier.Evaluate(distance / range) * gravityMultiplier;
                star.GravitateTowards(transform.position, Time.deltaTime * multiplier);
            }
        }

        private void Update()
        {

            if (currentCharge > 0)
            {
                currentCharge -= Time.deltaTime * chargeDepletion;
                RefreshVFX();
            }
            chargeIntensity = currentCharge / chargeToComplete;
        }

        void RefreshVFX()
        {
            if (vfx)
            {
                vfx.SetFloat("Intensity", chargeIntensity);
            }
        }
        private void OnStarShoot(CosmicStar star)
        {
            if (star)
            {
                this.star = star;
            }
        }

        float timeAtLastStar = 0;
        bool hitRecently = false;

        public float lastFloatTime = 0;

        [Button]
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CosmicStar star))
            {
                Debug.Log("Hit by Star");
                HitByStar(star);
                //ReactToStar();
                //set the public data to collect here
                lastFloatTime = star.FloatTime;
                timeAtLastStar = Time.time;
            }

        }

        void ReactToStar()
        {
            currentCharge += chargeToAdd;
            if (currentCharge >= chargeToComplete)
            {
                (this as ICompletable).Complete();
            }
        }

        public void CollectStar(StarCollectable star)
        {
            OnHitByCollectableStar.Invoke(star, chargeIntensity);
            ReactToStar();
        }
        public int numberOfStars = 0;
        private CosmicStar currentStar = null;
        void HitByStar(CosmicStar star)
        {
            OnHitByStar.Invoke(star);
            currentStar = star;
            star.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
            {
                star.OnConsumedByBlackHole(this);
                WhenConsumedStar.Invoke(star);
                numberOfStars = star.NumberOfCollectables;
                Destroy(star.gameObject);
            });
        }


        public void Reset()
        {
            currentCharge = 0;
            RefreshVFX();
        }
        public void InitializeForRound(GameObject reference)
        {
            transform.position = reference.transform.position;
            transform.rotation = reference.transform.rotation;

            NumberRange range = new NumberRange(-45, 45);
            vfx.transform.rotation = Quaternion.Euler(range.Random(), 0, range.Random());

            numberOfStars = 0;
        }
    }
}
