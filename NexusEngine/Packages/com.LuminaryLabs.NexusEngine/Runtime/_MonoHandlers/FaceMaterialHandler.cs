using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;


namespace LuminaryLabs.NexusEngine.UnityHandlers
{

    public class FaceMaterialHandler : EntitySequence<FaceMaterialHandlerData>
    {
        public enum FaceMode
        {
            Nearest,
            Farthest,
        }

        private Transform transformToOrient;
        private float range = 100f;
        private FaceMode faceMode = FaceMode.Nearest;
        private float refreshRate = 0.5f;
        private PhysicMaterial materialToSearchFor;

        private Transform currentTarget = null;
        private float lastTimeRefreshed = 0;
        private Collider[] collidersBuffer = new Collider[100];

        void OnDrawGizmos()
        {
            if(this.phase != Phase.Run) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        /// <summary>
        /// Initializes the entity sequence with the provided data.
        /// </summary>
        protected override UniTask Initialize(FaceMaterialHandlerData currentData)
        {
            transformToOrient = currentData.transformToOrient;
            materialToSearchFor = currentData.materialToSearchFor;
            range = currentData.range;
            faceMode = currentData.faceMode;
            refreshRate = currentData.refreshRate;

            // Validate data
            if (transformToOrient == null)
            {
                Debug.LogError("FaceMaterialHandler: Transform to orient is not assigned.");
            }

            if (materialToSearchFor == null)
            {
                Debug.LogError("FaceMaterialHandler: Material to search for is not assigned.");
            }

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            Debug.Log("FaceMaterialHandler: Sequence started.");
        }

        void Update()
        {
            // Ensure the transform to orient is set
            if (transformToOrient == null) return;

            // Continuously look at the current target if available
            if (currentTarget != null)
            {
                OrientTowardsTarget();
            }

            // Refresh the target at the specified refresh rate
            if (Time.time - lastTimeRefreshed > refreshRate)
            {
                RefreshTarget();
                lastTimeRefreshed = Time.time;
            }
        }

        private void RefreshTarget()
        {
            currentTarget = (faceMode == FaceMode.Nearest) ? GetNearestCollider() : GetFarthestCollider();
        }

        private void OrientTowardsTarget()
        {
            if (currentTarget != null && transformToOrient != null)
            {
                transformToOrient.LookAt(currentTarget);
            }
        }

        private Transform GetNearestCollider()
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, collidersBuffer);

            Collider nearestCollider = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < numColliders; i++)
            {
                Collider collider = collidersBuffer[i];
                if (collider.sharedMaterial == materialToSearchFor)
                {
                    float distance = Vector3.Distance(collider.transform.position, transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestCollider = collider;
                    }
                }
            }

            return nearestCollider?.transform;
        }

        private Transform GetFarthestCollider()
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, collidersBuffer);

            Collider farthestCollider = null;
            float farthestDistance = float.MinValue;

            for (int i = 0; i < numColliders; i++)
            {
                Collider collider = collidersBuffer[i];
                if (collider.sharedMaterial == materialToSearchFor)
                {
                    float distance = Vector3.Distance(collider.transform.position, transform.position);
                    if (distance > farthestDistance)
                    {
                        farthestDistance = distance;
                        farthestCollider = collider;
                    }
                }
            }

            return farthestCollider?.transform;
        }
    }

    [Serializable]
    public class FaceMaterialHandlerData : EntitySequenceData
    {
        public Transform transformToOrient;
        public PhysicMaterial materialToSearchFor;
        public float range = 100f;
        public FaceMaterialHandler.FaceMode faceMode = FaceMaterialHandler.FaceMode.Nearest;
        public float refreshRate = 0.5f;
    }

}

