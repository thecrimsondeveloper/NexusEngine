using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class RigidbodyMovementHandler : EntitySequence<RigidbodyMovementData>
    {
        public enum MovementType
        {
            AddForce,
            AddImpulse,
            SetVelocity,
            WanderWithinBounds,
            SeekTarget,
            Patrol
        }

        public enum MovementConstraint
        {
            None,
            ColliderBounds,
            MeshRendererBounds
        }

        private MovementType _movementType;
        private MovementConstraint _movementConstraint;

        public List<Rigidbody> rigidbodies;

        private Bounds _movementBounds;

        private Transform target;
        private List<Vector3> patrolPoints;
        private float forceStrength = 1.0f;
        private float impulseStrength = 1.0f;
        private float velocityMagnitude = 1.0f;

        protected override UniTask Initialize(RigidbodyMovementData currentData)
        {
            _movementType = currentData.movementType;
            _movementConstraint = currentData.movementConstraint;

            if(currentData.target != null)
                target = currentData.target;
            if(currentData.patrolPoints != null)
                patrolPoints = currentData.patrolPoints;
            if(currentData.forceStrength != 0)
                forceStrength = currentData.forceStrength;
            if(currentData.impulseStrength != 0)
                impulseStrength = currentData.impulseStrength;
            if(currentData.velocityMagnitude != 0)
                velocityMagnitude = currentData.velocityMagnitude;
            // Set bounds based on the constraint type
            if (currentData.collider != null && _movementConstraint == MovementConstraint.ColliderBounds)
            {
                _movementBounds = currentData.collider.bounds;
            }
            else if (currentData.meshRenderer != null && _movementConstraint == MovementConstraint.MeshRendererBounds)
            {
                _movementBounds = currentData.meshRenderer.bounds;
            }

            if (currentData.rigidbodies != null)
                rigidbodies = currentData.rigidbodies;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            switch (_movementType)
            {
                case MovementType.AddForce:
                    ApplyForce().Forget();
                    break;
                case MovementType.AddImpulse:
                    ApplyImpulse().Forget();
                    break;
                case MovementType.SetVelocity:
                    SetVelocity().Forget();
                    break;
                case MovementType.WanderWithinBounds:
                    WanderWithinBounds().Forget();
                    break;
                case MovementType.SeekTarget:
                    SeekTarget().Forget();
                    break;
                case MovementType.Patrol:
                    Patrol().Forget();
                    break;
            }
        }

        private async UniTask ApplyForce()
        {
            foreach (var rb in rigidbodies)
            {
                Vector3 forceDirection = GetRandomDirectionWithinConstraints();
                rb.AddForce(forceDirection * forceStrength, ForceMode.Force);
                await UniTask.Yield();
            }
        }

        private async UniTask ApplyImpulse()
        {
            foreach (var rb in rigidbodies)
            {
                Vector3 impulseDirection = GetRandomDirectionWithinConstraints();
                rb.AddForce(impulseDirection * impulseStrength, ForceMode.Impulse);
                await UniTask.Yield();
            }
        }

        private async UniTask SetVelocity()
        {
            foreach (var rb in rigidbodies)
            {
                rb.velocity = GetRandomDirectionWithinConstraints() * velocityMagnitude;
                await UniTask.Yield();
            }
        }

        private async UniTask WanderWithinBounds()
        {
            foreach (var rb in rigidbodies)
            {
                while (true)
                {
                    Vector3 randomPosition = GetRandomPositionWithinConstraints();
                    Vector3 direction = (randomPosition - rb.position).normalized;
                    rb.AddForce(direction * forceStrength, ForceMode.Force);
                    await UniTask.Yield();
                }
            }
        }

        private async UniTask SeekTarget()
        {
            foreach (var rb in rigidbodies)
            {
                while (true)
                {
                    Vector3 targetPosition = target.position;
                    Vector3 direction = (targetPosition - rb.position).normalized;
                    rb.AddForce(direction * forceStrength, ForceMode.Force);
                    await UniTask.Yield();
                }
            }
        }

        private async UniTask Patrol()
        {
            foreach (var rb in rigidbodies)
            {
                foreach (var point in patrolPoints)
                {
                    Vector3 direction = (point - rb.position).normalized;
                    rb.AddForce(direction * forceStrength, ForceMode.Force);
                    await UniTask.Yield();
                }
            }
        }

        private Vector3 GetRandomDirectionWithinConstraints()
        {
            if (_movementConstraint == MovementConstraint.None)
            {
                return Random.insideUnitSphere.normalized;
            }

            // If there are bounds, constrain direction based on them
            Vector3 randomPointWithinBounds = GetRandomPositionWithinConstraints();
            return (randomPointWithinBounds - Vector3.zero).normalized; // Normalized vector from origin
        }

        private Vector3 GetRandomPositionWithinConstraints()
        {
            if (_movementConstraint == MovementConstraint.ColliderBounds || _movementConstraint == MovementConstraint.MeshRendererBounds)
            {
                // Generate a random point within the bounds
                return new Vector3(
                    Random.Range(_movementBounds.min.x, _movementBounds.max.x),
                    Random.Range(_movementBounds.min.y, _movementBounds.max.y),
                    Random.Range(_movementBounds.min.z, _movementBounds.max.z)
                );
            }

            return Vector3.zero;
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class RigidbodyMovementData : SequenceData
    {
        public RigidbodyMovementHandler.MovementType movementType;
        public RigidbodyMovementHandler.MovementConstraint movementConstraint;
        public List<Rigidbody> rigidbodies;
        public Collider collider;  // Define movement constraint via collider bounds
        public MeshRenderer meshRenderer;  // Define movement constraint via mesh renderer bounds
        public Transform target;
        public List<Vector3> patrolPoints;
        public float forceStrength = 1.0f;
        public float impulseStrength = 1.0f;
        public float velocityMagnitude = 1.0f;
    }
}
