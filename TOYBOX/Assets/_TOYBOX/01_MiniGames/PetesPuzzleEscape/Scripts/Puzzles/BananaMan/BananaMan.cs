using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using Toolkit.Samples;
using Toolkit.Sequences;
using ToyBox.Minigames.EscapeRoom;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Sequence = Toolkit.Sequences.Sequence;

namespace ToyBox
{
    public class BananaMan : MonoSequence
    {
        [SerializeField] DialCompletable bananaManDial;
        [SerializeField] SliderCompletable bananaManSlider;
        [SerializeField] OneGrabTranslateTransformer bananaManCube;
        [SerializeField, FoldoutGroup("DialDependencies")] Transform dialTransformTarget;

        [SerializeField, FoldoutGroup("SFX")] AudioSource dialAudioSource;
        [SerializeField, FoldoutGroup("SFX")] AudioClip dialUnlockSound;
        [SerializeField, FoldoutGroup("SFX")] AudioClip dialRelockSound;
        [SerializeField, FoldoutGroup("SFX")] Animation spawnAnimation;
        [SerializeField, FoldoutGroup("SFX")] AnimationClip spawnClip;
        [SerializeField, FoldoutGroup("SFX")] AnimationClip despawnClip;

        private bool isActivated = false;

        private void Start()
        {
            bananaManDial.OnComplete.AddListener(DialSet);
            bananaManDial.OnReset.AddListener(DialReset);
            bananaManSlider.OnComplete.AddListener(SliderSet);
            bananaManCube.GetComponent<PointableUnityEventWrapper>().WhenRelease.AddListener(CheckBananaManCubePosition);

            dialTransformTarget.gameObject.SetActive(false);
        }

        private void Update()
        {
            float targetDialVal = dialTransformTarget.localEulerAngles.y / 360;
            bananaManDial.SetDialValue(targetDialVal);
        }

        void CheckBananaManCubePosition(PointerEvent args)
        {
            if (bananaManCube.transform.localPosition.x > 0.15f)
            {
                bananaManCube.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                Sequence.Finish(this);
            }
        }

        void SliderSet()
        {
            dialTransformTarget.gameObject.SetActive(true);
            bananaManSlider.knobTransformTarget.gameObject.SetActive(false);

            bananaManCube.Constraints.MaxX = new FloatConstraint
            {
                Constrain = true,
                Value = 0.1f
            };
        }

        void DialSet()
        {
            PlaySound(dialUnlockSound);

            bananaManCube.Constraints.MaxX = new FloatConstraint
            {
                Constrain = true,
                Value = 0.3f
            };
        }

        void DialReset()
        {
            PlaySound(dialRelockSound);
        }

        public void PlaySound(AudioClip clip)
        {
            dialAudioSource.PlayOneShot(clip);
        }

        protected override UniTask Finish()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            spawnAnimation.Play(spawnClip.name);
            UniTask.WaitWhile(() => spawnAnimation.isPlaying).Forget();
            isActivated = true;
        }

        protected override void OnStart()
        {
        }

        protected override void OnFinished()
        {
        }

        protected override void OnUnload()
        {
            spawnAnimation.Play(despawnClip.name);
            UniTask.WaitWhile(() => spawnAnimation.isPlaying).Forget();
            isActivated = false;
        }
    }
}
