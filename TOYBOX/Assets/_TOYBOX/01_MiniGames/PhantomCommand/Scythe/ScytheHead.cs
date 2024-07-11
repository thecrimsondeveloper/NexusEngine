using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.PhantomCommand
{
    public class ScytheHead : MonoBehaviour
    {
        public UnityEvent OnStab = new UnityEvent();

        private void OnCollisionEnter(Collision other)
        {
            Vector3 normal = other.contacts[0].normal;
            Vector3 forward = transform.forward;

            float dot = Vector3.Dot(normal, forward);

            if (dot < 0)
            {
                Stab();
            }
        }

        void Stab()
        {
            OnStab.Invoke();
        }

    }
}
