using System.Collections.Generic;
using UnityEngine;

public static class NexusMeshUtility
{
    public static GameObject CombineMeshesWithMaterials(GameObject parentObject)
    {
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0)
        {
            Debug.LogError("No MeshFilters found under the parent object.");
            return null;
        }

        // Dictionary to store submeshes for each unique material
        Dictionary<Material, List<CombineInstance>> materialToMeshCombine = new Dictionary<Material, List<CombineInstance>>();

        // Loop through each MeshFilter and group by material
        foreach (MeshFilter mf in meshFilters)
        {
            MeshRenderer meshRenderer = mf.GetComponent<MeshRenderer>();
            if (meshRenderer != null && mf.sharedMesh != null)
            {
                Material[] materials = meshRenderer.sharedMaterials;

                for (int submesh = 0; submesh < mf.sharedMesh.subMeshCount; submesh++)
                {
                    if (submesh >= materials.Length)
                    {
                        Debug.LogWarning("Mesh has more submeshes than materials. Skipping extra submeshes.");
                        continue;
                    }

                    Material mat = materials[submesh];
                    CombineInstance ci = new CombineInstance
                    {
                        mesh = mf.sharedMesh,
                        subMeshIndex = submesh,
                        transform = mf.transform.localToWorldMatrix
                    };

                    // Add the mesh to the list for the corresponding material
                    if (!materialToMeshCombine.ContainsKey(mat))
                    {
                        materialToMeshCombine[mat] = new List<CombineInstance>();
                    }
                    materialToMeshCombine[mat].Add(ci);
                }
            }
        }

        // Create a new GameObject to hold the combined mesh
        GameObject combinedObject = new GameObject("CombinedMeshWithMaterials");

        // Add MeshFilter and MeshRenderer to the new GameObject
        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer combinedMeshRenderer = combinedObject.AddComponent<MeshRenderer>();

        // Prepare to combine submeshes, one for each material
        Mesh finalCombinedMesh = new Mesh();
        finalCombinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Allow larger meshes

        int totalSubmeshCount = materialToMeshCombine.Count;
        CombineInstance[] combineInstances = new CombineInstance[totalSubmeshCount];
        Material[] materialsArray = new Material[totalSubmeshCount];
        int submeshIndex = 0;

        // Combine submeshes for each material
        foreach (var kvp in materialToMeshCombine)
        {
            Material mat = kvp.Key;
            CombineInstance[] combineArray = kvp.Value.ToArray();

            Mesh combinedSubMesh = new Mesh();
            combinedSubMesh.CombineMeshes(combineArray, true, true);

            // Store the combined mesh into the final submesh
            combineInstances[submeshIndex].mesh = combinedSubMesh;
            combineInstances[submeshIndex].transform = Matrix4x4.identity;

            // Assign the corresponding material to the submesh
            materialsArray[submeshIndex] = mat;
            submeshIndex++;
        }

        // Combine all submeshes into the final mesh
        finalCombinedMesh.CombineMeshes(combineInstances, false, false);

        // Assign the final combined mesh to the MeshFilter
        combinedMeshFilter.mesh = finalCombinedMesh;

        // Assign the materials to the MeshRenderer
        combinedMeshRenderer.sharedMaterials = materialsArray;

        // Return the combined mesh object
        return combinedObject;
    }



    private static void LogMeshDetails(MeshFilter meshFilter, MeshRenderer meshRenderer)
    {
        if (meshFilter == null || meshRenderer == null)
        {
            Debug.LogError("MeshFilter or MeshRenderer is null. Ensure both components are added correctly.");
            return;
        }

        // Get mesh details
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh assigned to the MeshFilter.");
            return;
        }

        // Get shadow settings
        bool castShadows = meshRenderer.shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off;
        bool receiveShadows = meshRenderer.receiveShadows;

        // Get material details
        Material[] materials = meshRenderer.sharedMaterials;
        string materialInfo = "";
        foreach (Material mat in materials)
        {
            if (mat != null)
            {
                materialInfo += $"Material: {mat.name}, Shader: {mat.shader.name}; ";
            }
        }

        // Log all relevant details in one log message
        Debug.Log($"Mesh Info: Vertices: {mesh.vertexCount}, Submeshes: {mesh.subMeshCount}, Cast Shadows: {castShadows}, " +
                  $"Receive Shadows: {receiveShadows}, Materials: {materialInfo}");
    }

}
