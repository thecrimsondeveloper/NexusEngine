using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEngine.AI;

public class BaseNavAgentHandler : BaseSequence<BaseNavAgentHandlerData>
{
    private NavMeshAgent navMeshAgent;
    private Transform destination;

    private bool warpToCurrentTransformPosition = false;
            private bool waitForAgentToReachDestination = true;
            private bool setDestinationOfAgent = true;


    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseNavAgentHandlerData currentData)
    {
        navMeshAgent = currentData.navMeshAgent;
        destination = currentData.destination;
        warpToCurrentTransformPosition = currentData.warpToCurrentTransformPosition;
        setDestinationOfAgent = currentData.setDestinationOfAgent;
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        if (navMeshAgent == null || destination == null)
        {
            Debug.LogWarning("NavMeshAgent or destination is null. Cannot move agent.");
            this.Complete();
            return;
        }

        if(warpToCurrentTransformPosition)
        {
            navMeshAgent.Warp(navMeshAgent.transform.position);
        }


        if(setDestinationOfAgent)
        {
            MoveAgentAndComplete();
        }
        else
        {
            this.Complete();
        }
    }

    /// <summary>
    /// Waits until the NavMeshAgent has stopped moving.
    /// </summary>
    private async void MoveAgentAndComplete()
    {
        // Set the destination
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(destination.position);


        if(waitForAgentToReachDestination == false)
        {
            this.Complete();
            return;
        }

        

        // Wait until the agent reaches the destination
        while (navMeshAgent.pathPending || 
            navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || 
            navMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            if(Application.isPlaying == false)
            {
                return;
            }

            if(navMeshAgent == null)
            {
                this.Complete();
                return;
            }
            await UniTask.NextFrame();
        }
        this.Complete();
    }
}

public class BaseNavAgentHandlerData : BaseSequenceData
{
    public bool warpToCurrentTransformPosition = false;
    public bool setDestinationOfAgent = true;
    public bool waitForAgentToReachDestination = true;

    public NavMeshAgent navMeshAgent;
    public Transform destination;
}
