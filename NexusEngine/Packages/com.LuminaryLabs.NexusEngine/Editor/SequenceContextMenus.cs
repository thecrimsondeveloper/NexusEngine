using UnityEngine;
using UnityEditor;
using System.IO;
using LuminaryLabs.NexusEngine;

public class SequenceContextMenus : MonoBehaviour
{
    // Specify the path to the source prefab for MonoSequence_Base
    private static string sourcePrefabPath = "Packages/com.LuminaryLabs.NexusEngine/Runtime/_Sequences/MonoSequence_Base.prefab";
    
    // MonoSequence Variant Menu
   

    [MenuItem("Assets/Create MonoSequence", true)]
    private static bool ValidateCreateMonoSequenceVariant()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }

    // RunnerSequence Menu
 

    [MenuItem("Assets/Create Runner Sequence", true)]
    private static bool ValidateCreateRunnerSequence()
    {
        // Validate if the source prefab exists
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);
        return sourcePrefab != null;
    }





}
