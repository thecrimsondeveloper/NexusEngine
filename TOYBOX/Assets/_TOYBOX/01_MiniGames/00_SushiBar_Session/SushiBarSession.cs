using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Toolkit;
using Toolkit.Sessions;

namespace ToyBox.Minigames.SushiBar
{

    public class SushiBarSession : Session
    {
        SushiBarSessionData sessionData = null;
        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as SushiBarSessionData;
        }

        public override UniTask OnLoad()
        {
            return UniTask.CompletedTask;
        }
        public override void OnSessionStart()
        {

        }

        public override void OnSessionEnd()
        {

        }

        public override UniTask OnUnload()
        {
            return UniTask.CompletedTask;
        }
    }

}