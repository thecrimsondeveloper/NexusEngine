using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class OrientTowardTransformHandler : EntitySequence<OrientTowardTransformHandlerData>
{
    private Transform transformToOrient;
    private Transform transformToLookAt;
    private float rotationSpeed;

    private bool lockX;
    private bool lockY;
    private bool lockZ;

    /// <summary>
    /// Initializes the entity sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the entity sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(OrientTowardTransformHandlerData currentData)
    {
        transformToOrient = currentData.transformToOrient;
        transformToLookAt = currentData.targetTransform;
        rotationSpeed = currentData.rotationSpeed;

        lockX = currentData.lockX;
        lockY = currentData.lockY;
        lockZ = currentData.lockZ;

        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override void OnBegin()
    {
    }

    void Update()
    {
        if (phase != Phase.Run) return;
        if (transformToOrient == null || transformToLookAt == null) return;

        // Calculate the direction to the target
        Vector3 targetDirection = transformToLookAt.position - transformToOrient.position;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Lock axes if specified
        Vector3 eulerRotation = targetRotation.eulerAngles;

        if (lockX) eulerRotation.x = transformToOrient.eulerAngles.x;
        if (lockY) eulerRotation.y = transformToOrient.eulerAngles.y;
        if (lockZ) eulerRotation.z = transformToOrient.eulerAngles.z;

        // Smoothly interpolate towards the target rotation
        transformToOrient.rotation = Quaternion.Lerp(
            transformToOrient.rotation,
            Quaternion.Euler(eulerRotation),
            rotationSpeed * Time.deltaTime
        );
    }
}

[System.Serializable]
public class OrientTowardTransformHandlerData : EntitySequenceData
{
    [Tooltip("The transform to orient.")]
    public Transform transformToOrient;

    [Tooltip("The target transform to look at.")]
    public Transform targetTransform;

    [Tooltip("The speed at which the object rotates toward the target.")]
    public float rotationSpeed = 5f;

    [Tooltip("Lock rotation on the X axis.")]
    public bool lockX;

    [Tooltip("Lock rotation on the Y axis.")]
    public bool lockY;

    [Tooltip("Lock rotation on the Z axis.")]
    public bool lockZ;
}
