using System.Collections;
using System.Collections.Generic;
using LuminaryLabs.NexusEngine;
using UnityEngine;
using UnityEditor;

public class NexusLogging : NexusFactoryPane
{

    public NexusLogging()
    {
        title = "Logging";
    }
    Vector2 scrollPosition = Vector2.zero;
    protected override void WhenDraw()
    {
        if (GUILayout.Button("Dump Log"))
        {
            DumpLog();
        }

        // Define a scroll position variable to hold the current scroll position within the scroll area

        // Begin a scroll view, making the log entries scrollable
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200)); // Adjust height as needed

        foreach (string log in Nexus.logDump)
        {
            NexusFactory.BeginContentArea();
            GUILayout.Label(log, EditorStyles.boldLabel);
            NexusFactory.EndContentArea();
        }

        // End the scroll view
        EditorGUILayout.EndScrollView();
    }

    private void DumpLog()
    {
        // Implement the functionality to dump or display logs here
        Nexus.LogDump();
    }
}

