using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{
    public class Newt : Animal
    {
        protected override void OnReachedDestination()
        {
            Destroy(gameObject);
        }
    }
}
