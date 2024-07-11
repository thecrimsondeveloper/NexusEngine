using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ToyBox;
using UnityEngine;
using UnityEngine.AI;

namespace Toolkit.NexusEngine
{
    public class NexusNavMeshMovement : NexusMovement
    {
        [SerializeField] string targetName = "Target";
        [SerializeField] Transform target;
        [SerializeField] NavMeshAgent agent;

        protected override void OnInitializeBlock(NexusEntity entity)
        {
            if (ResolveComponent(ref agent))
            {

            }


            base.OnInitializeBlock(entity);
        }


        private void Update()
        {
            if (target != null)
            {
                SetTargetPoint();

                Debug.Log("Target Point: " + target.position, this);
            }
            else
            {
                try
                {
                    //find the gameobject with the name "ZombiesTarget"
                    target = GameObject.Find(targetName).transform;

                    Debug.Log("Target Found: " + target == null ? "No" : "Yes");
                }
                catch (System.Exception e)
                {
                    Debug.Log("Target Not Found: " + e.Message);
                }
            }
        }


        [Button]
        public void SetTargetPoint()
        {
            agent.SetDestination(target.position);
        }

    }
}
