using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    //Movement
    public Vector3 direction;

    //2D Movement
    public Vector2 direction2D;

    //Bullet
    public const byte MOUSEBUTTON0 = 1;
    public const byte MOUSEBUTTON1 = 2;
    public NetworkButtons buttons;
}
