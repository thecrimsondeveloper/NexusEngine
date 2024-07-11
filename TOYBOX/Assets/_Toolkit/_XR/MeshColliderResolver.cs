using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class MeshColliderResolver : MonoBehaviour
    {
        [SerializeField] MeshCollider meshCollider;
        [SerializeField] MeshFilter meshFilter;

        private void Update()
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
                return;
            }
            //if the filter has no mesh
            if (meshFilter.sharedMesh == null)
            {
                return;
            }


            if (meshCollider == null)
            {
                meshCollider = GetComponent<MeshCollider>();
                return;
            }


            //if collider has mesh
            if (meshCollider.sharedMesh != meshFilter.sharedMesh)
            {
                SetMeshOnCollider();
            }
            else
            {
                Destroy(this);
            }

        }

        void SetMeshOnCollider()
        {
            if (meshFilter != null && meshCollider != null)
            {
                meshCollider.sharedMesh = meshFilter.sharedMesh;
            }
        }
    }
}
