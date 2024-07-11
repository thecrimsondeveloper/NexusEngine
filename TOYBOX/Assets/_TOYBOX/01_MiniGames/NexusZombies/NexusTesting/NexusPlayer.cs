using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using ToyBox;
using UnityEngine;


namespace Toolkit.NexusEngine
{
    public class NexusPlayer : NexusCharacter, IPlayer
    {
        public List<NexusInventory> _inventories = null;
        public List<NexusInventory> inventories { get => _inventories; set => _inventories = value; }


        public void RegisterInventory(NexusInventory inventory)
        {
            _inventories.Add(inventory);
        }

        public void UnregisterInventory(NexusInventory inventory)
        {
            _inventories.Remove(inventory);
        }

        [Button]
        public bool PickupItem(NexusItem item)
        {
            if (item.IsPickedUp) return false;
            bool pickedUp = false;
            foreach (var inventory in _inventories)
            {
                if (inventory.items.Count >= inventory.maxItems)
                {
                    continue;
                }

                inventory.Pickup(item);
                item.Pickup(this);

                return true;
            }

            return pickedUp;
        }



    }



}
