using System;
using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomUnitController : MonoBehaviour
    {
        PhantomPlayer owner;
        public List<PhantomUnit> units = new List<PhantomUnit>();
        public void Initialize(PhantomPlayer owner)
        {
            this.owner = owner;
        }

        public PhantomUnit Spawn(PhantomUnit unitPrefab)
        {
            PhantomUnit unit = Instantiate(unitPrefab);

            unit.Initialize(owner);

            units.Add(unit);
            return unit;
        }
    }
}
