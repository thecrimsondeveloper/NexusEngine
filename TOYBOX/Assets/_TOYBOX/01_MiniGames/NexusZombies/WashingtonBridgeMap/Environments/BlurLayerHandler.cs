using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace ToyBox
{
    [ExecuteInEditMode]
    public class BlurLayerHandler : MonoBehaviour
    {
        [SerializeField] private List<BlurLayer> blurLayers = new List<BlurLayer>();
        [SerializeField] private BlurLayer blurLayerPrefab;

        private BlurLayer currentLayer;

        private bool isEditing = false;
        private Vector3 mouseStartPosition;
        private float initialDiameter;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (isEditing)
            {
                foreach (var layer in blurLayers)
                {
                    if (layer.IsInEditMode)
                    {
                        UnityEditor.Handles.DrawWireDisc(layer.transform.position, Vector3.up, layer.Diameter);
                        currentLayer = layer;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (isEditing)
            {
                UnityEditor.Handles.Label(currentLayer.transform.position, "    Blur Layer");
                float handleSize = UnityEditor.HandleUtility.GetHandleSize(currentLayer.transform.position) * 0.5f;
                Handles.color = Color.blue;
                Handles.DrawWireDisc(currentLayer.transform.position, Vector3.up, currentLayer.Diameter);
                Handles.color = Color.red;
                currentLayer.Diameter = Handles.ScaleValueHandle(currentLayer.Diameter, currentLayer.transform.position, Quaternion.identity, handleSize, Handles.CircleHandleCap, 1);
            }
        }

        private void Update()
        {
            if (isEditing && Event.current != null)
            {
                switch (Event.current.type)
                {
                    case EventType.MouseDown:
                        if (Event.current.button == 0)
                        {
                            mouseStartPosition = Event.current.mousePosition;
                            initialDiameter = currentLayer.Diameter;
                        }
                        break;
                    case EventType.MouseDrag:
                        if (Event.current.button == 0)
                        {
                            float delta = (mouseStartPosition.x - Event.current.mousePosition.x) * 0.01f; // Adjust sensitivity as needed
                            currentLayer.Diameter = initialDiameter + delta;
                        }
                        break;
                }
            }
        }
#endif

        [Button("Create New Blur Layer")]
        private void CreateNewBlurLayer()
        {
            if (blurLayerPrefab == null)
            {
                Debug.LogError("Blur layer prefab is not assigned!");
                return;
            }

            BlurLayer newBlurLayer = Instantiate(blurLayerPrefab);
            newBlurLayer.transform.SetParent(transform);
            blurLayers.Add(newBlurLayer);
            EnterEditMode(newBlurLayer);
        }

        [Button("Edit Blur Layer")]
        private void EnterEditMode(BlurLayer blurLayer)
        {
            blurLayer.IsInEditMode = true;
            foreach (var layer in blurLayers)
            {
                if (layer != blurLayer)
                {
                    layer.IsInEditMode = false;
                }
            }
            isEditing = true;
        }

        private void ExitEditMode(BlurLayer blurLayer)
        {
            blurLayer.IsInEditMode = false;
            blurLayer.IsInitialized = true;
            isEditing = false;
        }
    }
}
