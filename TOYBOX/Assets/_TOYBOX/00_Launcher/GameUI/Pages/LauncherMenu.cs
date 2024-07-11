using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace ToyBox.UserInterface
{
    public class LauncherMenu : Page
    {
        protected override async UniTask OnClose()
        {
            await ScaleTo(Vector3.zero, 0.5f);
        }

        protected override async UniTask OnOpen()
        {
            transform.localScale = Vector3.zero;
            await ScaleTo(Vector3.one, 0.5f);
        }
    }
}
