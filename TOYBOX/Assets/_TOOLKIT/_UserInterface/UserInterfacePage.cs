using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.UserInterfaces
{
    [Serializable]
    public class UserInterfacePage : MonoSequence
    {
        public enum NamingType
        {
            ObjectName,
            CustomName
        }

        [Title("Settings")]
        [SerializeField] NamingType namingType;

        [ShowIf(nameof(namingType), NamingType.ObjectName)]
        [SerializeField] string pageName;

        [SerializeField, BoxGroup("Animations")] private float pageOpenTime;
        [SerializeField, BoxGroup("Animations")] private float pageCloseTime;

        [Title("Debugging")]
        [ShowInInspector, ReadOnly]
        public string PageName => namingType == NamingType.ObjectName ? gameObject.name : pageName;

        [ShowInInspector] Vector3 startingScale;
        [ShowInInspector] Vector3 startingPosition;

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
            startingScale = transform.localScale;
            startingPosition = transform.position;

            transform.localScale = Vector3.zero;
            transform.DOScale(startingScale, pageOpenTime);
        }

        protected override void OnStart()
        {
            gameObject.SetActive(true);
        }

        protected override void OnFinished()
        {
            // Logic for when sequence is finished
        }

        protected override void OnUnload()
        {
            gameObject.SetActive(false);
        }

        public async UniTask UnLoad_Async()
        {
            bool isUnloaded = false;

            transform.DOScale(Vector3.zero, pageCloseTime)
            .OnComplete(() =>
            {
                isUnloaded = true;
            });

            await UniTask.WaitUntil(() => isUnloaded);
        }
    }
}
