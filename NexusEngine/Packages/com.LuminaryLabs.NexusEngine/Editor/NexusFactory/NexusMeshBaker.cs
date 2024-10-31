using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class NexusMeshBaker : NexusFactoryPane
{
    private GameObject parentObject;
    private Mesh meshToGenerateUVs;

    public NexusMeshBaker()
    {
        title = "Mesh Baker";
    }

    protected override void WhenDraw()
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

        // Field to assign a mesh to generate lightmap UVs
        meshToGenerateUVs = (Mesh)EditorGUILayout.ObjectField("Mesh to Generate Lightmap UVs", meshToGenerateUVs, typeof(Mesh), true);
        if (GUILayout.Button("Generate Lightmap UVs"))
        {
            if (meshToGenerateUVs != null)
            {
                GenerateLightmapUVs(meshToGenerateUVs);
            }
            else
            {
                Debug.LogWarning("No mesh assigned to generate lightmap UVs.");
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

    private void GenerateLightmapUVs(Mesh mesh)
    {
        // Check if the mesh already has UV2 (lightmap UVs)
        if (mesh.uv2 != null && mesh.uv2.Length > 0)
        {
            if (EditorUtility.DisplayDialog("Lightmap UVs", "The mesh already has UV2 (lightmap UVs). Do you want to regenerate them?", "Yes", "No"))
            {
                Unwrapping.GenerateSecondaryUVSet(mesh);
                Debug.Log("Lightmap UVs regenerated.");
            }
        }
        else
        {
            Unwrapping.GenerateSecondaryUVSet(mesh);
            Debug.Log("Lightmap UVs generated.");
        }

        // Mark the asset dirty so the changes are saved
        EditorUtility.SetDirty(mesh);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
                    GenerateLightmapUVs(meshFilter.sharedMesh);
                }

                // Generate lightmap UVs for each mesh
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
