using System.Collections;
using Cysharp.Threading.Tasks; // Assuming you are using UniTask for async operations
using LuminaryLabs.NexusEngine; // Replace with your actual namespace if different
using UnityEngine;

public class RandomTeleportHandler : EntitySequence<RandomTeleportHandlerData>
{
    private Transform targetTransform;
    private float teleportRadius;

    protected override UniTask Initialize(RandomTeleportHandlerData currentData)
    {
        // Store the data provided, but do not perform the teleport here
        targetTransform = currentData.targetTransform;
        teleportRadius = currentData.teleportRadius;

        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        // Perform the teleport when the sequence begins
        Teleport();

        // Mark the sequence as complete immediately after teleporting
        Complete();
    }

    private void Teleport()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("Target transform is not set.");
            return;
        }

        // Generate a random position within a sphere of the given radius
        Vector3 randomPosition = Random.insideUnitSphere * teleportRadius;

        // Change the target transform's position to the new random position
        targetTransform.position = randomPosition;

        Debug.Log($"Teleported {targetTransform.name} to new position: {randomPosition}");
    }

    private async void Complete()
    {
        await UniTask.Delay(100);
        if(gameObject == null) return; 
        await Sequence.Finish(this);
        await Sequence.Stop(this);
    }

    protected override UniTask Unload()
    {
        return UniTask.CompletedTask;
    }
}

// Data class to store the target transform and radius
[System.Serializable]
public class RandomTeleportHandlerData : SequenceData
{
    public Transform targetTransform;
    public float teleportRadius = 1f;
}
