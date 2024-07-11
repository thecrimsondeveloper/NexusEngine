using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;

namespace MetaAdvancedFeatures.SceneUnderstanding
{
    public class SnapToDesk : MonoBehaviour
    {
        [SerializeField] OVRSceneManager sceneManager;
        [SerializeField] List<OVRSceneAnchor> sceneAnchors;
        [SerializeField] Transform table;
        [SerializeField] Bounds tableBounds;
        [SerializeField] Vector3[] bottomEdges;
        [SerializeField] Vector3[] topEdges;
        [SerializeField] Vector3[] verticalEdges;
        [SerializeField] Quaternion boundsRotation;

        private void Awake()
        {
            if (sceneManager == null)
            {
                sceneManager = GetComponent<OVRSceneManager>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        }

        private void OnSceneModelLoadedSuccessfully()
        {
            
        }

        [Button]
        void Snap(Transform playSpace)
        {
            OVRSceneRoom sceneRoom = FindAnyObjectByType<OVRSceneRoom>();
            sceneAnchors = sceneRoom.GetComponentsInChildren<OVRSceneAnchor>().ToList();

            foreach (OVRSceneAnchor anchor in sceneAnchors)
            {
                OVRSemanticClassification classification = anchor.GetComponent<OVRSemanticClassification>();
                
                if (classification != null && classification.Labels.Contains("TABLE"))
                {
                    table = anchor.transform;
                    Vector2 tableDimensions = anchor.GetComponent<OVRScenePlane>().Dimensions;

                    // Update the tableBounds rotation to match the table
                    tableBounds.center = anchor.transform.position;
                    tableBounds.size = new Vector3(tableDimensions.x, 0.1f, tableDimensions.y);
                    boundsRotation = table.rotation;

                    SnapToTableEdge(playSpace);
                }
            }
        }

        public void SnapToTableEdge(Transform playspace)
        {
            Vector3 headPosition = GameObject.Find("CenterEyeAnchor").transform.position;
            Vector3[] tableEdges = GetBoundsEdges(tableBounds, boundsRotation);
            Vector3 closestEdge = tableEdges.OrderBy(edge => Vector3.Distance(edge, headPosition)).First();
            playspace.transform.position = closestEdge;
        }

        [Button]
        public void SnapToTableCenter(Transform playspace)
        {
            playspace.transform.position = table.position;
            playspace.transform.rotation = table.rotation;
        }

        Vector3[] GetBoundsEdges(Bounds bounds, Quaternion rotation)
        {
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            // Calculate the minimum and maximum corners of the rotated bounding box
            Vector3 minCorner = center - extents;
            Vector3 maxCorner = center + extents;

            // Apply rotation to the corners
            minCorner = rotation * minCorner;
            maxCorner = rotation * maxCorner;

            // Calculate the edges of the bounding box
            bottomEdges = new Vector3[4];
            topEdges = new Vector3[4];
            verticalEdges = new Vector3[4];
            
            bottomEdges[0] = new Vector3(minCorner.x, minCorner.y, minCorner.z);
            bottomEdges[1] = new Vector3(maxCorner.x, minCorner.y, minCorner.z);
            bottomEdges[2] = new Vector3(minCorner.x, minCorner.y, maxCorner.z);
            bottomEdges[3] = new Vector3(maxCorner.x, minCorner.y, maxCorner.z);

            topEdges[0] = new Vector3(minCorner.x, maxCorner.y, minCorner.z);
            topEdges[1] = new Vector3(maxCorner.x, maxCorner.y, minCorner.z);
            topEdges[2] = new Vector3(minCorner.x, maxCorner.y, maxCorner.z);
            topEdges[3] = new Vector3(maxCorner.x, maxCorner.y, maxCorner.z);

            verticalEdges[0] = new Vector3(minCorner.x, minCorner.y, minCorner.z);
            verticalEdges[1] = new Vector3(minCorner.x, maxCorner.y, minCorner.z);
            verticalEdges[2] = new Vector3(maxCorner.x, minCorner.y, minCorner.z);
            verticalEdges[3] = new Vector3(maxCorner.x, maxCorner.y, minCorner.z);

            return topEdges;
        }

        private void OnDrawGizmos() 
        {
            if (table != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(tableBounds.center, tableBounds.size);
            }
        }
    }
}
