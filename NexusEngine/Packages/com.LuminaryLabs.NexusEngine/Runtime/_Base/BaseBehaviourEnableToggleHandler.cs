using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseBehaviourEnableToggleHandler : BaseSequence<BaseBehaviourEnableToggleHandlerData>
{

    private Behaviour behaviour;
    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseBehaviourEnableToggleHandlerData currentData)
    {
        behaviour = currentData.behaviour;  
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {

        behaviour.enabled = !behaviour.enabled;

        this.Complete();
    }
}

public class BaseBehaviourEnableToggleHandlerData : BaseSequenceData
{
    public Behaviour behaviour;
}
