using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public static class SelectWall
    {
        public static OVRSceneAnchor[] wallsInRoom;
        public static OVRSceneAnchor defaultWall;
        public static void SetDefaultWall()
        {
            //get the distance from each of the walls in the room
            float[] distances = new float[wallsInRoom.Length];
            for (int i = 0; i < wallsInRoom.Length; i++)
            {
                distances[i] = Vector3.Distance(XRPlayer.HeadPose.position, wallsInRoom[i].transform.position);
            }
            //get the index of the closest wall
            int closestWallIndex = 0;
            for (int i = 0; i < distances.Length; i++)
            {
                if (distances[i] < distances[closestWallIndex])
                {
                    closestWallIndex = i;
                }
            }
            //set the closest wall as the default wall
            defaultWall = wallsInRoom[closestWallIndex];
        }
    }
}
