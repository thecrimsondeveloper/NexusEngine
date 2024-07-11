using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sessions;
using TMPro;
using ToyBox.Sessions;
using UnityEngine;

namespace ToyBox.UserInterface
{
    public class SessionPlayingMenu : Page
    {
        [SerializeField] TMP_Text outputText;
        protected override async UniTask OnClose()
        {
            await ScaleTo(Vector3.zero, 0.5f);
        }

        protected override async UniTask OnOpen()
        {
            await ScaleTo(Vector3.one, 0.5f);
            string sessionTitle = CurrentSession.Title;
            outputText.text = "Session: " + sessionTitle;
        }
    }
}
