using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.AI;

namespace ToyBox.Games.OpenWorld
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] NavMeshAgent agent;
        [SerializeField] float isWalkingThreshold = 0.01f;
        protected virtual void Update()
        {
            if (agent.velocity.magnitude < isWalkingThreshold && agent.remainingDistance <= agent.stoppingDistance)
            {
                OnReachedDestination();
            }
        }

        public void MoveTo(Vector3 targetPosition)
        {
            agent.SetDestination(targetPosition);
        }


        protected virtual void OnReachedDestination()
        {
            // Do something
        }



    }
}
