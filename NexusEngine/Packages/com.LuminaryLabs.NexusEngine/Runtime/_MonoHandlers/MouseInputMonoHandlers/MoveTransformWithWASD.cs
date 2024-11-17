using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class MoveTransformWithWASD : EntitySequence<MoveTransformWithWASDData>
{
    private Transform targetTransform;
    private Transform referenceTransform;
    private float movementSpeed;

    /// <summary>
    /// Initializes the entity sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the entity sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(MoveTransformWithWASDData currentData)
    {
        Debug.Log("Entity Sequence Initialized");

        // Assign data from currentData
        targetTransform = currentData.targetTransform;
        referenceTransform = currentData.referenceTransform;
        movementSpeed = currentData.movementSpeed;

        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
        Debug.Log("Entity Sequence Started");
    }

    /// <summary>
    /// Update is called once per frame to handle movement.
    /// </summary>
    private void Update()
    {
        if (targetTransform == null || referenceTransform == null) return;

        // Get input axes for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate movement direction relative to the reference transform
        Vector3 moveDirection = (referenceTransform.right * moveX + referenceTransform.forward * moveZ).normalized;

        // Move the target transform in the calculated direction
        targetTransform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);
    }
}

/// <summary>
/// Data class for MoveTransformWithWASD.
/// </summary>
[System.Serializable]
public class MoveTransformWithWASDData : EntitySequenceData
{
    [Tooltip("The transform to move.")]
    public Transform targetTransform;

    [Tooltip("Reference transform for relative movement (e.g., a camera).")]
    public Transform referenceTransform;

    [Tooltip("Movement speed multiplier.")]
    public float movementSpeed = 5f;
}
