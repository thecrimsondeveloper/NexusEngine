using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Multiplayer
{
    // public class RealtimeManager : MonoSingleton<RealtimeManager>
    // {
    //     [SerializeField] Realtime _realtime;

    //     public static UnityEvent<Realtime> DidConnectToRoom = new UnityEvent<Realtime>();

    //     private void Start()
    //     {
    //         _realtime.didConnectToRoom += OnDidConnectToRoom;
    //     }


    //     void OnDidConnectToRoom(Realtime realtime)
    //     {
    //         Debug.Log("Connected to room!");
    //         DidConnectToRoom.Invoke(_realtime);

    //     }

    //     void Internal_ConnectToRoom(string roomName)
    //     {
    //         Debug.Log("Connecting to room: " + roomName);
    //         _realtime.Connect("TestRoom");
    //     }

    //     public void Internal_DisconnectFromRoom()
    //     {
    //         _realtime.Disconnect();
    //     }

    //     public static void ConnectToRoom(string roomName)
    //     {
    //         Debug.Log("Connecting to room: " + roomName);
    //         if (Instance != null)
    //         {
    //             Instance.Internal_ConnectToRoom(roomName);
    //         }
    //     }

    //     public static void DisconnectFromRoom()
    //     {
    //         if (Instance != null)
    //         {
    //             Instance.Internal_DisconnectFromRoom();
    //         }
    //     }
    // }
}
