using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;

public class WaitForCollideWithMaterialHandler : EntitySequence<WaitForCollideWithMaterialHandler.WaitForCollideWithMaterialData>
{
    private PhysicMaterial targetMaterial;
    private CollisionType collisionType;

    /// <summary>
    /// Initializes the sequence with the provided data.
    /// </summary>
    protected override UniTask Initialize(WaitForCollideWithMaterialData currentData)
    {
        targetMaterial = currentData.physicsMaterial;
        collisionType = currentData.collisionType;
        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        Debug.Log("WaitForCollideWithMaterialHandler Started");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionType != CollisionType.Collision || this.phase != Phase.Run) return;

        PhysicMaterial materialOfCollidedCollider = collision.collider.sharedMaterial;
        if (materialOfCollidedCollider == targetMaterial)
        {
            Debug.Log("Collision with target material detected");
            (this as ISequence).Complete();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collisionType != CollisionType.Trigger || this.phase != Phase.Run) return;

        PhysicMaterial materialOfCollidedCollider = other.sharedMaterial;
        if (materialOfCollidedCollider == targetMaterial)
        {
            Debug.Log("Trigger with target material detected");
            (this as ISequence).Complete();
        }
    }

    [System.Serializable]
    public class WaitForCollideWithMaterialData : SequenceData
    {
        public PhysicMaterial physicsMaterial;
        public CollisionType collisionType = CollisionType.Collision;
    }

    /// <summary>
    /// Enum for selecting collision detection type.
    /// </summary>
    public enum CollisionType
    {
        Collision,
        Trigger
    }
}
