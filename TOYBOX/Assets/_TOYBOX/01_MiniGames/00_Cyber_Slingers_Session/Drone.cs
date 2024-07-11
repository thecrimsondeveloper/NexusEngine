
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Minigames.CyberSlingers
{
    public class Drone : MonoBehaviour
    {
        public enum MovementType
        {
            GravitateToward,
            MoveToward
        }
        [SerializeField] MovementType movementType = MovementType.GravitateToward;
        [SerializeField] DroneSpawner spawner;
        [SerializeField] float speedMultiplier = 1;
        [SerializeField] float maxSpeed = 5;
        [SerializeField] float drag = 0.1f;
        [SerializeField, ShowIf("movementType", MovementType.GravitateToward)] float gravity = 9.8f;
        [SerializeField] Vector3[] targetPoints;
        [SerializeField] float distanceThreshold = 0.1f;


        [SerializeField] float avoidanceSpeedMultiplier = 1;
        [SerializeField] float awarenessDistance = 0.5f;
        [SerializeField] float avoidanceRate = 5;

        int currentTargetIndex = 0;
        bool isFollowingPath = true;

        [SerializeField] CharacterController characterController;


        Vector3 velocity = Vector3.zero;


        private void OnDrawGizmos()
        {
            if (targetPoints == null || targetPoints.Length == 0)
                return;

            for (int i = 0; i < targetPoints.Length; i++)
            {
                Gizmos.color = Color.red;
                Vector3 targetPoint = targetPoints[i] + transform.parent.position;
                Gizmos.DrawSphere(targetPoint, 0.1f);
                Gizmos.DrawWireSphere(targetPoint, distanceThreshold);
            }
        }

        public void Inititalize(DroneSpawner spawner, Vector3[] targetPoints)
        {
            this.spawner = spawner;
            this.targetPoints = new Vector3[targetPoints.Length];

            targetPoints.CopyTo(this.targetPoints, 0);
        }


        void FixedUpdate()
        {
            CheckForCollisions();
            if (isFollowingPath)
            {
                FollowPathUpdate();
            }
            else
            {
                AvoidUpdate();
            }
        }

        void AvoidUpdate()
        {
            //add force to avoid the drone
            characterController.Move(velocity * Time.deltaTime * speedMultiplier);
            velocity = Vector3.Lerp(velocity, Vector3.zero, drag * Time.deltaTime);
            //clamp the speed
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        void FollowPathUpdate()
        {
            if (targetPoints == null || targetPoints.Length == 0)
                return;

            if (Vector3.Distance(transform.position, targetPoints[currentTargetIndex]) < distanceThreshold)
            {
                currentTargetIndex++;
                if (currentTargetIndex >= targetPoints.Length)
                {
                    currentTargetIndex = 0;
                }
            }

            //get the target point in world space
            Vector3 targetPoint = targetPoints[currentTargetIndex] + transform.parent.position;

            if (movementType == MovementType.GravitateToward)
            {
                GravitateToward(targetPoint, gravity);
            }
            else
            {
                MoveToward(targetPoint);
            }

            characterController.Move(velocity * Time.deltaTime * speedMultiplier);
            velocity = Vector3.Lerp(velocity, Vector3.zero, drag * Time.deltaTime);

            //clamp the speed
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }


        void AddForce(Vector3 force)
        {
            velocity += force;
        }

        void GravitateToward(Vector3 target, float gravity = 9.8f)
        {


            float distanceToTarget = Vector3.Distance(target, transform.position);
            float timeToTarget = Mathf.Sqrt(2 * distanceToTarget / gravity);
            Vector3 forceVector = (target - transform.position) / timeToTarget;

            //apply speed and time.deltaTime to the force vector
            AddForce(forceVector);
        }

        void MoveToward(Vector3 target)
        {
            Vector3 forceVector = (target - transform.position);
            AddForce(forceVector);
        }


        Drone DroneInSight()
        {
            //check in the direction of the velocity if there is a drone
            RaycastHit hit;

            Debug.DrawRay(transform.position, velocity.normalized * awarenessDistance, Color.red, 1 / avoidanceRate);

            if (Physics.Raycast(transform.position, velocity.normalized, out hit, awarenessDistance))
            {
                if (hit.collider.TryGetComponent(out Drone drone))
                {
                    return drone;
                }
            }
            return null;
        }


        float timeAtLastCheck = 0;
        void CheckForCollisions()
        {
            //ensure that we are not checking too often
            if (timeAtLastCheck - Time.time < 1 / avoidanceRate)
            {
                return;
            }

            Drone drone = DroneInSight();
            if (drone)
            {
                AvoidDrone(drone);
            }
        }

        async void AvoidDrone(Drone drone)
        {
            Vector3 avoidanceDirection = (transform.position - drone.transform.position).normalized;
            Vector3 avoidanceForce = avoidanceDirection * avoidanceSpeedMultiplier;
            Vector3 randomDirection = Random.onUnitSphere * avoidanceSpeedMultiplier;

            //lerp between the two directions
            Vector3 avoidanceLerp = Vector3.Lerp(avoidanceDirection, randomDirection, 0.5f);


            velocity = avoidanceLerp;


            isFollowingPath = false;
            await UniTask.Delay(2000);
            isFollowingPath = true;
        }




    }
}
