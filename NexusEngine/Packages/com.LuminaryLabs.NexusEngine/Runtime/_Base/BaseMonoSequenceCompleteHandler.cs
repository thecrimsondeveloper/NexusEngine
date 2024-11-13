using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class BaseMonoSequenceCompleteHandler : BaseSequence<BaseMonoSequenceCompleteHandlerData>
{

    private List<MonoSequence> monoSequences;

    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseMonoSequenceCompleteHandlerData currentData)
    {
        monoSequences = currentData.monoSequences;

        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        if (monoSequences != null)
        {
            foreach (MonoSequence monoSequence in monoSequences)
            {
                if (monoSequence == null)
                {   
                    continue;
                }
                (monoSequence as ISequence).Complete();
            }
        }

        this.Complete();
    }
}

public class BaseMonoSequenceCompleteHandlerData : BaseSequenceData
{
    public List<MonoSequence> monoSequences;
}
