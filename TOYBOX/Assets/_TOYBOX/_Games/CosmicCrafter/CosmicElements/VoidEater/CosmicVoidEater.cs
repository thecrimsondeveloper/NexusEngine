using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Toolkit.Extras;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicVoidEater : MonoBehaviour, IRoundInitializable
    {
        [SerializeField] StarSlingshot slingshot = null;
        [SerializeField] float range = 10;

        [SerializeField] float eventHorizon = 1;
        [SerializeField] VisualEffect vfx = null;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, eventHorizon);
        }


        private void Start()
        {
            StarSlingshot.OnShootStar.AddListener(OnStarShoot);
        }

        private void OnStarShoot(CosmicStar star)
        {
            if (star)
            {
                this.star = star;
            }
        }

        CosmicStar star = null;



        float distance = 0;
        bool isConsumingStar = false;
        private void FixedUpdate()
        {
            if (star)
            {
                distance = Vector3.Distance(transform.position, star.transform.position);
                star.GravitateTowards(transform.position, Time.deltaTime);

                if (distance < eventHorizon && isConsumingStar == false)
                {
                    isConsumingStar = true;
                    star.enabled = false;
                    star.transform.DOMove(transform.position, 0.25f).OnComplete(() =>
                    {
                        Destroy(star.gameObject);

                        transform.DOPunchScale(Vector3.one * 0.5f, 0.5f).OnComplete(() =>
                        {
                            isConsumingStar = false;
                        });
                    });
                }
            }
        }

        public void InitializeRound(Vector3 position, Quaternion rotation)
        {
        }

        public void InitializeForRound(GameObject reference)
        {
            transform.position = reference.transform.position;
            transform.rotation = reference.transform.rotation;

            NumberRange range = new NumberRange(-45, 45);
            vfx.transform.rotation = Quaternion.Euler(range.Random(), 0, range.Random());
        }
    }
}
