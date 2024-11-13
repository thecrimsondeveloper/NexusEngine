using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEngine.AI;

public class BaseSuperNavAgentHandler : BaseSequence<BaseSuperNavAgentHandlerData>
{
       private NavMeshAgent navMeshAgent;
        private Transform destination;

        private bool warpToCurrentTransformPosition = false;
        private bool setDestinationOfAgent = true;
        private bool waitForAgentToReachDestination = true;


    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(BaseSuperNavAgentHandlerData currentData)
    {
        if((this as ISequence).TryGetUnityComponent(out navMeshAgent))
        {

        }
        destination = currentData.destination;
        
        warpToCurrentTransformPosition = currentData.warpToCurrentTransformPosition;
        setDestinationOfAgent = currentData.setDestinationOfAgent;
        waitForAgentToReachDestination = currentData.waitForAgentToReachDestination;

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
            await UniTask.NextFrame();

            if(Application.isPlaying == false)
            {
                return;
            }


            if(navMeshAgent == null)
            {
                break;
            }
        }
        this.Complete();
    }
}

public class BaseSuperNavAgentHandlerData : BaseSequenceData
{
    public bool warpToCurrentTransformPosition = false;
    public bool setDestinationOfAgent = true;
    public bool waitForAgentToReachDestination = true;
    public Transform destination;

}
