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

        // Ask the user for the name of the new MonoSequence variant
        string prefabName = EditorUtility.SaveFilePanel("Save MonoSequence Variant As", "Assets", "NewMonoSequenceVariant", "prefab");
        if (string.IsNullOrEmpty(prefabName))
        {
            Debug.LogWarning("Prefab creation cancelled.");
            return;
        }

        // Ensure the prefab name is within the project's Assets directory
        if (!prefabName.StartsWith(Application.dataPath))
        {
            Debug.LogError("Prefab must be saved within the Assets folder.");
            return;
        }

        // Convert the system path to a relative path
        string relativePath = "Assets" + prefabName.Substring(Application.dataPath.Length);

        // Create the prefab variant
        GameObject variant = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;
        PrefabUtility.SaveAsPrefabAsset(variant, relativePath);

        // Destroy the instantiated variant in the scene
        DestroyImmediate(variant);

        Debug.Log("Prefab variant created at: " + relativePath);

        // Highlight the newly created prefab variant in the project window
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
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

        // Ask for a name for the RunnerSequence prefab
        string prefabName = EditorUtility.SaveFilePanel("Save Runner Sequence As", "Assets", "NewRunnerSequence", "prefab");
        if (string.IsNullOrEmpty(prefabName))
        {
            Debug.LogWarning("Runner Sequence creation cancelled.");
            return;
        }

        // Ensure the prefab name is within the project's Assets directory
        if (!prefabName.StartsWith(Application.dataPath))
        {
            Debug.LogError("Runner Sequence must be saved within the Assets folder.");
            return;
        }

        // Convert the system path to a relative path
        string relativePath = "Assets" + prefabName.Substring(Application.dataPath.Length);

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
    }

    [MenuItem("Assets/Create Runner Sequence", true)]
    private static bool ValidateCreateRunnerSequence()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }
}
