using System.IO;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.Editor
{
    public static class EntitySequenceGenerator
    {
        private static string originalScriptPath;

        [MenuItem("Assets/Create/#C Script (Entity Sequence)", false, 3)]
        private static void CreateEntitySequenceScript()
        {
            // Get the entity template using the updated function
            MonoScript entitySequenceScript = SequenceEditorSettings.GetEntitySequenceTemplate();
            if (entitySequenceScript == null)
            {
                Debug.LogError("Entity sequence template not found!");
                return;
            }

            originalScriptPath = AssetDatabase.GetAssetPath(entitySequenceScript);
            if (string.IsNullOrEmpty(originalScriptPath))
            {
                Debug.LogError("Failed to get the asset path for the entity sequence template.");
                return;
            }

            // Prompt the user to choose a save location
            string savePath = EditorUtility.SaveFilePanelInProject(
                "Save Entity Sequence Script",
                "NewEntitySequence",
                "cs",
                "Please enter a file name to save the script."
            );

            // If the user cancels, exit early
            if (string.IsNullOrEmpty(savePath))
            {
                Debug.Log("Script creation cancelled by the user.");
                return;
            }

            string scriptName = Path.GetFileNameWithoutExtension(savePath);

            // Check if the script name is valid
            if (string.IsNullOrEmpty(scriptName))
            {
                Debug.LogError("Invalid script name.");
                return;
            }

            // Copy the template script to the selected path
            FileUtil.CopyFileOrDirectory(originalScriptPath, savePath);

            // Immediately modify the class name in the copied file
            ModifyClassNames(savePath, scriptName);

            // Refresh the AssetDatabase to ensure Unity recognizes the updated script
            AssetDatabase.ImportAsset(savePath);
            AssetDatabase.Refresh();

            Debug.Log($"Entity Sequence Script '{scriptName}' created and modified successfully!");
        }

        /// <summary>
        /// Modifies the class names in the script file immediately after copying.
        /// </summary>
        private static void ModifyClassNames(string scriptPath, string newScriptName)
        {
            if (!File.Exists(scriptPath))
            {
                Debug.LogError("Script file not found for updating class name.");
                return;
            }

            string scriptContent = File.ReadAllText(scriptPath);

            // Get the original class names from the entity template
            MonoScript entitySequenceScript = SequenceEditorSettings.GetEntitySequenceTemplate();
            if (entitySequenceScript == null)
            {
                Debug.LogError("Failed to retrieve the entity sequence template for class name update.");
                return;
            }

            string oldClassName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(entitySequenceScript));
            if (string.IsNullOrEmpty(oldClassName))
            {
                Debug.LogError("Original class name is invalid.");
                return;
            }

            string oldDataClassName = $"{oldClassName}Data";
            string newDataClassName = $"{newScriptName}Data";

            // Replace both the main class name and the data class name
            scriptContent = scriptContent
                .Replace($"public class {oldClassName}", $"public class {newScriptName}")
                .Replace($"public class {oldDataClassName}", $"public class {newDataClassName}");

            // Write the modified content back to the file
            File.WriteAllText(scriptPath, scriptContent);
        }
    }
}
