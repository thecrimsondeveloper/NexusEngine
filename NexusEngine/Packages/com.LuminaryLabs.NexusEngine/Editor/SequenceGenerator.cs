using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LuminaryLabs.NexusEngine;

public class SequenceGenerator : EditorWindow
{
    private MonoScript sequenceTemplateAsset => SequenceEditorSettings.GetSequenceTemplate();

    private SequenceView rootSequenceView;
    private string targetNamespace = "YourNamespace";
    private string selectedPath = "No folder selected";

    [MenuItem("Luminary Labs/Generate Sequence")]
    public static void ShowWindow()
    {
        GetWindow<SequenceGenerator>("Sequence Generator");
    }

    private void OnGUI()
    {
        // Allow user to assign the target namespace
        targetNamespace = EditorGUILayout.TextField("Namespace", targetNamespace);

        // Display the current selected folder path
        GUILayout.Label("Selected Folder:", EditorStyles.boldLabel);
        GUILayout.Label(selectedPath, EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Generate Sequence", EditorStyles.boldLabel);

        MonoScript templateScriptAsset = sequenceTemplateAsset;

        // Allow user to assign the templates
        if (rootSequenceView == null)
        {
            if (GUILayout.Button("Create Root Sequence"))
            {
                rootSequenceView = new SequenceView("Root Sequence", SequenceView.SequenceType.MonoSequence);
            }
        }
        else
        {
            List<SequenceView> sequencesToRemove = new List<SequenceView>();

            DrawSequenceView(rootSequenceView, 0, sequencesToRemove);

            // After drawing, process any sequences marked for removal
            foreach (var sequenceView in sequencesToRemove)
            {
                sequenceView.parent.RemoveSubSequence(sequenceView);
            }

            if (GUILayout.Button("Generate Sequences"))
            {
                if (selectedPath != "No folder selected")
                {
                    GenerateFolderStructure(rootSequenceView, selectedPath);
                    AssetDatabase.Refresh(); // Refresh the AssetDatabase to show the new folders and scripts in Unity
                }
                else
                {
                    EditorUtility.DisplayDialog("No Folder Selected", "Please select a folder in the Project window.", "OK");
                }
            }
        }
    }

    private void OnInspectorUpdate()
    {
        // This method is called at regular intervals; use it to update the selected path
        UpdateSelectedPath();
        Repaint(); // Repaint the window to show the updated path
    }

    private void UpdateSelectedPath()
    {
        var selectedObject = Selection.activeObject;

        if (selectedObject != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedObject);

            if (AssetDatabase.IsValidFolder(path))
            {
                selectedPath = path;
            }
            else
            {
                selectedPath = "No folder selected";
            }
        }
        else
        {
            selectedPath = "No folder selected";
        }
    }

    private void DrawSequenceView(SequenceView sequenceView, int indentLevel, List<SequenceView> sequencesToRemove)
    {
        // Alternate background color based on depth level
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = (indentLevel % 2 == 0) ? Color.white : new Color(0.9f, 0.9f, 0.9f);

        EditorGUILayout.BeginVertical("box");
        EditorGUI.indentLevel = indentLevel;
        sequenceView.isExpanded = EditorGUILayout.Foldout(sequenceView.isExpanded, sequenceView.name, true);

        if (sequenceView.isExpanded)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indentLevel * 20);  // Additional indentation
            sequenceView.name = EditorGUILayout.TextField("Name", sequenceView.name);
            sequenceView.sequenceType = (SequenceView.SequenceType)EditorGUILayout.EnumPopup(sequenceView.sequenceType);

