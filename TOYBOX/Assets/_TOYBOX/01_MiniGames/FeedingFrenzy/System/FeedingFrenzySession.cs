using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Toolkit.Sessions;
using Toolkit.XR;
using Fusion;
using Fusion.Sockets;
using System;
using Unity.VisualScripting;
using Sirenix.OdinInspector;

namespace ToyBox.Minigames.FeedingFrenzy
{

    public class FeedingFrenzySession : MultiplayerSession
    {
        [SerializeField] FeedingFrenzySessionData sessionData = null;
        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as FeedingFrenzySessionData;
        }



        public enum GameState { Idle, Lobby, Starting, Running, Ending }
        [SerializeField] GameState gameState = GameState.Idle;
        [SerializeField] Transform arena;

        [SerializeField] int _soldierPlayerCount;
        [SerializeField] int _hippoPlayerCount;

        [Networked, OnChangedRender(nameof(OnSoldierPlayerCountChanged))]
        public int soldierPlayerCount { get => _soldierPlayerCount; set => _soldierPlayerCount = value; }

        [Networked, OnChangedRender(nameof(OnHippoPlayerCountChanged))]
        public int hippoPlayerCount { get => _hippoPlayerCount; set => _hippoPlayerCount = value; }

        void OnSoldierPlayerCountChanged(int previous, int current)
        {

        }

        void OnHippoPlayerCountChanged(int previous, int current)
        {

        }

        public override async void OnSessionStart()
        {
            runner.AddCallbacks(this);
            Debug.Log("Starting Feeding Frenzy Session");
            //set idle state
            CheckGameState();
            await UniTask.Delay(2000);
            // ConnectToLobbySession();
        }

        public override void OnSessionEnd()
        {

        }

        void SetupArenaForSoldierPlayer()
        {
            arena.localPosition = Vector3.zero;
            arena.localScale = Vector3.one * 50;
        }

        void SetupArenaForHippoPlayer()
        {

        }

        public void SetGameState(GameState state, bool force = false)
        {
            if (!force && gameState == state)
                return;
            gameState = state;
            CheckGameState();
        }

        void CheckGameState()
        {
            Debug.Log("Game State: " + gameState);
            switch (gameState)
            {
                case GameState.Idle:
                    OnIdle();
                    break;
                case GameState.Lobby:
                    OnEnterLobby();
                    break;
                case GameState.Starting:
                    OnRoundSetup();
                    break;
                case GameState.Running:
                    OnRoundBegin();
                    break;
                case GameState.Ending:
                    OnRoundEnd();
                    break;
            }
        }

        void OnIdle()
        {
            if (sessionData.playerMode == FeedingFrenzySessionData.PlayerMode.Soldier)
            {
                SetupArenaForSoldierPlayer();
            }
            else
            {
                // playerObject.GetComponent<FFHippo>().isLocalHippo = true;
                SetupArenaForHippoPlayer();
            }
        }

        NetworkObject playerObject;
        void OnEnterLobby()
        {

            GameObject playerPrefab = sessionData.playerMode == FeedingFrenzySessionData.PlayerMode.Hippo ? sessionData.hippoPlayerPrefab : sessionData.soldierPlayerPrefab;

            if (sessionData.playerMode == FeedingFrenzySessionData.PlayerMode.Hippo)
            {
                hippoPlayerCount++;
            }
            else
            {
                soldierPlayerCount++;
            }

            playerObject = runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, runner.LocalPlayer);
            //spawn the player within the specified radius 
            playerObject.transform.parent = transform;


        }

        void OnSpawnSoldier()
        {
            if (playerObject == null) return;

            //set the position of the player
            //get the ffso
            if (playerObject.TryGetBehaviour(out FFSoldier ffPlayer))
            {

            }
        }

        void OnHippoEnterLobby()
        {
            if (playerObject == null) return;

            //set the position of the player
            //get the ffso
            if (playerObject.TryGetBehaviour(out FFHippo ffPlayer))
            {

            }

        }

        void OnRoundSetup()
        {
            //any animation and setup
        }

        void OnRoundBegin()
        {
            //set the position of the player


        }

        void OnRoundEnd()
        {

        }

        protected override void OnJoinedRoom(PlayerRef player)
        {
            SetGameState(GameState.Lobby);

        }

        protected override void OnOtherPlayerJoinedRoom(PlayerRef otherPlayer)
        {
        }

        protected override void OnLeftRoom(PlayerRef player)
        {
        }

        protected override void OnOtherPlayerLeftRoom(PlayerRef otherPlayer)
        {
        }

        public override void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public override void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public override void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public override void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }
    }

}