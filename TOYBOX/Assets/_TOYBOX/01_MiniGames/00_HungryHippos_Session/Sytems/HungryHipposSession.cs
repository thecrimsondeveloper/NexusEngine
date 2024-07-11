using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toybox.Minigame;
using UnityEngine;
using Toolkit.Sessions;
using Toolkit.Multiplayer;
using ToyBox.Minigames.HungryHippos;
using Sirenix.OdinInspector;

namespace Toybox.Minigame.HungryHippos
{

    public class HungryHipposSession : Session
    {
        HungryHipposSessionData sessionData = null;
        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as HungryHipposSessionData;
        }

        public override void OnSessionEnd()
        {
            throw new System.NotImplementedException();
        }

        public override void OnSessionStart()
        {
            throw new System.NotImplementedException();
        }

        // [SerializeField] HungryHipposNetworkedSession networkedSessionPrefab = null;
        // [SerializeField] RealtimeSpawner marbleSpawner = null;
        // [SerializeField] RealtimeSpawner hippoSpawner = null;

        // private void Start()
        // {
        //     RealtimeManager.DidConnectToRoom.AddListener(OnDidConnectToRoom);
        // }

        // private void OnDidConnectToRoom(Realtime realtime)
        // {
        //     if (realtime.clientID == 0)
        //     {
        //         // hippoSpawner.Spawn();
        //         marbleSpawner.Spawn();
        //         SpawnNetworkedSession();
        //     }
        //     else
        //     {
        //         marbleSpawner.Spawn();
        //     }
        // }

        // void SpawnNetworkedSession()
        // {
        //     InstantiateOptions options = new InstantiateOptions
        //     {
        //         ownedByClient = false,
        //         preventOwnershipTakeover = true,
        //         destroyWhenLastClientLeaves = true,
        //         destroyWhenOwnerLeaves = false,
        //     };

        //     Realtime.Instantiate(networkedSessionPrefab.name, options);
        // }

        // protected override UniTask OnLoad()
        // {
        //     return UniTask.CompletedTask;
        // }
        // protected override void OnSessionStart()
        // {
        //     RealtimeManager.ConnectToRoom("TestRoom");
        // }

        // protected override void OnSessionEnd()
        // {

        // }

        // protected override UniTask OnUnload()
        // {
        //     return UniTask.CompletedTask;
        // }


    }

}