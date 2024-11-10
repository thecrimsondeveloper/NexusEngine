using System.IO;
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.Editor
{
    public static class SequenceGenerator
    {
        /// <summary>
        /// Creates a new script based on the provided template, modifies its class names, and saves it to the specified path.
        /// </summary>
        /// <param name="templateScript">The template MonoScript to copy from.</param>
        /// <param name="defaultName">The default file name to suggest.</param>
        /// <param name="menuTitle">The title for the save file panel dialog.</param>
        public static void CreateScriptFromTemplate(MonoScript templateScript, string defaultName, string menuTitle)
        {
            if (templateScript == null)
            {
                Debug.LogError("Template script is null. Cannot create a new script.");
                return;
            }

            string templatePath = AssetDatabase.GetAssetPath(templateScript);
            if (string.IsNullOrEmpty(templatePath))
            {
                Debug.LogError("Failed to get the asset path for the template.");
                return;
            }

            // Get the initial path based on the selected object
            string initialPath = GetSelectedFolderPath();
            if (string.IsNullOrEmpty(initialPath))
            {
                initialPath = "Assets"; // Default to the root of the Assets folder
            }

            // Prompt the user to choose a save location
            string savePath = EditorUtility.SaveFilePanelInProject(
                menuTitle,
                defaultName,
                "cs",
                "Please enter a file name to save the script.",
                initialPath
            );

            if (string.IsNullOrEmpty(savePath))
            {
                Debug.Log("Script creation cancelled by the user.");
                return;
            }

            string scriptName = Path.GetFileNameWithoutExtension(savePath);
            if (string.IsNullOrEmpty(scriptName))
            {
                Debug.LogError("Invalid script name.");
                return;
            }

            // Copy the template script to the selected path
            FileUtil.CopyFileOrDirectory(templatePath, savePath);
            Debug.Log("Script copied successfully!");

            // Modify the class names using the new script name
            ModifyClassNames(templateScript, savePath, scriptName);

            // Refresh the AssetDatabase to ensure Unity recognizes the updated script
            AssetDatabase.ImportAsset(savePath);
            AssetDatabase.Refresh();

            Debug.Log($"Script '{scriptName}' created and modified successfully!");
        }

        /// <summary>
        /// Gets the folder path of the currently selected object in the Project window.
        /// </summary>
        /// <returns>The selected folder path or null if none is selected.</returns>
        private static string GetSelectedFolderPath()
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
                
                if (!string.IsNullOrEmpty(path))
                {
                    if (Directory.Exists(path))
                    {
                        return path; // Selected object is a folder
                    }
                    else
                    {
                        return Path.GetDirectoryName(path); // Selected object is a file
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Modifies the class names in the script file based on the provided template and new script name.
        /// </summary>
        public static void ModifyClassNames(MonoScript templateScript, string scriptPath, string newScriptName)
        {
            if (templateScript == null)
            {
                Debug.LogError("Template script is null. Cannot modify class names.");
                return;
            }

            if (!File.Exists(scriptPath))
            {
                Debug.LogError("Script file not found for updating class names.");
                return;
            }

            string scriptContent = File.ReadAllText(scriptPath);
            string oldClassName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(templateScript));
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
