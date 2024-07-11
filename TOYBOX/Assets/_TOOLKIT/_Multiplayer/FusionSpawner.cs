using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public class FusionSpawner : SimulationBehaviour
{
    public GameObject prefab;

    [SerializeField] SpawnType spawnType = SpawnType.OnPlayerJoined;
    [SerializeField, ShowIf("spawnType", SpawnType.OnPlayerJoined)] bool spawnOnlyOnMaster;
    [SerializeField] bool spawnOnlyOnLocal;
    [SerializeField] float spawnDelay = 0f;
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 rotation;

    [SerializeField, Sirenix.OdinInspector.ReadOnly] NetworkObject spawnedObject;

    private void Start()
    {
        if (spawnType == SpawnType.OnPlayerJoined)
        {
            // Multiplayer.SubscribeToPlayerJoined(PlayerJoined);
        }
    }

    public void PlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        bool isMaster = runner.IsSharedModeMasterClient;
        if (spawnOnlyOnMaster && !isMaster)
        {
            return;
        }
        if (spawnOnlyOnLocal && (runner.LocalPlayer != player)) return;


        Debug.Log("Player joined and is spawning object. " + spawnOnlyOnMaster + " " + isMaster);
        Spawn();
    }

    public async void Spawn()
    {
        if (spawnedObject != null)
        {
            return;
        }

        await UniTask.Delay((int)(spawnDelay * 1000));
        if (this == null) return;

        Pose spawnPose = new Pose(position, Quaternion.Euler(rotation));
        // if (Multiplayer.TrySpawn(prefab, spawnPose, out spawnedObject))
        // {
        //     Debug.Log("Spawned " + prefab.name);
        //     spawnedObject.RequestStateAuthority();
        // }
    }



    enum SpawnType
    {
        OnPlayerJoined,
        Manual
    }
}
