
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.Editor
{

    public class NexusReferenceEditor : NexusFactoryPane
    {
        private string searchId;

        [MenuItem("Tools/Find Managed Reference")]
        public static void FindManagedReference( string searchId )
        {
            string[] guids = AssetDatabase.FindAssets("t:Object");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string content = System.IO.File.ReadAllText(path);

                if (content.Contains(searchId))
                {
                    Debug.Log($"Found reference in: {path}");

                    //set the active selection to the asset
                    Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                    Selection.activeObject = obj;
                }
            }
        }

        protected override void WhenDraw()
        {
            GUILayout.Label("Find Managed Reference");
            GUILayout.Label("Enter the search ID below to find the reference in the project");
            searchId = EditorGUILayout.TextField("Search ID", searchId);

            if (GUILayout.Button("Find"))
            {
                FindManagedReference(searchId);
            }
        }
    }

}
