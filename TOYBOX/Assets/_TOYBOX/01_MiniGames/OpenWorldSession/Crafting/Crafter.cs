using System.Collections;
using System.Collections.Generic;
using ToyBox.Games.OpenWorld;
using UnityEngine;
namespace Toolkit.Crafting
{
    public class Crafter : MonoSingleton<Crafter>
    {
        [SerializeField] ItemLibrary itemLibrary;


        void Create_Internal(CraftInteractable interactable, CraftInteractable interactor)
        {
            // Get the item from the interactable
            Item item = interactable.Item;

            // Get the item from the interactor
            Item interactorItem = interactor.Item;
            string interactionID = item.ID + "-" + interactorItem.ID;
            Debug.Log("Interaction ID: " + interactionID);

            // Check if the items can be crafted together
            if (itemLibrary.TryGetCraftableProfile(interactionID, out ItemProfile itemProfile))
            {
                // Create the new item
                Item newItem = Instantiate(itemProfile.Asset, interactable.transform.position, Quaternion.identity);

                // Call the OnCrafted event
                Destroy(interactable.gameObject);
                Destroy(interactor.gameObject);
            }
        }

        public static void Create(CraftInteractable interactable, CraftInteractable interactor)
        {
            if (Instance == null)
            {
                Debug.LogError("Crafter instance is null");
                return;
            }
            Instance.Create_Internal(interactable, interactor);
        }



    }
}
