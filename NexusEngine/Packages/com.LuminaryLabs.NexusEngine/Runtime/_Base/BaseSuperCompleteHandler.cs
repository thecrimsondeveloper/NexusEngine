using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseSuperCompleteHandler : BaseSequence<BaseSuperCompleteHandlerData>
{
    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseSuperCompleteHandlerData currentData)
    {
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override async void OnBegin()
    {
        await UniTask.NextFrame();
        if(this.superSequence != null)
        {
            this.superSequence.Complete();
        }
        Complete();
    }
}

public class BaseSuperCompleteHandlerData : BaseSequenceData
{
}
