using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ToyBox.UserInterface
{
    public abstract class Page : MonoBehaviour
    {

        [SerializeField] Transform contentParent;
        public async UniTask SetIsOpen(bool active)
        {
            if (active)
            {
                contentParent.gameObject.SetActive(true);
                await OnOpen();
            }
            else
            {
                await OnClose();
                await UniTask.NextFrame();
                contentParent.gameObject.SetActive(false);
            }
        }

        protected abstract UniTask OnOpen();
        protected abstract UniTask OnClose();

        protected async UniTask ScaleTo(Vector3 scale, float duration)
        {
            bool isTweening = true;
            transform.DOScale(scale, duration).SetEase(Ease.InOutBack).OnComplete(() => isTweening = false);
            await UniTask.WaitUntil(() => isTweening == false);
        }
    }
}
