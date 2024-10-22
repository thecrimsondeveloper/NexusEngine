using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

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
        Boundaries,
        CustomZone
    }

    private MovementType _movementType;
    private MovementConstraint _movementConstraint;

    public List<Rigidbody> rigidbodies;

    protected override UniTask Initialize(RigidbodyMovementData currentData)
    {
        _movementType = currentData.movementType;
        _movementConstraint = currentData.movementConstraint;
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
            rb.AddForce(forceDirection * currentData.forceStrength, ForceMode.Force);
            await UniTask.Yield();
        }
    }

    private async UniTask ApplyImpulse()
    {
        foreach (var rb in rigidbodies)
        {
            Vector3 impulseDirection = GetRandomDirectionWithinConstraints();
            rb.AddForce(impulseDirection * currentData.impulseStrength, ForceMode.Impulse);
            await UniTask.Yield();
        }
    }

    private async UniTask SetVelocity()
    {
        foreach (var rb in rigidbodies)
        {
            rb.velocity = GetRandomDirectionWithinConstraints() * currentData.velocityMagnitude;
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
                rb.AddForce(direction * currentData.forceStrength, ForceMode.Force);
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
                Vector3 targetPosition = currentData.target.position;
                Vector3 direction = (targetPosition - rb.position).normalized;
                rb.AddForce(direction * currentData.forceStrength, ForceMode.Force);
                await UniTask.Yield();
            }
        }
    }

    private async UniTask Patrol()
    {
        foreach (var rb in rigidbodies)
        {
            foreach (var point in currentData.patrolPoints)
            {
                Vector3 direction = (point - rb.position).normalized;
                rb.AddForce(direction * currentData.forceStrength, ForceMode.Force);
                await UniTask.Yield();
            }
        }
    }

    private Vector3 GetRandomDirectionWithinConstraints()
    {
        switch (_movementConstraint)
        {
            case MovementConstraint.Boundaries:
                // Example of random direction constrained by boundaries
                return new Vector3(
                    Random.Range(currentData.boundaryMin.x, currentData.boundaryMax.x),
                    Random.Range(currentData.boundaryMin.y, currentData.boundaryMax.y),
                    Random.Range(currentData.boundaryMin.z, currentData.boundaryMax.z)
                ).normalized;
            case MovementConstraint.CustomZone:
                // Implement custom zone logic
                break;
        }
        return Random.insideUnitSphere.normalized;  // Default to random direction
    }

    private Vector3 GetRandomPositionWithinConstraints()
    {
        switch (_movementConstraint)
        {
            case MovementConstraint.Boundaries:
                return new Vector3(
                    Random.Range(currentData.boundaryMin.x, currentData.boundaryMax.x),
                    Random.Range(currentData.boundaryMin.y, currentData.boundaryMax.y),
                    Random.Range(currentData.boundaryMin.z, currentData.boundaryMax.z)
                );
            case MovementConstraint.CustomZone:
                // Implement custom zone logic
                break;
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
    public Vector3 boundaryMin;
    public Vector3 boundaryMax;
    public Transform target;
    public List<Vector3> patrolPoints;
    public float forceStrength = 1.0f;
    public float impulseStrength = 1.0f;
    public float velocityMagnitude = 1.0f;
}
