using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class NexusPinchButton : MonoBehaviour
    {
        [SerializeField] Grabbable grabbable;
        [SerializeField] PointableUnityEventWrapper events;
        public UnityEvent OnPinch;

        Transform startParent = null;
        Pose startLocalPose = Pose.identity;

        private void Start()
        {
            startParent = transform.parent;
            startLocalPose = new Pose(transform.localPosition, transform.localRotation);

            events.WhenHover.AddListener(OnHover);
            events.WhenUnhover.AddListener(OnUnhover);
            events.WhenSelect.AddListener(OnSelect);
            events.WhenUnselect.AddListener(OnUnselect);

        }

        private void OnHover(PointerEvent eventData)
        {

        }

        private void OnUnhover(PointerEvent eventData)
        {

        }

        bool isPinching = false;
        [Button(DrawResult = false)]
        private void OnSelect(PointerEvent eventData)
        {
            if (isPinching)
            {
                return;
            }

            isPinching = true;
            OnPinch.Invoke();
            grabbable.enabled = false;

            transform.DOScale(0, 0.25f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                isPinching = false;
                Reset();
            });
        }

        private void OnUnselect(PointerEvent eventData)
        {
            Reset();
        }


        void Reset()
        {
            transform.parent = startParent;
            transform.localPosition = startLocalPose.position;
            transform.localRotation = startLocalPose.rotation;
            transform.localScale = Vector3.zero;


            transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                grabbable.enabled = true;
                transform.localScale = Vector3.one;
            });
        }


    }
}
