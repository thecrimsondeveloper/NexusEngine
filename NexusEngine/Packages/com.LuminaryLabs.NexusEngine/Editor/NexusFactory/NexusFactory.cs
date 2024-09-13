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
    }

    // OnGUI method of the EditorWindow
    private void OnGUI()
    {
        // Draw each pane
        foreach (var pane in factoryPanes)
        {
            pane.Draw();
        }
    }
}
// Custom class NexusFactoryPane
public abstract class NexusFactoryPane
{
    public string title;          // Title for the foldout
    public bool isFolded = true;  // Whether the foldout is expanded
    public GUIStyle boxStyle;     // Style for the border around the pane

    public void Draw()
    {
        boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.padding = new RectOffset(10, 10, 10, 10);
        GUILayout.BeginVertical(boxStyle);

        // Foldout logic
        isFolded = EditorGUILayout.Foldout(isFolded, title, true);

        if (isFolded)
        {
            OnDraw();
        }

        GUILayout.EndVertical();
    }

    protected abstract void OnDraw();
}
