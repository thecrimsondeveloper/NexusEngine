using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Minigames.CosmicCrafter
{
    public class Cannon : MonoBehaviour, IEntity
    {
        const string FIRE_Animation_Name = "Fire";
        [SerializeField] CannonLever lever;
        [SerializeField] Transform firePoint;
        [SerializeField] Transform barrel;
        [SerializeField] CannonBall cannonBallPrefab;
        [SerializeField] Animation animation;
        [SerializeField] float strength = 5;
        [SerializeField] float cooldown = 1;
        [SerializeField] bool autoFire = false;
        [SerializeField] AudioSource fireSource;
        [SerializeField] OneGrabRotateTransformer oneGrabRotateTransformer;

        public UnityEvent OnFire;
        public UnityEvent OnReset;

        private void Start()
        {
            // lever.OnPulled.AddListener(Fire);

            //invoke repeat fire if auto fire is enabled

        }

        float lastTimeFired = 0;
        private void Update()
        {
            if (autoFire && Time.time - lastTimeFired > cooldown / 2)
            {
                Fire();
                lastTimeFired = Time.time;
            }
        }




        [Button, HideInEditorMode]
        public void Fire()
        {
            //if the animation is already playing the fire clip, return
            if (animation.IsPlaying(FIRE_Animation_Name))
                return;

            OnFire.Invoke();
            animation.Play(FIRE_Animation_Name);
        }


        //called by the animation
        public void Shoot()
        {
            fireSource.Play();
            CannonBall cannonBall = Instantiate(cannonBallPrefab, firePoint.position, Quaternion.LookRotation(firePoint.forward));
            cannonBall.AddForce(firePoint.forward * strength);
            // cannonBall.OnDie.AddListener(OnBallDie);
        }

        // void OnBallDie(StarStats starStats)
        // {
        //     Debug.Log("Ball died with airtime: " + starStats);
        //     //add to total airtime
        //     if (Session.CurrentSession is CosmicCrafterSession cosmicCrafterSession)
        //     {
        //         cosmicCrafterSession.AddStarStats(ballStats);
        //     }
        // }

        bool isActive = false;
        public async UniTask Activate()
        {
            if (isActive)
                return;

            isActive = true;

            Vector3 targetScale = transform.localScale;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            bool isScaled = false;

            transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                isScaled = true;
            });

            await UniTask.WaitUntil(() => isScaled);
        }

        public async UniTask Deactivate()
        {
            if (isActive == false)
                return;

            isActive = false;

            Vector3 targetScale = transform.localScale;
            bool isScaled = false;

            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                isScaled = true;
            });

            await UniTask.WaitUntil(() => isScaled);
            gameObject.SetActive(false);
            transform.localScale = targetScale;
        }
    }
}
