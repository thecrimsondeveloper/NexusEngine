using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Extras.Playspace;
using Sirenix.OdinInspector;

namespace Toolkit.Playspace
{
    public class SnapToPlayspace : MonoBehaviour
    {
        public float duration = 0.5f;
        public Ease ease = Ease.OutCubic;
        private void Start()
        {
            Playspace.onPlayspaceChanged.AddListener(Snap);
        }

        [Button]
        public void Snap()
        {
            if (DOTween.IsTweening(transform))
                DOTween.Kill(transform);
            transform.DOMove(Playspace.pose.position, duration).SetEase(ease);
            transform.DORotateQuaternion(Playspace.pose.rotation, duration).SetEase(ease);
        }
    }
}
