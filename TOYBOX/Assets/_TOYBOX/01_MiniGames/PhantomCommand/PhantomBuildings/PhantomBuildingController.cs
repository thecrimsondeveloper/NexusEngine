using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomBuildingController : MonoBehaviour
    {
        PhantomPlayer owner;
        public List<PhantomBuilding> buildings = new List<PhantomBuilding>();

        public void Initialize(PhantomPlayer owner)
        {
            this.owner = owner;
        }

        public void Spawn(PhantomBuilding buildingPrefab)
        {
            PhantomBuilding building = Instantiate(buildingPrefab);
            building.Initialize(owner);


            buildings.Add(building);
        }
    }
}
