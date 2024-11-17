using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class FollowTransformMonoHandler : EntitySequence<FollowTransformMonoHandlerData>
{
    private Transform targetToMove;
    private Transform targetToFollow;
    private bool followPosition;
    private bool followRotation;

    private bool lockXPosition;
    private bool lockYPosition;
    private bool lockZPosition;

    private bool followXPosition;
    private bool followYPosition;
    private bool followZPosition;

    private bool followXRotation;
    private bool followYRotation;
    private bool followZRotation;

    private Vector3 offset;

    /// <summary>
    /// Initializes the entity sequence with the provided data.
    /// </summary>
    /// <param name="currentData">The current data for the entity sequence.</param>
    /// <returns>A UniTask representing the initialization process.</returns>
    protected override UniTask Initialize(FollowTransformMonoHandlerData currentData)
    {
        targetToMove = currentData.targetToMove;
        targetToFollow = currentData.targetToFollow;
        followPosition = currentData.followPosition;
        followRotation = currentData.followRotation;

        lockXPosition = currentData.lockXPosition;
        lockYPosition = currentData.lockYPosition;
        lockZPosition = currentData.lockZPosition;

        followXPosition = currentData.followXPosition;
        followYPosition = currentData.followYPosition;
        followZPosition = currentData.followZPosition;

        followXRotation = currentData.followXRotation;
        followYRotation = currentData.followYRotation;
        followZRotation = currentData.followZRotation;

        offset = currentData.offset;

        return UniTask.CompletedTask;
    }

    /// <summary>
    /// Called when the sequence begins.
    /// </summary>
    protected override async void OnBegin()
    {
        if (targetToFollow == null)
        {
            await Sequence.Stop(this);
        }
    }

    void Update()
    {
        if (phase != Phase.Run) return;
        if (targetToFollow == null) return;

        // Follow position if enabled
        if (followPosition)
        {
            Vector3 newPosition = targetToFollow.position +
                                  targetToFollow.right * offset.x +
                                  targetToFollow.up * offset.y +
                                  targetToFollow.forward * offset.z;

            // Apply axis locks
            if (!followXPosition || lockXPosition) newPosition.x = targetToMove.position.x;
            if (!followYPosition || lockYPosition) newPosition.y = targetToMove.position.y;
            if (!followZPosition || lockZPosition) newPosition.z = targetToMove.position.z;

            targetToMove.position = newPosition;
        }

        // Follow rotation if enabled
        if (followRotation)
        {
            Vector3 newRotation = targetToMove.eulerAngles;

            // Calculate the target rotation
            Vector3 targetRotation = targetToFollow.eulerAngles;

            // Apply axis follow settings
            if (followXRotation) newRotation.x = targetRotation.x;
            if (followYRotation) newRotation.y = targetRotation.y;
            if (followZRotation) newRotation.z = targetRotation.z;

            targetToMove.eulerAngles = newRotation;
        }
    }
}

[System.Serializable]
public class FollowTransformMonoHandlerData : EntitySequenceData
{
    [Tooltip("The transform to move.")]
    public Transform targetToMove;

    [Tooltip("The target transform to follow.")]
    public Transform targetToFollow;

    [Tooltip("Follow the position of the target.")]
    public bool followPosition = true;

    [Tooltip("Follow the rotation of the target.")]
    public bool followRotation = true;

    [Tooltip("Lock position on the X axis.")]
    public bool lockXPosition;

    [Tooltip("Lock position on the Y axis.")]
    public bool lockYPosition;

    [Tooltip("Lock position on the Z axis.")]
    public bool lockZPosition;

    [Tooltip("Follow position on the X axis.")]
    public bool followXPosition = true;

    [Tooltip("Follow position on the Y axis.")]
    public bool followYPosition = true;

    [Tooltip("Follow position on the Z axis.")]
    public bool followZPosition = true;

    [Tooltip("Follow rotation on the X axis.")]
    public bool followXRotation = true;

    [Tooltip("Follow rotation on the Y axis.")]
    public bool followYRotation = true;

    [Tooltip("Follow rotation on the Z axis.")]
    public bool followZRotation = true;

    [Tooltip("Offset from the target's position.")]
    public Vector3 offset = Vector3.zero;
}
