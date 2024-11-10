using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;


public class WaitForCollideWithMaterialHandler : EntitySequence<WaitForCollideWithMaterialHandler.WaitForCollideWithMaterialData>
{
    PhysicMaterial targetMaterial;

    protected override UniTask Initialize(WaitForCollideWithMaterialData currentData)
    {
        targetMaterial = currentData.physicsMaterial;
        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");
        if(this.phase != Phase.Run) return;

        PhysicMaterial materialOfCollidedCollider = collision.collider.sharedMaterial;

        if(materialOfCollidedCollider == targetMaterial)
        {
            (this as ISequence).Complete();
        }
    }

    
    [System.Serializable]
    public class WaitForCollideWithMaterialData : SequenceData
    {
        public PhysicMaterial physicsMaterial;
    }
}
