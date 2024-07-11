using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;

namespace ToyBox.Games.OpenWorld
{
    public class Mouse : Animal
    {
        [SerializeField] float mouseRange = 5f;

        private void Start()
        {
            InvokeRepeating(nameof(CheckForMouse), 0f, 0.1f);
        }




        void CheckForMouse()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, mouseRange);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Cat cat))
                {
                    MoveTo(transform.position + Random.insideUnitSphere * 5f);
                }
            }
        }

        protected override void OnReachedDestination()
        {
            base.OnReachedDestination();
            MoveTo(transform.position + Random.insideUnitSphere * 5f);
        }
    }
}
