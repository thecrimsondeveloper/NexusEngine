using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// NexusFactory Editor Window
public class NexusFactory : EditorWindow
{
    // Dictionary to store one instance of each pane type
    private Dictionary<Type, NexusFactoryPane> factoryPanes = new Dictionary<Type, NexusFactoryPane>();
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Luminary Labs/Factory")]
    public static void ShowWindow()
    {
        GetWindow<NexusFactory>("Nexus Factory");
    }

    // OnEnable to initialize panes
    private void OnEnable()
    {
        AddPane<SequenceViewer>();
        AddPane<NexusLogging>();
        AddPane<NexusMeshBaker>();
        AddPane<NexusSkyboxGenerator>();
        AddPane<NexusWorldGenerator>();
    }

    // Method to add a pane only if it doesn’t already exist in the dictionary
    private void AddPane<T>() where T : NexusFactoryPane, new()
    {
        Type paneType = typeof(T);
        if (!factoryPanes.ContainsKey(paneType))
        {
            factoryPanes[paneType] = new T();
        }
    }

    // OnGUI method of the EditorWindow
    private void OnGUI()
    {
        // Draw the title
        GUILayout.Label("Nexus Factory", EditorStyles.boldLabel);

        // Draw a horizontal line
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

        // Begin the scroll view
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

        // Draw each pane
        foreach (var pane in factoryPanes.Values)
        {
            pane.DrawContent();
            GUILayout.Space(3); // Add some spacing between panes
        }

        // End the scroll view
        GUILayout.EndScrollView();

        Repaint();
    }

    public static void BeginContentArea()
    {
        // Draw a border for the content area
        Rect innerRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
        innerRect.yMin += 5; // Add padding to avoid overlapping with the outer group
        innerRect.xMin += 5;
        innerRect.xMax -= 5;
        innerRect.yMax += 5;

        // Outline color for content (You can modify this to the desired color)
        Color contentOutlineColor = Color.yellow;
        EditorGUI.DrawRect(innerRect, contentOutlineColor); // Draw the outline background

        // Draw the inner content area within the border
        GUILayout.BeginVertical(EditorStyles.helpBox);
    }

    public static void EndContentArea()
    {
        GUILayout.EndVertical();
    }

    public static string GetCurrentFolderPath()
    {
        string folderPath = "Assets"; // Default path is the root of the Assets folder

        try
        {
            // Get the Project Browser window type
            Type projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser, UnityEditor");

            // Get all open instances of the Project Browser
            EditorWindow[] projectBrowsers = Resources.FindObjectsOfTypeAll(projectBrowserType) as EditorWindow[];

            if (projectBrowsers != null && projectBrowsers.Length > 0)
            {
                // Get the first instance (there could be multiple Project Browsers)
                var projectBrowser = projectBrowsers[0];

                // Use reflection to access the internal field m_ViewMode
                FieldInfo viewModeField = projectBrowserType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic);
                int viewMode = (int)viewModeField.GetValue(projectBrowser);

                if (viewMode == 1) // 1 means 'TwoColumns' mode
                {
                    // Get the selected folder paths in the project browser
                    MethodInfo getSelectedFoldersMethod = projectBrowserType.GetMethod("GetSelectedFolders", BindingFlags.Instance | BindingFlags.NonPublic);
                    string[] selectedFolders = (string[])getSelectedFoldersMethod.Invoke(projectBrowser, null);

                    if (selectedFolders != null && selectedFolders.Length > 0)
                    {
                        folderPath = selectedFolders[0];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error getting current folder path: " + ex.Message);
        }

        return folderPath;
    }
}

// Custom class NexusFactoryPane
public abstract class NexusFactoryPane : ScriptableObject
{
    public string title;          // Title for the foldout
    public bool isFolded = false;  // Whether the foldout is expanded

    public void DrawContent()
    {
        NexusFactory.BeginContentArea();
        isFolded = EditorGUILayout.Foldout(isFolded, title, true);

        if (isFolded)
        {
            NexusFactory.BeginContentArea();
            WhenDraw();
            NexusFactory.EndContentArea();
        }

        NexusFactory.EndContentArea();
    }

    protected abstract void WhenDraw();
}
