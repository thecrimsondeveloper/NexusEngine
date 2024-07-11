using System.Collections;
using System.Collections.Generic;
using ToyBox.Games.OpenWorld;
using UnityEngine;

namespace Toolkit.Items
{
    public class Breakable : EntityObject
    {
        [SerializeField] protected Item itemToDropPrefab = null;
        public virtual Item Break()
        {
            Item droppedItem = null;
            if (itemToDropPrefab != null)
            {
                droppedItem = Instantiate(itemToDropPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);

            return droppedItem;
        }
    }

}
