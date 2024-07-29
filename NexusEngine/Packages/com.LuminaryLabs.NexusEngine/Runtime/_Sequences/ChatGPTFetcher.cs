using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class ChatGPTFetcher : EditorWindow
{
    private string selectedFolderPath = string.Empty;
    private string outputString = string.Empty;
    private Vector2 scrollPosition;

    [MenuItem("Window/ChatGPT Fetcher")]
    public static void ShowWindow()
    {
        GetWindow<ChatGPTFetcher>("ChatGPT Fetcher");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Selected Folder Path:", EditorStyles.boldLabel);
        EditorGUILayout.TextField(selectedFolderPath);

        if (GUILayout.Button("Generate Output"))
        {
            GenerateOutput();
        }

        if (GUILayout.Button("Copy to Clipboard"))
        {
            EditorGUIUtility.systemCopyBuffer = outputString;
        }

        EditorGUILayout.LabelField("Output:", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
        EditorGUILayout.TextArea(outputString, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }

    private void OnInspectorUpdate()
    {
        if (Selection.activeObject != null)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path))
            {
                selectedFolderPath = path;
                Repaint();
            }
        }
    }

    private void GenerateOutput()
    {
        if (string.IsNullOrEmpty(selectedFolderPath))
        {
            outputString = "No folder selected.";
            return;
        }

        StringBuilder sb = new StringBuilder();
        string[] guids = AssetDatabase.FindAssets("t:Script", new[] { selectedFolderPath });

        foreach (string guid in guids)
        {
            string scriptPath = AssetDatabase.GUIDToAssetPath(guid);
            string scriptContent = File.ReadAllText(scriptPath);

            sb.AppendLine($"Script: {Path.GetFileName(scriptPath)}");
            sb.AppendLine(scriptContent);
            sb.AppendLine(new string('-', 80));
            sb.AppendLine();
        }

        outputString = sb.ToString();
    }
}
