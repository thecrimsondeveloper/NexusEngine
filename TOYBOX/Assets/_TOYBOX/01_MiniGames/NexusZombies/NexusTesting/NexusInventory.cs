using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusInventory : ScriptableObject
    {
        public int maxItems = 10;
        public List<NexusItem> items = new List<NexusItem>();
        public Transform itemParent = null;
        public NexusItem activeItem = null;



        public void Pickup(NexusItem item)
        {
            if (items.Count >= maxItems)
            {
                return;
            }
            if (items.Contains(item))
            {
                return;
            }

            if (itemParent != null)
            {
                item.transform.SetParent(itemParent);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }


            items.Add(item);


        }

        public void Drop(NexusItem item)
        {
            if (!items.Contains(item))
            {
                return;
            }
            items.Remove(item);
        }

        public void SetActiveItem(NexusItem item)
        {
            // Deactivate all items
            foreach (var i in items)
            {
                bool found = i == item;
                i.gameObject.SetActive(found);
                if (found)
                {
                    activeItem = i;
                }
            }
        }

        public void UseActiveItem(NexusPlayer player)
        {
            if (activeItem != null)
            {
                activeItem.Use(player);
            }
        }



    }
}
