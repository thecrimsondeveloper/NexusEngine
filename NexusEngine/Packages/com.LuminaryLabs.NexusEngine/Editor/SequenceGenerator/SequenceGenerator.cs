using System.IO;
using System.Text.RegularExpressions;
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
        /// Modifies the class names, type parameters, and function parameter types in the script file.
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

            // Define the old and new data class names
            string oldDataClassName = $"{oldClassName}Data";
            string newDataClassName = $"{newScriptName}Data";

            // Replace the main class name
            scriptContent = scriptContent.Replace($"public class {oldClassName}", $"public class {newScriptName}");

            // Replace the data class name
            scriptContent = scriptContent.Replace($"public class {oldDataClassName}", $"public class {newDataClassName}");

            // Extract the base class type and its type parameter
            string baseClassPattern = @"class\s+\w+\s*:\s*(\w+<(\w+)>)";
            Match baseClassMatch = Regex.Match(scriptContent, baseClassPattern);
            if (baseClassMatch.Success)
            {
                string oldBaseClassType = baseClassMatch.Groups[1].Value;
                string oldBaseDataType = baseClassMatch.Groups[2].Value;

                // Replace the type parameter in the base class
                string newBaseClassType = oldBaseClassType.Replace(oldBaseDataType, newDataClassName);
                scriptContent = scriptContent.Replace(oldBaseClassType, newBaseClassType);
            }

            // Use regex to update the Initialize method's parameter type
            string oldInitializePattern = $@"protected\s+override\s+UniTask\s+Initialize\(\s*{oldDataClassName}\s+currentData\s*\)";
            string newInitializeReplacement = $"protected override UniTask Initialize({newDataClassName} currentData)";
            scriptContent = Regex.Replace(scriptContent, oldInitializePattern, newInitializeReplacement);

            // Write the modified content back to the file
            File.WriteAllText(scriptPath, scriptContent);

            Debug.Log($"Successfully updated class names, base class, and Initialize method in '{scriptPath}'.");
        }
    }
}
