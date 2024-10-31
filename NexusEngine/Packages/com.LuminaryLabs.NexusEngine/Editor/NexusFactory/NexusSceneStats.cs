using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NexusSceneStats : NexusFactoryPane
{
    public int trianglesInScene = 0;
    public NexusSceneStats()
    {
        title = "Stats";
    }

    protected override void WhenDraw()
    {
        // Reset triangle count
        trianglesInScene = 0;

        // Get all MeshFilters in the scene
        MeshFilter[] meshFilters = GameObject.FindObjectsOfType<MeshFilter>();

        // Loop through all MeshFilters and accumulate triangle counts
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.mesh != null)
            {
                trianglesInScene += meshFilter.mesh.triangles.Length / 3;
            }
        }

        // Draw the triangle count in the editor window
        EditorGUILayout.LabelField("Total Triangles in Scene: " + trianglesInScene);
    }
}

