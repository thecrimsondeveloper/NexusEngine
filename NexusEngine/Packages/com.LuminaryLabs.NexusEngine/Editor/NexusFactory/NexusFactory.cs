using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// NexusFactory Editor Window
public class NexusFactory : EditorWindow
{
    // List of panes to display
    private List<NexusFactoryPane> factoryPanes = new List<NexusFactoryPane>();

    [MenuItem("Luminary Labs/Factory")]
    public static void ShowWindow()
    {
        GetWindow<NexusFactory>("Nexus Factory");
    }

    // OnEnable to initialize some panes
    private void OnEnable()
    {
        factoryPanes.Add(new NexusMeshBaker());
        factoryPanes.Add(new NexusSkyboxGenerator());
        factoryPanes.Add(new NexusWorldGenerator());
    }

    // OnGUI method of the EditorWindow
    private void OnGUI()
    {
        //Draw the title
        GUILayout.Label("Nexus Factory", EditorStyles.boldLabel);

        //draw a line
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));



        // Draw each pane
        foreach (var pane in factoryPanes)
        {
            BeginContentArea();
            pane.Draw();
            EndContentArea();
        }
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
