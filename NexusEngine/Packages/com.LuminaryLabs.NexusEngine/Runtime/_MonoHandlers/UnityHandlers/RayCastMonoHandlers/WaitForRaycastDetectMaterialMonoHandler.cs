using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;

public class WaitForRaycastDetectMaterialHandler : EntitySequence<WaitForRaycastDetectMaterialHandler.WaitForRaycastDetectMaterialData>
{
    private Rigidbody rb;
    private PhysicMaterial targetMaterial;
    private float rayDistanceMultiplier;

    protected override UniTask Initialize(WaitForRaycastDetectMaterialData currentData)
    {
        // Store the target material and ray distance from the provided data
        rb = currentData.rigidbody;
        targetMaterial = currentData.physicsMaterial;
        rayDistanceMultiplier = currentData.rayDistanceMultiplier;
        return UniTask.CompletedTask;
    }

    protected override void OnBegin()
    {

    }

    void Update()
    {
        if(this.phase != Phase.Run) return;
        // Perform a raycast in the forward direction


        float rayDistance = rb.velocity.magnitude * rayDistanceMultiplier;
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
        {
            
            Debug.Log("Raycast Hit: " + hit.collider.name);

            // Check if the material of the hit collider matches the target material
            if (hit.collider.sharedMaterial == targetMaterial)
            {
                Debug.Log("Material Match Found");
                (this as ISequence).Complete();
                return;
            }
        }
    }

    [System.Serializable]
    public class WaitForRaycastDetectMaterialData : SequenceData
    {
        public Rigidbody rigidbody;
        public PhysicMaterial physicsMaterial;
        public float rayDistanceMultiplier = 10f;
    }
}
