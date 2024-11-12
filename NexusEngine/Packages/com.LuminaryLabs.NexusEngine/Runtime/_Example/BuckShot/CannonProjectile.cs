using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class CannonProjectile : EntitySequence<CannonProjectileData>
{
    /// <summary>
    /// Initializes the entity sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the entity sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(CannonProjectileData currentData)
    {
        Debug.Log("Entity Sequence Initialized");
        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        Debug.Log("Entity Sequence Started");
    }


}

public class CannonProjectileData : EntitySequenceData
{

}
