using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicGravityPad : MonoBehaviour
    {
        [SerializeField] Animation onReflectAnimation;
        [SerializeField] Transform bounceArea;
        [SerializeField] float bounceMuliplier = 1f;
        [SerializeField] float pushMultiplier = 1f;
        [SerializeField] float reflectionMultiplier = 1f;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip reflectClip;

        public UnityEvent<CosmicStar> OnCollideWithStar = new UnityEvent<CosmicStar>();

        bool isBouncing = false;
        private void OnCollisionEnter(Collision other)
        {
            if (isBouncing)
                return;
            if (other.gameObject.TryGetComponent(out CosmicStar star))
            {
                Reflect(star);
            }
        }

        async void Reflect(CosmicStar star)
        {
            star.Reflect(bounceArea.up, reflectionMultiplier);
            star.Bounce(bounceArea.up, bounceMuliplier);

            //play one shot audio
            if (reflectClip)
            {
                audioSource.PlayOneShot(reflectClip);
            }

            OnCollideWithStar.Invoke(star);


            isBouncing = true;
            onReflectAnimation.Play();
            await UniTask.WaitWhile(() => onReflectAnimation.isPlaying);
            isBouncing = false;
        }

        Vector3 starPositionLocal;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out CosmicStar star))
            {
                star.Push(bounceArea.up, pushMultiplier * Time.deltaTime);
                starPositionLocal = transform.InverseTransformPoint(star.transform.position);
            }
        }
    }
}
