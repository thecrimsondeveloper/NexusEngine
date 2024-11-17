using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseSuperAlignTransformHandler : BaseSequence<BaseSuperAlignTransformHandlerData>
{

    private Transform targetReference;

    private bool alignPosition = true;
    private bool alignRotation = true;
    private bool alignScale = true;



    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseSuperAlignTransformHandlerData currentData)
    {
        
        targetReference = currentData.targetReference;

        alignPosition = currentData.alignPosition;
        alignRotation = currentData.alignRotation;
        alignScale = currentData.alignScale;


        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {


        if (targetReference == null || this.superSequence == null)
        {
            Debug.LogWarning("TargetReference is null or does not have a superSequence. Cannot align transform.");
            this.Complete();
            return;
        }
        
        Transform transformToAlign = this.superSequence.GetTransform();

        if (transformToAlign == null)
        {
            Debug.LogWarning("TransformToAlign is null. Cannot align transform.");
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

public class BaseSuperAlignTransformHandlerData : BaseSequenceData
{
    public Transform targetReference;
    public bool alignPosition = true;
    public bool alignRotation = true;
    public bool alignScale = true;
}
