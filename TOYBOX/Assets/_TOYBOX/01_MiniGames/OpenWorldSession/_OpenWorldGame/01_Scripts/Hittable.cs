using System;
using System.Collections;
using System.Collections.Generic;
using ToyBox.Games.OpenWorld;
using ToyBox.Minigames.OpenWorld;
using UnityEngine;

namespace Toolkit.Items
{
    public class Hittable : Breakable
    {
        [SerializeField] int health = 1;
        [SerializeField] bool dropItemOnHit = false;
        public override Item Break()
        {
            Item droppedItem = null;
            if (dropItemOnHit && itemToDropPrefab != null)
            {
                droppedItem = Instantiate(itemToDropPrefab, transform.position, Quaternion.identity);
            }

            health -= 1;
            if (health <= 0)
            {
                Destroy(gameObject);
            }

            return droppedItem;
        }

    }
}
