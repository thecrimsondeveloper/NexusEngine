using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class GenericRoundInitializable : MonoBehaviour, IRoundInitializable
    {
        public void InitializeForRound(GameObject reference)
        {
            transform.position = reference.transform.position;
            transform.rotation = reference.transform.rotation;
        }
    }
}
