using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class StarSlingshot : MonoBehaviour, IRoundInitializable
    {
        [SerializeField] Transform shootPoint = null;
        [SerializeField] Transform slingshotPullPoint = null;
        [SerializeField] Transform targetPoint = null;
        [SerializeField] PointableUnityEventWrapper grabbableEvents = null;

        [SerializeField] CosmicStar starPrefab = null;
        [SerializeField] float maxSlingshotPullDistance = 1.0f;
        [SerializeField] float forceMultiplier = 1.0f;
        [SerializeField] VisualEffect visualEffect = null;
        [SerializeField] AnimationCurve forceCurve = null;
        [SerializeField] bool resetEventPosition = true;
        [SerializeField] AudioSource OnReleaseSource = null;
        [SerializeField] AudioSource angelsSingingSource = null;

        public static UnityEvent<CosmicStar> OnShootStar = new UnityEvent<CosmicStar>();
        public static UnityEvent<CosmicStar> OnStartPull = new UnityEvent<CosmicStar>();
        // [SerializeField] AnimationCurve slingshotForceCurve = null;

        public UnityEvent<CosmicStar> OnReleaseStar = new UnityEvent<CosmicStar>();
        public UnityEvent<CosmicStar> OnGrabStar = new UnityEvent<CosmicStar>();


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(slingshotPullPoint.position, 0.01f);
            Gizmos.DrawWireSphere(shootPoint.position, 0.01f);
            Gizmos.DrawWireSphere(grabbableEvents.transform.position, 0.01f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(slingshotPullPoint.position, shootPoint.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(shootPoint.position, maxSlingshotPullDistance);
        }

        private void Start()
        {
            grabbableEvents.WhenRelease.AddListener(OnGrabRelease);
            grabbableEvents.WhenSelect.AddListener(OnGrab);

            SetupStar();
        }

        Vector3 pullPointPosition = Vector3.zero;
        bool isHeld = false;

        private void Update()
        {
            if (isHeld == false)
            {
                if (star == null && isSlingingStar == false)
                {
                    SetupStar();
                }
                return;
            }


            Vector3 direction = grabbableEvents.transform.position - shootPoint.position;

            Vector3 directionToGrab = Vector3.ClampMagnitude(direction, maxSlingshotPullDistance);

            Debug.DrawRay(grabbableEvents.transform.position, directionToGrab, Color.green);


            //set the target position to the grabbableEvents position
            pullPointPosition = shootPoint.position + directionToGrab;
            slingshotPullPoint.position = Vector3.Lerp(slingshotPullPoint.position, pullPointPosition, Time.deltaTime * 10);
            float distanceRatio = directionToGrab.magnitude / maxSlingshotPullDistance;


            // if (Physics.Raycast(shootPoint.position, direction.normalized, out RaycastHit hit))
            // {
            //     targetPoint.position = hit.point;
            // }
            // else
            // {
            // }
            targetPoint.position = shootPoint.position - directionToGrab;


            //set the hum volume based on the distance
            if (angelsSingingSource) angelsSingingSource.volume = distanceRatio;

            visualEffect.SetFloat("PullIntensity", distanceRatio);
        }





        [SerializeField] CosmicStar star = null;


        [Button]
        async void OnGrabRelease(PointerEvent eventData)
        {
            if (star)
            {

                ReleaseStar();
            }

            //set to zero after release star so the star is not parented to the slingshot anymore
            slingshotPullPoint.localPosition = Vector3.zero;
            targetPoint.localPosition = Vector3.zero;

            if (resetEventPosition)
                grabbableEvents.transform.localPosition = Vector3.zero;


            if (angelsSingingSource.isPlaying)
            {
                angelsSingingSource.DOFade(0, 0.5f).OnComplete(() =>
                {
                    angelsSingingSource.Stop();
                    angelsSingingSource.volume = 0;
                });
            }
            isHeld = false;
        }

        bool isSlingingStar = false;
        async void ReleaseStar()
        {
            isSlingingStar = true;
            Vector3 directionToShoot = shootPoint.position - star.transform.position;
            //create a star
            star.transform.SetParent(transform.parent);
            star.ReleaseStar(directionToShoot * forceMultiplier, directionToShoot.magnitude, forceCurve);
            if (OnReleaseSource)
            {
                OnReleaseSource.Play();
            }
            OnShootStar.Invoke(star);
            OnReleaseStar.Invoke(star);

            //destroy the star
            Destroy(star.gameObject, 10);
            star = null;

            visualEffect.SetBool("IsHolding", false);
            await UniTask.Delay(3000);
            visualEffect.enabled = false;
            SetupStar();

            isSlingingStar = false;
        }

        [Button]
        void OnGrab(PointerEvent eventData)
        {
            visualEffect.enabled = true;
            visualEffect.SetBool("IsHolding", true);
            OnStartPull.Invoke(star);
            OnGrabStar.Invoke(star);

            if (angelsSingingSource.isPlaying == false)
            {
                angelsSingingSource.Play();
            }
            isHeld = true;
        }
        void SetupStar()
        {
            star = Instantiate(starPrefab, slingshotPullPoint.position, Quaternion.identity, slingshotPullPoint);
        }

        public void InitializeForRound(GameObject reference)
        {
            transform.position = reference.transform.position;
            transform.rotation = reference.transform.rotation;
        }
    }
}
