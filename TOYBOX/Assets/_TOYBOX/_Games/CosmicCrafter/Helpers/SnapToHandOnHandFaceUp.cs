using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Toolkit.XR;
using ToyBox.Minigames.BeatEmUp;
using UnityEngine;

namespace ToyBox
{
    public class SnapToHandOnHandFaceUp : MonoBehaviour
    {
        bool wasHandFaceUp = false;

        // Update is called once per frame
        void Update()
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
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.5f).SetEase(Ease.InOutBack);
        }

        public void Deactivate()
        {
            transform.DOScale(0, 0.5f).SetEase(Ease.InOutBack).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
