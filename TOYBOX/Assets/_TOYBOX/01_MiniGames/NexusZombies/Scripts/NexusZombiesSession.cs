using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox.Minigames.NexusZombies
{
    public class NexusZombiesSession : Session
    {
        [SerializeField] SessionData _sessionData;

        public override SessionData SessionData
        {
            get => _sessionData;
            set => _sessionData = value;
        }



        public override void OnSessionEnd()
        {
        }

        public override void OnSessionStart()
        {
            NexusState.SetState<ZombiesGameState>();
        }


    }
}
