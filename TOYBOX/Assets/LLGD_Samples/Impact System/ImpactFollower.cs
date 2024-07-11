using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit
{
    [RequireComponent(typeof(Rigidbody))]
    public class ImpactFollower : Impactor
    {
        [SerializeField] Transform target = null;
        [SerializeField] float speedMultiplier = 1f;
        [SerializeField] string targetTag = "";
        Rigidbody rb;



        private void OnDrawGizmos()
        {
            if (target == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);

            Gizmos.color = Color.blue;
            if (rb) Gizmos.DrawLine(transform.position, transform.position + rb.velocity);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }



        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            //invoke repeatedly until the target is found
            InvokeRepeating(nameof(FindTarget), 0, 0.5f);
        }
        void FindTarget()
        {
            if (target == null)
            {
                GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
                if (targetObject != null)
                {
                    target = targetObject.transform;
                }
            }
            else
            {
                //stop invoking
                CancelInvoke(nameof(FindTarget));
            }
        }




        List<Vector3> directions = new List<Vector3>();

        protected override void UpdateImpactInfo(ImpactInfo impactInfo)
        {
            impactInfo.direction = avegerageDirection;
        }

        public Vector3 avegerageDirection => directions.Count > 0 ? GetAverageDirection() : Vector3.zero;
        void FixedUpdate()
        {
            Vector3 dirToTarget = target.position - rb.position;

            rb.velocity = dirToTarget * speedMultiplier;

            //match rotation to the target
            rb.rotation = Quaternion.LookRotation(target.rotation.eulerAngles);

            //add the direction to the list
            directions.Add(dirToTarget);

            //remove the oldest direction if the list is too long
            if (directions.Count > 10)
            {
                directions.RemoveAt(0);
            }

            //draw the average direction
            Debug.DrawRay(transform.position, avegerageDirection * 5, Color.red);
        }

        Vector3 GetAverageDirection()
        {
            Vector3 averageDirection = Vector3.zero;
            foreach (Vector3 direction in directions)
            {
                averageDirection += direction;
            }
            averageDirection /= directions.Count;
            return averageDirection;
        }


    }
}
