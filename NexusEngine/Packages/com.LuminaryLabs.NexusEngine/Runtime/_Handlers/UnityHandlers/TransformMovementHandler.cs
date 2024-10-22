using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class TransformMovementHandler : EntitySequence<TransformMovementData>
{
    public enum MovementType
    {
        RandomMovement,
        Patrol,
        SeekTarget,
        WanderWithinBounds,
        Sway,
        Circle
    }

    public enum MovementConstraint
    {
        None,
        Boundaries,
        CustomZone
    }

    private MovementType _movementType;
    private MovementConstraint _movementConstraint;

    public List<Transform> objectsToMove;

    protected override UniTask Initialize(TransformMovementData currentData)
    {
        _movementType = currentData.movementType;
        _movementConstraint = currentData.movementConstraint;
        if (currentData.objectsToMove != null)
            objectsToMove = currentData.objectsToMove;

        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        switch (_movementType)
        {
            case MovementType.RandomMovement:
                MoveRandomly().Forget();
                break;
            case MovementType.Patrol:
                Patrol().Forget();
                break;
            case MovementType.SeekTarget:
                SeekTarget().Forget();
                break;
            case MovementType.WanderWithinBounds:
                WanderWithinBounds().Forget();
                break;
            case MovementType.Sway:
                SwayMovement().Forget();
                break;
            case MovementType.Circle:
                CircleMovement().Forget();
                break;
        }
    }

    private async UniTask MoveRandomly()
    {
        foreach (var obj in objectsToMove)
        {
            while (true)
            {
                Vector3 randomPosition = GetRandomPositionWithinConstraints();
                await MoveToPosition(obj, randomPosition, currentData.moveSpeed);
                await UniTask.Yield();
            }
        }
    }

    private async UniTask Patrol()
    {
        foreach (var obj in objectsToMove)
        {
            // Implement patrol logic: moving between waypoints
            foreach (var point in currentData.patrolPoints)
            {
                await MoveToPosition(obj, point, currentData.moveSpeed);
                await UniTask.Yield();
            }
        }
    }

    private async UniTask SeekTarget()
    {
        foreach (var obj in objectsToMove)
        {
            while (true)
            {
                Vector3 targetPosition = currentData.target.position;
                await MoveToPosition(obj, targetPosition, currentData.moveSpeed);
                await UniTask.Yield();
            }
        }
    }

    private async UniTask WanderWithinBounds()
    {
        foreach (var obj in objectsToMove)
        {
            while (true)
            {
                Vector3 randomPosition = GetRandomPositionWithinConstraints();
                await MoveToPosition(obj, randomPosition, currentData.moveSpeed);
                await UniTask.Yield();
            }
        }
    }

    private async UniTask SwayMovement()
    {
        foreach (var obj in objectsToMove)
        {
            while (true)
            {
                // Sway logic
                obj.position += new Vector3(0, Mathf.Sin(Time.time * currentData.swaySpeed), 0);
                await UniTask.Yield();
            }
        }
    }

    private async UniTask CircleMovement()
    {
        foreach (var obj in objectsToMove)
        {
            while (true)
            {
                obj.position = currentData.circleCenter + new Vector3(Mathf.Cos(Time.time * currentData.circleSpeed) * currentData.circleRadius, 0, Mathf.Sin(Time.time * currentData.circleSpeed) * currentData.circleRadius);
                await UniTask.Yield();
            }
        }
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

    private async UniTask MoveToPosition(Transform obj, Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(obj.position, targetPosition) > 0.01f)
        {
            obj.position = Vector3.Lerp(obj.position, targetPosition, Time.deltaTime * speed);
            await UniTask.Yield();
        }
    }

    protected override UniTask Unload()
    {
        return UniTask.CompletedTask;
    }
}

[System.Serializable]
public class TransformMovementData : SequenceData
{
    public TransformMovementHandler.MovementType movementType;
    public TransformMovementHandler.MovementConstraint movementConstraint;
    public List<Transform> objectsToMove;
    public Vector3 boundaryMin;
    public Vector3 boundaryMax;
    public Transform target;
    public List<Vector3> patrolPoints;
    public Vector3 circleCenter;
    public float circleRadius = 1.0f;
    public float moveSpeed = 1.0f;
    public float swaySpeed = 1.0f;
    public float circleSpeed = 1.0f;
}
