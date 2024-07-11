using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.VFX;
using Unity.VisualScripting;

namespace UnityEngine.VFX
{
    public class VFXEditorWidnow : EditorWindow
    {

        [MenuItem("Window/VFX Editor Utility")]
        public static void OpenWindow()
        {
            var window = GetWindow<VFXEditorWidnow>();
            window.titleContent = new GUIContent("VFX Editor");
            window.Show();
        }

        VisualEffectAsset visualEffect;
        int capacity = 0;

        private void OnGUI()
        {
            visualEffect = (VisualEffectAsset)EditorGUILayout.ObjectField("Visual Effect", visualEffect, typeof(VisualEffectAsset), false);
            capacity = EditorGUILayout.IntField("Capacity", capacity);

            if (visualEffect != null)
            {
                if (GUILayout.Button("Update Capacity"))
                {
                    visualEffect.UpdateCapacity(capacity);
                }
            }
        }

    }


    public static class VFXUtility
    {
        public static void UpdateCapacity(this VisualEffectAsset visualEffect, int capacity)
        {

            List<VFXExposedProperty> exposedProperties = new List<VFXExposedProperty>();
            visualEffect.GetExposedProperties(exposedProperties);
            foreach (VFXExposedProperty property in exposedProperties)
            {
                Debug.Log(property.name);
            }

            // visualEffect.


            // AssetDatabase.SaveAssets();

            Debug.Log("VFX System Capacity changed to " + capacity + "!");
        }
    }
}
