using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEngine.AI;

public class WaitForSuperNavAgentReachDestination : BaseSequence<WaitForSuperNavAgentReachDestinationData>
{

    NavMeshAgent navMeshAgent;
    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(WaitForSuperNavAgentReachDestinationData currentData)
    {

        if((this as ISequence).TryGetUnityComponent(out navMeshAgent))
        {
            Debug.Log("NavMeshAgent found");
            return UniTask.CompletedTask;
        }
        else
        {
            Debug.LogError("NavMeshAgent not found");
            return UniTask.CompletedTask;
        }
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        if(navMeshAgent != null)
        {
            Debug.Log("Waiting for NavMeshAgent to reach destination...");
            StartWaitForDestination();
        }
        else
        {
            Debug.LogError("NavMeshAgent not found");
            this.Complete();
        }
    }

    async void StartWaitForDestination()
    {
        while(navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            await UniTask.NextFrame();

            if(Application.isPlaying == false)
            {
                return;
            }

            if(navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent not found");
                break;
            }
        }
        Debug.Log("NavMeshAgent reached destination");
        this.Complete();
    }
}

public class WaitForSuperNavAgentReachDestinationData : BaseSequenceData
{
}
