using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class StarCollectable : MonoBehaviour, IRoundInitializable
    {
        [SerializeField] CosmicStar star = null;
        [SerializeField] Rigidbody rb = null;
        [SerializeField] float followSpeed = 1.0f;
        [SerializeField] float maxSpeed = 10;
        [SerializeField] VisualEffect marker;

        [SerializeField] Vector3 roundStartLocalPos;

        Vector3 roundStarPosition => transform.parent.TransformPoint(roundStartLocalPos);

        public static UnityEvent<StarCollectable, CosmicStar> OnAnyStarCollected = new UnityEvent<StarCollectable, CosmicStar>();


        private void OnDrawGizmos()
        {

            if (Application.isPlaying == false)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.01f);


            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(roundStarPosition, 0.01f);

            //draw line between start and end
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, roundStarPosition);
        }

        [SerializeField] float distance = 0;
        [SerializeField] bool needsToReturn = false;

        Transform followTarget = null;
        void Update()
        {
            distance = Vector3.Distance(transform.position, roundStarPosition);
            if (star == null)
            {
                followTarget = null;
            }

            if (star && followTarget)
            {
                needsToReturn = true;
                Vector3 dirToStar = followTarget.transform.position - transform.position;
                Debug.DrawRay(transform.position, dirToStar, Color.green, 1);


                rb.velocity = Vector3.ClampMagnitude(dirToStar * followSpeed, maxSpeed);
            }
            else if (needsToReturn && distance > 0.001f)
            {
                rb.velocity = Vector3.ClampMagnitude((roundStarPosition - transform.position) * followSpeed, maxSpeed);
            }
            else if (distance < 0.001f)
            {
                needsToReturn = false;
                transform.position = roundStarPosition;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else if (distance > 2)
            {
                needsToReturn = true;
            }





        }


        private void OnTriggerEnter(Collider other)
        {

            // If the other object has a CosmicStar component and this object does not have a star
            if (other.TryGetComponent(out CosmicStar star) && this.star == null)
            {
                this.star = star;

                //if the starcollectables is empty set the target to the star
                if (star.starCollectables.Count == 0)
                {
                    followTarget = star.transform;
                }
                else
                {
                    //if the starcollectables is not empty set the target to the last starcollectable
                    followTarget = star.starCollectables[star.starCollectables.Count - 1].transform;
                }

                star.starCollectables.Add(this);
                OnAnyStarCollected.Invoke(this, star);
            }
        }
        public async void OnCollectedByBlackHole(CosmicBlackHole blackHole)
        {

            rb.isKinematic = true;
            transform.DOMove(blackHole.transform.position, 0.25f).OnComplete(() =>
            {
                blackHole.CollectStar(this);
            });

            await UniTask.Delay(200);


            gameObject.SetActive(false);
            transform.position = roundStarPosition;
            star = null;
            Vector3 currentScale = transform.localScale;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            transform.DOScale(currentScale, 0.5f).OnComplete(() =>
            {
                rb.isKinematic = false;
            });
        }

        public void InitializeForRound(GameObject reference)
        {
            transform.position = reference.transform.position;
            transform.rotation = reference.transform.rotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Debug.Log("StarCollectable AfterRoundStart");
            roundStartLocalPos = transform.localPosition;
            if (marker) marker.SetFloat("Intensity", 0.1f);
        }
    }
}
