using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class RigidbodyKinematicMovementHandler : EntitySequence<RigidbodyKinematicMovementData>
{
    public enum MovementAction
    {
        MoveToPosition,
        RotateToRotation,
        LerpPosition,
        SlerpRotation
    }

    private MovementAction _action;
    public List<Rigidbody> kinematicRigidbodies;

    protected override UniTask Initialize(RigidbodyKinematicMovementData currentData)
    {
        _action = currentData.movementAction;
        if (currentData.kinematicRigidbodies != null)
            kinematicRigidbodies = currentData.kinematicRigidbodies;

        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        switch (_action)
        {
            case MovementAction.MoveToPosition:
                MoveToPosition().Forget();
                break;
            case MovementAction.RotateToRotation:
                RotateToRotation().Forget();
                break;
            case MovementAction.LerpPosition:
                LerpPosition().Forget();
                break;
            case MovementAction.SlerpRotation:
                SlerpRotation().Forget();
                break;
        }
    }

    private async UniTask MoveToPosition()
    {
        foreach (var rb in kinematicRigidbodies)
        {
            rb.MovePosition(currentData.targetPosition);
            await UniTask.Yield();
        }
        Sequence.Finish(this);
        Sequence.Stop(this);
    }

    private async UniTask RotateToRotation()
    {
        foreach (var rb in kinematicRigidbodies)
        {
            rb.MoveRotation(Quaternion.Euler(currentData.targetRotation));
            await UniTask.Yield();
        }
        Sequence.Finish(this);
        Sequence.Stop(this);
    }

    private async UniTask LerpPosition()
    {
        foreach (var rb in kinematicRigidbodies)
        {
            while (Vector3.Distance(rb.position, currentData.targetPosition) > 0.01f)
            {
                rb.MovePosition(Vector3.Lerp(rb.position, currentData.targetPosition, Time.deltaTime * currentData.moveSpeed));
                await UniTask.Yield();
            }
        }
        Sequence.Finish(this);
        Sequence.Stop(this);
    }

    private async UniTask SlerpRotation()
    {
        foreach (var rb in kinematicRigidbodies)
        {
            while (Quaternion.Angle(rb.rotation, Quaternion.Euler(currentData.targetRotation)) > 0.1f)
            {
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(currentData.targetRotation), Time.deltaTime * currentData.rotateSpeed));
                await UniTask.Yield();
            }
        }
        Sequence.Finish(this);
        Sequence.Stop(this);
    }

    protected override UniTask Unload()
    {
        return UniTask.CompletedTask;
    }
}

[System.Serializable]
public class RigidbodyKinematicMovementData : SequenceData
{
    public RigidbodyKinematicMovementHandler.MovementAction movementAction;
    public List<Rigidbody> kinematicRigidbodies;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;
}
