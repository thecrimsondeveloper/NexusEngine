using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Unity.VisualScripting;
using ToyBox.Minigames.OpenWorld;

namespace ToyBox.Games.OpenWorld

{

    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerHead playerHead;
        public Transform hand;
        [SerializeField] GameObject itemFramePrefab;
        [SerializeField] Transform itemFrameParent;
        [SerializeField] int maxinventorySize = 5;
        [SerializeField] List<Item> inventory = new List<Item>();
        [SerializeField] Item currentItem;
        [SerializeField] int currentItemIndex = 0;
        [SerializeField] Animation handMeleeAnimation;

        public List<EntityObject> lookedAtItems => playerHead.lookedAtEntities;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && handMeleeAnimation.isPlaying == false)
            {
                handMeleeAnimation.Play();

                if (playerHead.directEntity)
                    if (currentItem != null)
                    {
                        currentItem.Use(new List<EntityObject> { playerHead.directEntity });
                    }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetCurrentItem(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetCurrentItem(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetCurrentItem(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetCurrentItem(3);
            }
        }

        void SetCurrentItem(int index)
        {
            if (inventory.Count == 0) return;


            if (index > inventory.Count - 1)
            {
                index = 0;
            }

            if (currentItem != null)
            {
                currentItem.gameObject.SetActive(false);
            }

            currentItem = inventory[index];
            currentItemIndex = index;
            currentItem.gameObject.SetActive(true);
        }


        public bool Pickup(Item item)
        {
            if (inventory.Count >= maxinventorySize) return false;
            if (item == null) return false;

            inventory.Add(item);

            item.transform.SetParent(hand);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            item.Pickup(this);


            //if the inventory is empty set the item to be the current item
            if (inventory.Count == 1)
            {
                SetCurrentItem(0);
            }

            playerHead.Excluded(item);


            return true;
        }

        public MonoBehaviour RemoveInventoryItem(Item item)
        {
            if (item == null) return null;
            int index = 0;

            if (inventory.Contains(item))
            {
                index = inventory.IndexOf(item);
                inventory.RemoveAt(index);
                return item;
            }

            playerHead.RemoveExclusion(item);

            return null;
        }


        [Button("Remove From Inventory")]
        MonoBehaviour RemoveFromInventory(int index)
        {
            if (inventory.Count == 0) return null;
            if (index < 0 || index >= inventory.Count) return null;

            MonoBehaviour monoBehaviour = inventory[index];
            inventory.RemoveAt(index);

            return monoBehaviour;
        }

        [Button("Clear Inventory")]
        void ClearInventory()
        {
            foreach (var i in inventory)
            {
                Destroy(i.gameObject);
            }

            inventory.Clear();
        }



    }
}