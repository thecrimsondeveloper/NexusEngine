using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class bucket : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                Debug.Log("Ball collision");
            }
        }
    }
    
}
