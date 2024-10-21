using UnityEngine;
using UnityEditor;
using System.IO;
using LuminaryLabs.NexusEngine;

public class MonoSequenceContextMenu : MonoBehaviour
{
    // Specify the path to the source prefab for MonoSequence_Base
    private static string sourcePrefabPath = "Packages/com.LuminaryLabs.NexusEngine/Runtime/_Sequences/MonoSequence_Base.prefab";
    
    // MonoSequence Variant Menu
    [MenuItem("Assets/Create MonoSequence Variant", false, 0)]
    private static void CreateMonoSequenceVariant()
    {
        // Load the MonoSequence_Base prefab
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        if (sourcePrefab == null)
        {
            Debug.LogWarning("The source prefab could not be found. Please check the path.");
            return;
        }
        
        // Get the current folder path
        string prefabFolderPath = NexusFactory.GetCurrentFolderPath();
        if (string.IsNullOrEmpty(prefabFolderPath))
        {
            Debug.LogWarning("Prefab creation cancelled.");
            return;
        }

        // Ensure the prefab name is within the project's Assets directory
        if (!prefabFolderPath.StartsWith("Assets"))
        {
            Debug.LogError("Prefab must be saved within the Assets folder.");
            return;
        }

        // Set the path for the new prefab variant
        string relativePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(prefabFolderPath, "NewMonoSequenceVariant.prefab"));

        // Create the prefab variant
        GameObject variant = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;
        PrefabUtility.SaveAsPrefabAsset(variant, relativePath);

        // Destroy the instantiated variant in the scene
        DestroyImmediate(variant);

        Debug.Log("Prefab variant created at: " + relativePath);

        // Highlight the newly created prefab variant in the project window
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);

        // Trigger rename prompt in the Project window
        EditorApplication.delayCall += () =>
        {
            if (Selection.activeObject != null)
            {
                // Trigger the rename operation using the built-in rename function
                EditorApplication.ExecuteMenuItem("Assets/Rename");
            }
        };
    }

    [MenuItem("Assets/Create MonoSequence", true)]
    private static bool ValidateCreateMonoSequenceVariant()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }

    // RunnerSequence Menu
    [MenuItem("Assets/Create RunnerSequence", false, 1)]
    private static void CreateRunnerSequence()
    {
        // Load the MonoSequence_Base prefab
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        if (sourcePrefab == null)
        {
            Debug.LogWarning("The source prefab could not be found. Please check the path.");
            return;
        }

        // Get the current folder path
        string prefabFolderPath = NexusFactory.GetCurrentFolderPath();
        if (string.IsNullOrEmpty(prefabFolderPath))
        {
            Debug.LogWarning("Runner Sequence creation cancelled.");
            return;
        }

        // Ensure the prefab name is within the project's Assets directory
        if (!prefabFolderPath.StartsWith("Assets"))
        {
            Debug.LogError("Runner Sequence must be saved within the Assets folder.");
            return;
        }

        // Set the path for the new RunnerSequence prefab
        string relativePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(prefabFolderPath, "NewRunnerSequence.prefab"));

        // Instantiate the MonoSequence_Base prefab
        GameObject runnerSequenceInstance = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;

        // Add the RunnerSequence component to the instantiated prefab
        runnerSequenceInstance.AddComponent<RunnerSequence>();

        // Save the new prefab with the RunnerSequence component
        PrefabUtility.SaveAsPrefabAsset(runnerSequenceInstance, relativePath);

        // Destroy the instantiated object in the scene after saving
        DestroyImmediate(runnerSequenceInstance);

        Debug.Log("Runner Sequence created and saved at: " + relativePath);

        // Highlight the newly created prefab in the project window
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);

        EditorApplication.delayCall += () =>
        {
            if (Selection.activeObject != null)
            {
                // Trigger the rename operation using the built-in rename function
                EditorApplication.ExecuteMenuItem("Assets/Rename");
            }
        };
    }

    [MenuItem("Assets/Create Runner Sequence", true)]
    private static bool ValidateCreateRunnerSequence()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }
}
