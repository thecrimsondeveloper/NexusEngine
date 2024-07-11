using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using Fusion.Addons.Physics;
using System;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner networkRunner;
    [SerializeField] private List<NetworkPrefabRef> networkPrefabRefs;
    [SerializeField] private List<Transform> spawnPoints;

    private Dictionary<PlayerRef, NetworkObject> spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    //Host
    async void GameStart(GameMode mode)
    {
        //Creating Runner and saying user is giving input
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        gameObject.AddComponent<RunnerSimulatePhysics2D>();
        GetComponent<RunnerSimulatePhysics2D>().ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;
        networkRunner.ProvideInput = true;

        //Scene info
        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();

        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        //Creating a room
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });
    }

    //Disconnect
    void DisconnectFromRoom()
    {
        //if we are the local player
        if (networkRunner.IsPlayer)
        {
            //disconnect from the room
            networkRunner.Shutdown();
        }
    }


    private void OnGUI()
    {
        if (networkRunner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                GameStart(GameMode.Host);
            }

            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                GameStart(GameMode.Client);
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Disconnect From Room"))
            {
                DisconnectFromRoom();
            }
        }
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        // Implement your logic here
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        // Implement your logic here
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // Implement your logic here
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        // Implement your logic here
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        // Implement your logic here
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        // Implement your logic here
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        //set the direction 2D
        data.direction2D = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        input.Set(data);

        //send the input to all client        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // Implement your logic here
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        // Implement your logic here
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        // Implement your logic here
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        //get a random spawn point
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        //get a random character from the ones available
        NetworkPrefabRef prefab = networkPrefabRefs[UnityEngine.Random.Range(0, networkPrefabRefs.Count)];
        //remove from the list of available characters
        networkPrefabRefs.Remove(prefab);

        // if runner is server
        if (runner.IsServer)
        {
            Vector3 playerPos = spawnPoint.position;
            NetworkObject networkObject = runner.Spawn(prefab, playerPos, Quaternion.identity, player);
            //add the network object to a dictionary of spawnedcharacters
            spawnedCharacters.Add(player, networkObject);
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedPlayers.ContainsKey(player))
        {
            runner.Despawn(spawnedPlayers[player]);

            if (spawnedCharacters.ContainsKey(player))
            {
                runner.Despawn(spawnedCharacters[player]);
            }

            spawnedCharacters.Remove(player);

            spawnedPlayers.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        // Implement your logic here
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        // Implement your logic here
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // Implement your logic here
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        // Implement your logic here
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        // Implement your logic here
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        // Implement your logic here
    }

    // Update is called once per frame
    void Update()
    {

    }
}
