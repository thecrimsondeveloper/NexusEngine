using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class RayCastHitHandler : BaseSequence<RayCastHitHandlerData>
{
    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(RayCastHitHandlerData currentData)
    {
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
    }
}

public class RayCastHitHandlerData : BaseSequenceData
{
    public RaycastHit raycastHit;
}
