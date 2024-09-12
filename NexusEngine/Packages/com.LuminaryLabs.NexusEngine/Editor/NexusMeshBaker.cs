using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class NexusMeshBaker : EditorWindow
{
    private GameObject parentObject;
    [MenuItem("Luminary Labs/Nexus Mesh Baker")]
    public static void ShowWindow()
    {
        GetWindow<NexusMeshBaker>("Nexus Mesh Baker");
    }

    void OnGUI()
    {
        GUILayout.Label("Nexus Mesh Baker", EditorStyles.boldLabel);

        // Field to assign the parent GameObject
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent GameObject", parentObject, typeof(GameObject), true);

        // Bake button
        if (GUILayout.Button("Bake Meshes"))
        {
            if (parentObject != null)
            {
                BakeMesh();
            }
            else
            {
                Debug.LogWarning("No parent object assigned for mesh baking.");
            }
        }
    }

    // This method calls the NexusMeshUtility to combine meshes
    void BakeMesh()
    {
        System.Text.StringBuilder logOutput = new System.Text.StringBuilder();
        logOutput.AppendLine($"Baking mesh for parent object: {parentObject.name}");

        // Gather information about each child mesh before combining
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in meshFilters)
        {
            MeshRenderer mr = mf.GetComponent<MeshRenderer>();
            if (mr != null && mf.sharedMesh != null)
            {
                logOutput.AppendLine($"Pre-Bake Mesh: {mf.name}, Vertices: {mf.sharedMesh.vertexCount}, " +
                                     $"Submeshes: {mf.sharedMesh.subMeshCount}, " +
                                     $"Cast Shadows: {mr.shadowCastingMode}, Receive Shadows: {mr.receiveShadows}, " +
                                     $"Material: {mr.sharedMaterial.name}, Shader: {mr.sharedMaterial.shader.name}");
            }
        }

        GameObject combinedMeshObject = NexusMeshUtility.CombineMeshesWithMaterials(parentObject);

        if (combinedMeshObject != null)
        {
            logOutput.AppendLine("Mesh baked successfully and new GameObject created.");
            Selection.activeObject = combinedMeshObject;  // Select the newly created object in the editor

            // Save the prefab and the mesh as a separate asset
            SaveObjectAsPrefabWithSeparateMesh(combinedMeshObject);

            // Output the single log statement
            Debug.Log(logOutput.ToString());
        }
        else
        {
            logOutput.AppendLine("Error: Failed to bake meshes.");
            Debug.LogError(logOutput.ToString());
        }
    }

    // Save the entire GameObject as a prefab and save the baked mesh as a separate asset
    async void SaveObjectAsPrefabWithSeparateMesh(GameObject obj)
    {
        // Choose a location to save the prefab
        string prefabPath = EditorUtility.SaveFilePanelInProject("Save Prefab", obj.name, "prefab", "Please enter a file name to save the prefab.");
        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogWarning("Prefab save operation cancelled.");
            return;
        }

        // Save the prefab first
        GameObject prefabObject = PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        await UniTask.NextFrame();  // Wait for the prefab to be saved

        if (prefabObject != null)
        {
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
            if (mesh != null)
            {
                // Determine the mesh asset path based on the prefab path, using the same folder and name
                string meshPath = prefabPath.Replace(".prefab", ".asset");

                // Save the mesh as a separate asset with the same name and location as the prefab
                AssetDatabase.CreateAsset(mesh, meshPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // Assign the saved mesh to the prefab's MeshFilter
                MeshFilter meshFilter = prefabObject.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFilter.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
                    EditorUtility.SetDirty(meshFilter); // Mark it dirty so the changes are saved
                }
            }
            else
            {
                Debug.LogError("No mesh found in the GameObject.");
            }
        }
        else
        {
            Debug.LogError("Failed to create the prefab.");
        }
    }
}
