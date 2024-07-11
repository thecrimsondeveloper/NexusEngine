using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace ToyBox.Games.OpenWorld
{
    public class Item : EntityObject
    {
        [Title("Item References")]
        [Required]
        [SerializeField] ItemProfile profile = null;

        public ItemProfile Profile => profile;

        public string ID => profile.AssetID;


        [Title("Debugging")]
        [SerializeField] protected Player holder = null;

        public void Pickup(Player pickerUpper)
        {
            holder = pickerUpper;
        }

        public virtual void Use(List<EntityObject> entityToAffect)
        {

        }

    }

}