using UnityEngine;
using UnityEditor;
using System.IO;

public class MonoSequenceContextMenu : MonoBehaviour
{
    // Specify the path to the source prefab
    private static string sourcePrefabPath = "Packages/com.LuminaryLabs.NexusEngine/Runtime/_Sequences/MonoSequence_Base.prefab";

    [MenuItem("Assets/Create MonoSequence Variant", false, 0)]
    private static void CreatePrefabVariant()
    {
        // Load the prefab from the specified path
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        if (sourcePrefab == null)
        {
            Debug.LogWarning("The source prefab could not be found. Please check the path.");
            return;
        }

        // Get the path of the selected object in the project window
        Object selectedObject = Selection.activeObject;
        string selectedPath = AssetDatabase.GetAssetPath(selectedObject);

        // If the selected path is a file, get its directory
        if (!string.IsNullOrEmpty(selectedPath) && !AssetDatabase.IsValidFolder(selectedPath))
        {
            selectedPath = Path.GetDirectoryName(selectedPath);
        }

        // If no valid path is selected, default to "Assets" folder
        if (string.IsNullOrEmpty(selectedPath))
        {
            selectedPath = "Assets";
        }

        // Generate a unique name for the new prefab variant
        string baseName = Path.GetFileNameWithoutExtension(sourcePrefabPath) + " Variant";
        string uniqueName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(selectedPath, baseName + ".prefab"));

        // Create the prefab variant
        GameObject variant = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;
        PrefabUtility.SaveAsPrefabAsset(variant, uniqueName);

        // Destroy the instantiated variant in the scene
        DestroyImmediate(variant);

        Debug.Log("Prefab variant created at: " + uniqueName);

        // Highlight the newly created prefab variant in the project window
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(uniqueName);
    }

    [MenuItem("Assets/Create MonoSequence Variant", true)]
    private static bool ValidateCreatePrefabVariant()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }
}