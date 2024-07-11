using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class Pipe : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out BirdMovement bird))
            {
                bird.Die();
                Destroy(gameObject);
            }
        }
    }
}
