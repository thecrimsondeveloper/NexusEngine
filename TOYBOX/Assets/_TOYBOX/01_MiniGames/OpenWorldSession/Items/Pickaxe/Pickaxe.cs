using System.Collections;
using System.Collections.Generic;
using Toolkit.Items;
using UnityEngine;
namespace ToyBox.Games.OpenWorld
{

    public class Pickaxe : Item
    {
        public override void Use(List<EntityObject> entityToAffect)
        {
            foreach (EntityObject entity in entityToAffect)
            {
                if (entity is Hittable hittable)
                {
                    hittable.Break();
                }
            }
        }
    }
}
