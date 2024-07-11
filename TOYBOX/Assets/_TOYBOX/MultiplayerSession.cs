using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using Toolkit.DependencyResolution;
using UnityEngine;
using Toolkit.Multiplayer;

namespace Toolkit.Sessions
{
    public abstract class MultiplayerSession : NetworkBehaviourWithState<MultiplayerSessionState>, ISession, INetworkRunnerCallbacks
    {

        [ShowInInspector, FoldoutGroup("Session", 1000), ShowIf(nameof(SessionData))]
        public virtual SessionData SessionData { get; set; } = null;

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float timeOnLoadStart { get; set; } = 0;

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float timeOnLoadEnd { get; set; } = 0;

        [ShowInInspector, FoldoutGroup("Session", 1000)]
        public float CurrentScore { get; set; } = 0;
        [SerializeField] private NetworkRunner runnerPrefab;


        [SerializeField] MultiplayerSessionState state;
        public override ref MultiplayerSessionState State { get => ref state; }
        [SerializeField, ShowIf(nameof(runner))] protected NetworkRunner runner;




        public virtual async UniTask OnLoad()
        {
            Debug.Log("Loading Multiplayer Session");
            if (runner == null)
            {
                runner = Instantiate(runnerPrefab);
                runner.AddCallbacks(this);
            }
            await UniTask.NextFrame();
        }

        public abstract void OnSessionStart();
        public abstract void OnSessionEnd();
        public virtual UniTask OnUnload() { return UniTask.CompletedTask; }


        [Button("Connect to Lobby Session")]
        public async void ConnectToLobbySession()
        {
            if (runner == null)
            {
                runner = Instantiate(runnerPrefab);
            }

            Debug.Log("Connecting to lobby session");


            await UniTask.NextFrame();


            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = "testSession",
            });
        }
        public virtual void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public virtual void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public virtual void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.LocalPlayer == player)
            {
                OnJoinedRoom(player);
            }
            else
            {
                OnOtherPlayerJoinedRoom(player);
            }
        }

        protected abstract void OnJoinedRoom(PlayerRef player);
        protected abstract void OnOtherPlayerJoinedRoom(PlayerRef otherPlayer);

        public virtual void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.LocalPlayer == player)
            {
                OnLeftRoom(player);
            }
            else
            {
                OnOtherPlayerLeftRoom(player);
            }
        }

        protected abstract void OnLeftRoom(PlayerRef player);
        protected abstract void OnOtherPlayerLeftRoom(PlayerRef otherPlayer);

        public virtual void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public virtual void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public abstract void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason);
        public abstract void OnConnectedToServer(NetworkRunner runner);
        public abstract void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason);


        public virtual void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {

        }

        public virtual void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public virtual void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public virtual void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public virtual void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public abstract void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken);

        public virtual void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {

        }

        public virtual void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {

        }

        public virtual void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public virtual void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }

    public struct MultiplayerSessionState : INetworkStruct
    {

    }
}
