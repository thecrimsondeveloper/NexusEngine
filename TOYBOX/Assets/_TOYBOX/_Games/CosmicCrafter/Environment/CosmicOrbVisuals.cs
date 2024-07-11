using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
using Unity.Mathematics;
using Sirenix.OdinInspector;

namespace ToyBox
{
    public class CosmicOrbVisuals : MonoBehaviour
    {

        public enum OrbState
        {
            FullScale,
            HandSize,
            Hidden
        }

        const string LEFT_HAND_POSITION_PROPERTY = "LeftHandPosition";
        const string RIGHT_HAND_POSITION_PROPERTY = "RightHandPosition";
        const string STAR_POSITION_PROPERTY = "StarPosition";
        [SerializeField] VisualEffect visualEffect = null;
        [SerializeField, HideInInspector] OrbState orbState = OrbState.FullScale;
        [SerializeField] float fullScale = 1f;
        [SerializeField] float handSize = 0.2f;


        public void SetState(OrbState state, bool force = false)
        {
            if (orbState == state && !force)
                return;

            orbState = state;

            switch (state)
            {
                case OrbState.FullScale:
                    ChangeToFullSize();
                    break;
                case OrbState.HandSize:
                    ChangeToHandSize();
                    break;
                case OrbState.Hidden:
                    ChangeToHidden();
                    break;
            }
        }

        [Button]
        public void SetStateToHandSize()
        {
            SetState(OrbState.HandSize);
        }

        [Button]
        public void SetStateToFullSize()
        {
            SetState(OrbState.FullScale);
        }

        [Button]
        public void SetStateToHidden()
        {
            SetState(OrbState.Hidden);
        }

        void ChangeToHandSize()
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
            }
            transform.DOLocalMove(new Vector3(0, 1.3f, 0), 1f).SetEase(Ease.InOutSine);
            transform.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.InOutSine);
            transform.DOScale(Vector3.one * 0.5f, 1f).SetEase(Ease.InOutSine);
        }

        void ChangeToFullSize()
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
            }

            transform.DOLocalMove(new Vector3(0, 1.3f, 0), 1f).SetEase(Ease.InOutSine);
            transform.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.InOutSine);
            transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutSine);
        }

        void ChangeToHidden()
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Kill(transform);
            }
            transform.DOLocalMove(new Vector3(0, 1.3f, 0), 1f).SetEase(Ease.InOutSine);
            transform.DOLocalRotate(Vector3.zero, 1f).SetEase(Ease.InOutSine);
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutSine);
        }



        // Update is called once per frame
        void Update()
        {

            if (visualEffect == null)
                return;


            visualEffect.SetVector3(LEFT_HAND_POSITION_PROPERTY, XRPlayer.LeftHand.Position);
            visualEffect.SetVector3(RIGHT_HAND_POSITION_PROPERTY, XRPlayer.RightHand.Position);

        }
    }
}
