using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Multiplayer
{
    public class RealtimeSpawner : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] bool spawnOnConnect = false;
        private void Awake()
        {
        }

        // private void DidConnectToRoom(Realtime realtime)
        // {
        //     Debug.Log("Connected to room!");
        //     if (spawnOnConnect)
        //     {
        //         Spawn();
        //     }
        // }

        // public void Spawn()
        // {
        //     InstantiateOptions options = new InstantiateOptions
        //     {
        //         ownedByClient = true,
        //         preventOwnershipTakeover = true,
        //     };


        //     GameObject obj = Realtime.Instantiate(prefab.name, options);
        //     OnSpawned(obj);
        // }


        // protected virtual void OnSpawned(GameObject spawned)
        // {
        //     // Override this method to add custom behavior when a marble is spawned

        // }
    }
}
