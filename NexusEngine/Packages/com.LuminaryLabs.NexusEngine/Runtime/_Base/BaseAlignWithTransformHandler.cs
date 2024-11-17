using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseAlignWithTransformHandler : BaseSequence<BaseAlignWithTransformHandlerData>
{
    private Transform transformToAlign;
    private Transform targetReference;

    private bool alignPosition = true;
    private bool alignRotation = true;
    private bool alignScale = true;




    protected override UniTask Initialize(BaseAlignWithTransformHandlerData currentData)
    {
        transformToAlign = currentData.transformToAlign;
        targetReference = currentData.targetReference;

        alignPosition = currentData.alignPosition;
        alignRotation = currentData.alignRotation;
        alignScale = currentData.alignScale;

        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        if (transformToAlign == null || targetReference == null)
        {
            Debug.LogWarning("TransformToAlign or TargetReference is null. Cannot align transform.");
            this.Complete();
            return;
        }

        if (alignPosition)
        {
            transformToAlign.position = targetReference.position;
        }

        if (alignRotation)
        {
            transformToAlign.rotation = targetReference.rotation;
        }

        if (alignScale)
        {
            transformToAlign.localScale = targetReference.localScale;
        }

        this.Complete();
    }
}

public class BaseAlignWithTransformHandlerData : BaseSequenceData
{
    public Transform transformToAlign;
    public Transform targetReference;

    public bool alignPosition = true;
    public bool alignRotation = true;
    public bool alignScale = true;
}
