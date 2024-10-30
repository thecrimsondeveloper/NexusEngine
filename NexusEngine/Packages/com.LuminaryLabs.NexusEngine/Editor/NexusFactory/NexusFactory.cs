using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

// NexusFactory Editor Window
public class NexusFactory : EditorWindow
{
    // List of panes to display
    private List<NexusFactoryPane> factoryPanes = new List<NexusFactoryPane>();
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Luminary Labs/Factory")]
    public static void ShowWindow()
    {
        GetWindow<NexusFactory>("Nexus Factory");
    }

    // OnEnable to initialize some panes
    private void OnEnable()
    {
        factoryPanes.Add(new NexusLogging());
        factoryPanes.Add(new NexusMeshBaker());
        factoryPanes.Add(new NexusSkyboxGenerator());
        factoryPanes.Add(new NexusWorldGenerator());
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
        foreach (var pane in factoryPanes)
        {
            pane.Draw();
            GUILayout.Space(10); // Add some spacing between panes
        }

        // End the scroll view
        GUILayout.EndScrollView();
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
        GUILayout.BeginVertical(EditorStyles.helpBox); // -3 (Content Group)
    }

    public static void EndContentArea()
    {
        GUILayout.EndVertical(); // -3 (Content Group)
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
public abstract class NexusFactoryPane
{
    public string title;          // Title for the foldout
    public bool isFolded = true;  // Whether the foldout is expanded

    public void Draw()
    {


        // // Draw a separate area for the title and foldout
        // GUIStyle titleStyle = new GUIStyle(GUI.skin.box)
        // {
        //     padding = new RectOffset(10, 10, 10, 10),
        //     normal = { background = EditorGUIUtility.isProSkin ? Texture2D.grayTexture : Texture2D.whiteTexture }
        // };

        // GUILayout.BeginVertical(titleStyle); // -2 (Title Group)

        // Foldout logic
        isFolded = EditorGUILayout.Foldout(isFolded, title, true);

        // GUILayout.EndVertical(); // -2 (Title Group)

        // Check if foldout is expanded to display the content
        if (isFolded)
        {
            NexusFactory.BeginContentArea();

            OnDraw(); // Custom content of the pane

            NexusFactory.EndContentArea();
        }
    }

    protected abstract void OnDraw();
}
