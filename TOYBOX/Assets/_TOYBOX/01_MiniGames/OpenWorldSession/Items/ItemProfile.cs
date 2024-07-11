using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Library;
using ToyBox.Minigames.OpenWorld;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{
    [CreateAssetMenu(fileName = "ItemProfile", menuName = "ToyBox/Items/ItemProfile")]
    public class ItemProfile : AssetProfile<Item>
    {
        [Title("Crafting Settings")]
        [SerializeField] string craftID = "";
        [Title("Item References")]
        [SerializeField] bool canBeCrafted = false;

        [ShowIf(nameof(canBeCrafted))]
        [SerializeField] private List<ItemProfile> ingredients = new List<ItemProfile>();

        public bool CanBeCrafted => canBeCrafted;
        public List<ItemProfile> Ingredients => ingredients;


        //add up all the ingredients in the item profile


        public string CraftID => craftID;

        private void OnValidate()
        {
            if (canBeCrafted)
            {
                if (ingredients.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (ingredients[i] == null)
                    {
                        ingredients.RemoveAt(i);
                    }
                }
                craftID = string.Join("-", ingredients.ConvertAll(x => x.AssetID));
                //SAVE
                UnityEditor.EditorUtility.SetDirty(this);
            }




        }



    }
}
