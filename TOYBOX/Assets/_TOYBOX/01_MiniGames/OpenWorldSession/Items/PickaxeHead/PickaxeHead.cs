using System.Collections;
using System.Collections.Generic;
using ToyBox.Minigames.OpenWorld;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{
    public class PickaxeHead : Item
    {
        void OnCollisionEnter(Collision other)
        {
            // if (other.gameObject.TryGetComponent(out Breakable breakable))
            // {
            //     Item droppedItem = breakable.Break();
            //     holder.Pickup(droppedItem);
            // }

            Debug.Log("Collision detected");

            if (other.gameObject.TryGetComponent(out StoneDeposit hittable))
            {
                Debug.Log("Stone deposit hit");
                hittable.Break();
            }
        }
    }
}
