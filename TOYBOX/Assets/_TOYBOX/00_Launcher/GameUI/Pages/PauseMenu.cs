using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ToyBox.UserInterface
{
    public class PauseMenu : Page
    {
        protected override UniTask OnClose()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask OnOpen()
        {
            return UniTask.CompletedTask;
        }
    }
}
