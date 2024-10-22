using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

public class TransformModifyHandler : EntitySequence<TransformModifyData>
{
    public enum ModifyAction
    {
        SetPosition,
        SetRotation,
        SetScale,
        MoveTo,
        RotateTo,
        ScaleTo
    }

    private ModifyAction _action;
    public List<Transform> transforms;
    
    protected override UniTask Initialize(TransformModifyData currentData)
    {
        _action = currentData.modifyAction;
        if(currentData.transforms != null)
            transforms = currentData.transforms;
        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        switch (_action)
        {
            case ModifyAction.SetPosition:
                foreach (var t in transforms)
                {
                    t.position = currentData.targetPosition;
                }
                break;
            case ModifyAction.SetRotation:
                foreach (var t in transforms)
                {
                    t.rotation = Quaternion.Euler(currentData.targetRotation);
                }
                break;
            case ModifyAction.SetScale:
                foreach (var t in transforms)
                {
                    t.localScale = currentData.targetScale;
                }
                break;
            case ModifyAction.MoveTo:
                MoveToTarget().Forget();
                break;
            case ModifyAction.RotateTo:
                RotateToTarget().Forget();
                break;
            case ModifyAction.ScaleTo:
                ScaleToTarget().Forget();
                break;
        }
    }

    private async UniTask MoveToTarget()
    {
        foreach (var t in transforms)
        {
            while (Vector3.Distance(t.position, currentData.targetPosition) > 0.01f)
            {
                t.position = Vector3.Lerp(t.position, currentData.targetPosition, Time.deltaTime * currentData.moveSpeed);
                await UniTask.Yield();
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }
    }

    private async UniTask RotateToTarget()
    {
        foreach (var t in transforms)
        {
            while (Quaternion.Angle(t.rotation, Quaternion.Euler(currentData.targetRotation)) > 0.1f)
            {
                t.rotation = Quaternion.Slerp(t.rotation, Quaternion.Euler(currentData.targetRotation), Time.deltaTime * currentData.rotateSpeed);
                await UniTask.Yield();
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }
    }

    private async UniTask ScaleToTarget()
    {
        foreach (var t in transforms)
        {
            while (Vector3.Distance(t.localScale, currentData.targetScale) > 0.01f)
            {
                t.localScale = Vector3.Lerp(t.localScale, currentData.targetScale, Time.deltaTime * currentData.scaleSpeed);
                await UniTask.Yield();
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }
    }

    protected override UniTask Unload()
    {
        return UniTask.CompletedTask;
    }
}

[System.Serializable]
public class TransformModifyData : SequenceData
{
    public TransformModifyHandler.ModifyAction modifyAction;
    public List<Transform> transforms;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public Vector3 targetScale;
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;
    public float scaleSpeed = 1.0f;
}
