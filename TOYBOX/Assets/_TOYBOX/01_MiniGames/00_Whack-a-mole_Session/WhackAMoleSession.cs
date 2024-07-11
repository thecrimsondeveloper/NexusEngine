using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extras.Playspace;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox.Minigames.WackAMole
{
    public class WhackAMoleSession : Session
    {
        WhackAMoleSessionData sessionData;
        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as WhackAMoleSessionData;
        }

        public override async UniTask OnLoad()
        {
            await UniTask.Delay(1000);
        }

        public override void OnSessionStart()
        {

        }
        public override void OnSessionEnd()
        {

        }


        public override async UniTask OnUnload()
        {
            await UniTask.Delay(1000);
        }
    }
}
