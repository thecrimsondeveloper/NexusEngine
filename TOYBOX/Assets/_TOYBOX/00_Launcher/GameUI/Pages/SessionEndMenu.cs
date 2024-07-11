using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolkit.Sessions;
using Cysharp.Threading.Tasks;

namespace ToyBox.UserInterface
{
    public class SessionEndMenu : Page
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
