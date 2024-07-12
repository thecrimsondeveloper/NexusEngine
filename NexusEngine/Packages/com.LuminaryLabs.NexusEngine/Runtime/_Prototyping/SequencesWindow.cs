using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Toolkit.Sequences;

public class SequencesWindow : EditorWindow
{
    private List<SequenceState> loadedStates = new List<SequenceState>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Sequences")]
    public static void ShowWindow()
    {
        GetWindow<SequencesWindow>("Sequences");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Save Sequence States"))
        {
            SaveSequenceStates();
        }

        if (GUILayout.Button("Load Sequence States"))
        {
            LoadSequenceStates();
        }

        if (GUILayout.Button("Display Sequence Tree"))
        {
            Debug.Log(Sequence.GetSequenceTree());
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (loadedStates != null && loadedStates.Count > 0)
        {
            GUILayout.Label("Loaded Sequence States:", EditorStyles.boldLabel);
            foreach (var state in loadedStates)
            {
                EditorGUILayout.BeginVertical("box");
                DisplayState(state);
                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void SaveSequenceStates()
    {
        // Sequence.Save();
        Debug.Log("Sequence states saved.");
    }

    private void LoadSequenceStates()
    {
        // loadedStates = SequenceStateSaver.LoadSequenceStates();
        Debug.Log("Sequence states loaded.");
    }

    private void DisplayState(SequenceState state)
    {

    }
}
