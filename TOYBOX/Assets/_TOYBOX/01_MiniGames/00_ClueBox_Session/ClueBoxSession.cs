using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Toolkit.Sessions;

namespace ToyBox.Minigames.ClueBox
{

    public class ClueBoxSession : Session
    {
        ClueBoxSessionData sessionData = null;
        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as ClueBoxSessionData;
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