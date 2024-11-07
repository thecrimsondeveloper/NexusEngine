using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class MeshFragmentationHandler : EntitySequence<MeshFragmentationHandlerData>
    {
        private Mesh _originalMesh;
        private GameObject _targetObject;
        private int _fragmentCount;
        private float _fragmentDestructionDelay;

        protected override UniTask Initialize(MeshFragmentationHandlerData currentData)
        {
            // Retrieve target object and fragment count from data
            _targetObject = currentData.targetObject;
            _fragmentCount = currentData.fragmentCount;
            _fragmentDestructionDelay = currentData.fragmentDestructionDelay;

            if (_targetObject != null)
            {
                _originalMesh = _targetObject.GetComponent<MeshFilter>().mesh;
            }

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            if (_originalMesh == null || _targetObject == null)
            {
                Sequence.Stop(this);
                return;
            }

            // Fragment the mesh
            FragmentMesh();

            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private List<GameObject> fragments = new List<GameObject>(); // Store fragments for later destruction

        private void FragmentMesh()
        {
            // For this example, we're assuming the object is a cube
            Vector3 size = _targetObject.transform.localScale;

            float fragmentSize = size.x / _fragmentCount; // Assuming cube shape, we fragment it equally along all axes

            // Generate fragments
            for (int x = 0; x < _fragmentCount; x++)
            {
                for (int y = 0; y < _fragmentCount; y++)
                {
                    for (int z = 0; z < _fragmentCount; z++)
                    {
                        Vector3 fragmentPosition = _targetObject.transform.position +
                                                   new Vector3(
                                                       (x + 0.5f) * fragmentSize - size.x / 2,
                                                       (y + 0.5f) * fragmentSize - size.y / 2,
                                                       (z + 0.5f) * fragmentSize - size.z / 2
                                                   );

                        // Create fragment cubes
                        GameObject fragment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        fragment.transform.position = fragmentPosition;
                        fragment.transform.localScale = new Vector3(fragmentSize, fragmentSize, fragmentSize);
                        fragment.GetComponent<MeshRenderer>().material = _targetObject.GetComponent<MeshRenderer>().material;

                        // Add Rigidbody for physics
                        Rigidbody rb = fragment.AddComponent<Rigidbody>();
                        rb.mass = 0.1f;  // Adjust mass for realistic fragmentation

                        fragments.Add(fragment);
                    }
                }
            }

            //set the target object to inactive
            _targetObject.SetActive(false);
        }

        protected override UniTask Unload()
        {
            foreach (var fragment in fragments)
            {
                Destroy(fragment.gameObject, _fragmentDestructionDelay);
            }
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class MeshFragmentationHandlerData : SequenceData
    {
        public GameObject targetObject;   // The object to be fragmented
        public int fragmentCount = 3;     // Number of fragments along each axis (3x3x3 cubes)
        public float fragmentDestructionDelay = 5.0f; // Delay before destroying fragments
    }
}
