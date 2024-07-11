using System.Collections;
using System.Collections.Generic;
using ToyBox.Games.PhantomCommand;
using UnityEngine;

namespace ToyBox
{
    public class PhantomPlayer : MonoBehaviour
    {
        public float health = 100;
        public float money = 100;
        public float power = 100;

        public PhantomBuildingController buildingController;
        public PhantomUnitController unitController;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            buildingController.Initialize(this);
            unitController.Initialize(this);
        }

        public void GivePower(float amount)
        {
            power += amount;
            if (power < 0)
            {
                power = 0;
            }
        }

        public void GiveMoney(float amount)
        {
            money += amount;
            if (money < 0)
            {
                money = 0;
            }
        }

    }
}
