using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Cysharp.Threading.Tasks;

namespace ToyBox.Minigames.EscapeRoom
{
    public static class RoomSegmenter
    {
        public static GameObject CreateDividerFromTargetAnchors(Transform WallA, Transform WallB, Transform Floor, Transform Ceiling, GameObject _dividerPrefab)
        {
            //create a new game object to hold the divider
            GameObject divider = GameObject.Instantiate(_dividerPrefab);

            //set the rotation to be perpendicular to the walls
            divider.transform.rotation = Quaternion.LookRotation(WallA.position - WallB.position, Vector3.up);
            //get the center point between the two walls
            Vector3 center = (WallA.position + WallB.position) / 2;

            //based on the bounds of the divider, set the position to be at the center of the two walls
            divider.transform.position = center;


            //get the distance between the floor and the ceiling
            float height = Vector3.Distance(Floor.position, Ceiling.position);
            //set the scale of the divider to the distance between the walls and the height of the room
            divider.transform.localScale = new Vector3(Vector3.Distance(WallA.position, WallB.position), height, 0.05f);
            //move based on the scale offset
            divider.transform.position += Vector3.forward * (divider.transform.localScale.x / 2);


            return divider;
        }
    }
}