            // Plus and minus buttons
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                sequenceView.AddSubSequence();
            }
            if (sequenceView != rootSequenceView && GUILayout.Button("-", GUILayout.Width(25)))
            {
                sequencesToRemove.Add(sequenceView);
            }
            EditorGUILayout.EndHorizontal();

            // Draw sub-sequences
            foreach (var subSequence in sequenceView.subSequences)
            {
                DrawSequenceView(subSequence, indentLevel + 1, sequencesToRemove);
            }
        }

        EditorGUILayout.EndVertical();
        GUI.backgroundColor = originalColor;  // Restore original color
    }

    private void GenerateFolderStructure(SequenceView sequenceView, string parentPath)
    {
        // Create the folder for the current sequence
        string sequencePath = Path.Combine(parentPath, sequenceView.name);
        if (!AssetDatabase.IsValidFolder(sequencePath))
        {
            AssetDatabase.CreateFolder(parentPath, sequenceView.name);
        }

        // Generate script file for the current sequence
        GenerateSequenceScript(sequenceView, sequencePath);

        // Recursively create folders and scripts for all sub-sequences
        foreach (var subSequence in sequenceView.subSequences)
        {
            GenerateFolderStructure(subSequence, sequencePath);
        }
    }

    private void GenerateSequenceScript(SequenceView sequenceView, string path)
    {
        string scriptName = sequenceView.name.Replace(" ", "") + ".cs";
        string scriptPath = Path.Combine(path, scriptName);

        if (File.Exists(scriptPath))
        {
            Debug.LogWarning("Script already exists: " + scriptPath);
            return;
        }

        MonoScript templateScript = sequenceTemplateAsset;

        if (templateScript != null)
        {
            string templateContent = templateScript.text;
            string className = sequenceView.name.Replace(" ", "");
            string baseClass = GetBaseClassForSequenceType(sequenceView.sequenceType);

            // Prepare sub-sequence and data fields content
            string subSequenceFields = GenerateSubSequenceFields(sequenceView);
            string subSequenceDataFields = GenerateSubSequenceDataFields(sequenceView);
            string dataFields = GenerateDataFields(sequenceView);
            // string sequenceRunLogic = GenerateSequenceRunLogic(sequenceView);

            string modifiedContent = templateContent
                .Replace("TemplateClass", className)
                .Replace("TemplateBaseClass", baseClass)
                .Replace("YourNamespace", targetNamespace)
                .Replace("#region SubSequences", "#region SubSequences\n" + subSequenceFields)
                .Replace("#region SubSequenceData", "#region SubSequenceData\n" + subSequenceDataFields)
                .Replace("#region Data Fields", "#region Data Fields\n" + dataFields);
            // .Replace("#region Template Sequence Run", sequenceRunLogic);

            File.WriteAllText(scriptPath, modifiedContent);
            Debug.Log("Script created: " + scriptPath);
        }
        else
        {
            Debug.LogWarning("No template script assigned.");
        }
    }

    private string GenerateSubSequenceFields(SequenceView sequenceView)
    {
        string fields = "";

        foreach (var subSequence in sequenceView.subSequences)
        {
            string fieldName = char.ToLowerInvariant(subSequence.name[0]) + subSequence.name.Substring(1);
            fields += $"\t\tpublic {subSequence.name} {fieldName}Sequence;\n";
        }

        return fields;
    }

    private string GenerateSubSequenceDataFields(SequenceView sequenceView)
    {
        string fields = "";

        foreach (var subSequence in sequenceView.subSequences)
        {
            string fieldName = char.ToLowerInvariant(subSequence.name[0]) + subSequence.name.Substring(1);
            fields += $"\t\tpublic {subSequence.name}Data {fieldName}Data;\n";
        }

        return fields;
    }

    private string GenerateDataFields(SequenceView sequenceView)
    {
        string fields = "";

        foreach (var subSequence in sequenceView.subSequences)
        {
            string fieldName = char.ToLowerInvariant(subSequence.name[0]) + subSequence.name.Substring(1);
            fields += $"\t\tpublic {subSequence.name}Data {fieldName}Data;\n";
        }

        return fields;
    }

    private string GenerateSequenceRunLogic(SequenceView sequenceView)
    {
        string sequenceRunLogic = "";

        foreach (var subSequence in sequenceView.subSequences)
        {
            string fieldName = char.ToLowerInvariant(subSequence.name[0]) + subSequence.name.Substring(1);
            string sequenceRunTemplate = $@"
            #region {subSequence.name} Sequence Run
            // Run {subSequence.name}
            Sequence.Run({fieldName}Sequence, new SequenceRunData
            {{
                superSequence = this,
                sequenceData = currentData.{fieldName}Data,
                onFinished = {fieldName}Finished
            }});
            #endregion";

            sequenceRunLogic += sequenceRunTemplate + "\n";
        }

        return sequenceRunLogic;
    }

    private string GetBaseClassForSequenceType(SequenceView.SequenceType sequenceType)
    {
        switch (sequenceType)
        {
            case SequenceView.SequenceType.MonoSequence:
                return "MonoSequence";
            case SequenceView.SequenceType.ScriptableSequence:
                return "ScriptableSequence";
            case SequenceView.SequenceType.BaseSequence:
                return "BaseSequence";
            default:
                return "MonoSequence"; // Default fallback
        }
    }

    public class SequenceView
    {
        public enum SequenceType
        {
            MonoSequence,
            ScriptableSequence,
            BaseSequence
        }

        public string name;
        public SequenceType sequenceType;
        public List<SequenceView> subSequences = new List<SequenceView>();
        public bool isExpanded;
        public SequenceView parent;

        public SequenceView(string name, SequenceType sequenceType)
        {
            this.name = name;
            this.sequenceType = sequenceType;
            this.isExpanded = true;  // Default to expanded
        }

        public void AddSubSequence()
        {
            var subSequence = new SequenceView("Sub-sequence", SequenceType.MonoSequence);
            subSequence.parent = this;
            subSequences.Add(subSequence);
        }

        public void RemoveSubSequence(SequenceView sequenceView)
        {
            subSequences.Remove(sequenceView);
        }
    }
}
