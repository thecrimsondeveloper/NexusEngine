using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{

    public class Stone : Item
    {
        [SerializeField] Rigidbody rb = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            // rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
            // rb.AddForce(Random.insideUnitCircle * 5, ForceMode.Impulse);
        }
    }
}