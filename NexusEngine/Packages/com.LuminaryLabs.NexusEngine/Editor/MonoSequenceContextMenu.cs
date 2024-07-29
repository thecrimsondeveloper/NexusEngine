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

        // Open a save file dialog to specify where to save the prefab variant
        string fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(sourcePrefab)) + " Variant.prefab";
        string variantFilePath = EditorUtility.SaveFilePanelInProject("Save Prefab Variant", fileName, "prefab", "Please enter a file name to save the prefab variant to");

        if (string.IsNullOrEmpty(variantFilePath))
        {
            Debug.LogWarning("Invalid file path specified. Prefab variant creation cancelled.");
            return;
        }

        // Create the prefab variant
        GameObject variant = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;
        PrefabUtility.SaveAsPrefabAsset(variant, variantFilePath);

        // Destroy the instantiated variant in the scene
        DestroyImmediate(variant);

        Debug.Log("Prefab variant created at: " + variantFilePath);
    }

    [MenuItem("Assets/Create MonoSequence Variant", true)]
    private static bool ValidateCreatePrefabVariant()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }
}
