using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Toolkit.XR;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;

namespace ToyBox
{
    public class GalaxyBounceHandMenu : MonoBehaviour
    {
        [SerializeField] Transform contentParent;
        bool wasHandFaceUp = false;
        private void Update()
        {
            bool handFaceUp = XRPlayer.RightHand.direction == XRPlayer.HandDirection.PalmUp;
            if (handFaceUp && !wasHandFaceUp)
            {
                transform.position = XRPlayer.RightHand.Position;
                Activate();
            }

            if (!handFaceUp && wasHandFaceUp)
            {
                Deactivate();
            }
            wasHandFaceUp = handFaceUp;
        }

        public void Activate()
        {
            contentParent.localScale = Vector3.zero;
            contentParent.gameObject.SetActive(true);
            contentParent.DOScale(1, 0.5f).SetEase(Ease.InOutBack);
        }

        public void Deactivate()
        {
            contentParent.DOScale(0, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                contentParent.gameObject.SetActive(false);
            });
        }

    }
}
