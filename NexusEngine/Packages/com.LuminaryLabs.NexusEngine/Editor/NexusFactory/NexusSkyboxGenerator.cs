using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NexusSkyboxGenerator : NexusFactoryPane
{
    enum SkyboxResolution
    {
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048,
        _4096 = 4096
    }

    public NexusSkyboxGenerator()
    {
        title = "Skybox Generator";
    }

    SkyboxResolution resolution = SkyboxResolution._1024;

    protected override void WhenDraw()
    {

        //horizontal layout for the buttons
        GUILayout.BeginHorizontal();
        //add an enum selection for the skybox resolution
        GUILayout.Label("Skybox Resolution");
        resolution = (SkyboxResolution)EditorGUILayout.EnumPopup(resolution);

        GUILayout.EndHorizontal();


        if (GUILayout.Button("Generate Skybox From Scene View Camera"))
        {
            GenerateSkyboxFromSceneCamera((int)resolution);
        }
    }

    public static void GenerateSkyboxFromSceneCamera(int resolution)
    {
        // Get the currently active scene camera
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        if (sceneCamera == null)
        {
            Debug.LogError("No active Scene View camera found.");
            return;
        }

        // Ask the user where to save the skybox assets
        string path = EditorUtility.SaveFolderPanel("Save Skybox Assets", "Assets/", "");
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("Save folder selection canceled.");
            return;
        }

        // Ensure the path is inside the Assets folder
        if (!path.StartsWith(Application.dataPath))
        {
            Debug.LogError("Please choose a folder inside the Assets directory.");
            return;
        }

        // Convert path to a relative path (from "Assets/" onward)
        path = "Assets" + path.Substring(Application.dataPath.Length);


        Cubemap cubemap = new Cubemap(resolution, TextureFormat.RGBA32, false);

        // Render the scene from the camera to the cubemap
        sceneCamera.RenderToCubemap(cubemap);

        // Save the cubemap texture to the selected folder
        string cubemapPath = path + "/SkyboxCubemap.cubemap";
        AssetDatabase.CreateAsset(cubemap, cubemapPath);

        // Create a material for the skybox and assign the cubemap texture
        Material skyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
        skyboxMaterial.SetTexture("_Tex", cubemap);

        // Save the skybox material to the selected folder
        string materialPath = path + "/SkyboxMaterial.mat";
        AssetDatabase.CreateAsset(skyboxMaterial, materialPath);

        // Refresh the asset database to reflect the new assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Log the success message
        Debug.Log($"Skybox generated and saved at: {path}");
    }
}
