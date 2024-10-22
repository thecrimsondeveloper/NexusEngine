using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class RigidbodyKinematicModifierHandler : EntitySequence<RigidbodyKinematicModifierData>
{
    public enum ModifyAction
    {
        SetPosition,
        SetRotation
    }

    private ModifyAction _action;
    public List<Rigidbody> kinematicRigidbodies;

    protected override UniTask Initialize(RigidbodyKinematicModifierData currentData)
    {
        _action = currentData.modifyAction;
        if (currentData.kinematicRigidbodies != null)
            kinematicRigidbodies = currentData.kinematicRigidbodies;

        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        switch (_action)
        {
            case ModifyAction.SetPosition:
                foreach (var rb in kinematicRigidbodies)
                {
                    rb.position = currentData.targetPosition;
                }
                break;
            case ModifyAction.SetRotation:
                foreach (var rb in kinematicRigidbodies)
                {
                    rb.rotation = Quaternion.Euler(currentData.targetRotation);
                }
                break;
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
public class RigidbodyKinematicModifierData : SequenceData
{
    public RigidbodyKinematicModifierHandler.ModifyAction modifyAction;
    public List<Rigidbody> kinematicRigidbodies;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
}
