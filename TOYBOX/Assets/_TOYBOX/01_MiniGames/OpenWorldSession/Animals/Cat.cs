using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{
    public class Cat : Animal
    {
        [SerializeField] Transform target;

        private void Start()
        {
            InvokeRepeating(nameof(SetDestination), 0, 0.25f);
        }

        private void SetDestination()
        {
            if (target != null)
                MoveTo(target.position);
        }

        protected override void OnReachedDestination()
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.1f)
            {
                Destroy(target.gameObject);
            }
        }
    }
}

