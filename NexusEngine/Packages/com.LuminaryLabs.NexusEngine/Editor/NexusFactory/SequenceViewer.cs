using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using LuminaryLabs.NexusEngine;
using Cysharp.Threading.Tasks;

public class SequenceViewer : NexusFactoryPane
{
    public SequenceViewer()
    {
        title = "Sequence Viewer";
    }
    private Vector2 scrollPos;
    private Dictionary<ISequence, bool> foldoutStates = new Dictionary<ISequence, bool>();

    // List of all running sequences
    public List<ISequence> sequences = new List<ISequence>();

    protected override void WhenDraw()
    {
        if (!Application.isPlaying)
        {
            GUILayout.Label("Please enter play mode to view running sequences.", EditorStyles.label);

            if (GUILayout.Button("Enter Play Mode"))
            {
                EditorApplication.isPlaying = true;
            }
            return;
        }

        // Refresh sequences on every GUI update
        RefreshSequences();

        // Start a scroll view to list all sequences
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);

        // Iterate through the list of sequences and draw each one
        foreach (var sequence in sequences)
        {
            if (sequence != null)
            {
                DrawSequence(sequence);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    // Refreshes the list of running sequences
    void RefreshSequences()
    {
        sequences = Sequence.GetAll();

    }

    // Draws a single sequence in the list
    private void DrawSequence(ISequence sequence)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        GUILayout.Label(GetSequenceLabel(sequence), EditorStyles.boldLabel);

        if (sequence.superSequence != null)
        {
            GUILayout.Label($"Super Sequence: {GetSequenceLabel(sequence.superSequence)}");
        }
        else
        {
            GUILayout.Label("Super Sequence: None");
        }

        GUILayout.Label(sequence.currentData != null ? "Has Data" : "No Data");

        // Add Finish and Stop buttons for the sequence
        EditorGUILayout.BeginHorizontal();
        // if (GUILayout.Button("Finish"))
        // {
        //     FinishSequence(sequence);
        // }

        // if (GUILayout.Button("Stop"))
        // {
        //     StopSequence(sequence);
        // }
        if (GUILayout.Button("Complete"))
        {
            CompleteSequence(sequence);
        }
        EditorGUILayout.EndHorizontal();

        // Add a foldout for properties and fields of the sequence
        bool foldout = foldoutStates.ContainsKey(sequence) && foldoutStates[sequence];
        foldout = EditorGUILayout.Foldout(foldout, "Sequence Details");

        if (!foldoutStates.ContainsKey(sequence))
        {
            foldoutStates[sequence] = false;
        }
        foldoutStates[sequence] = foldout;

        if (foldout)
        {
            DrawSequenceValues(sequence);
        }

        // Add a button to select the sequence object in the inspector (if applicable)
        if (GUILayout.Button("Select Sequence"))
        {
            if (sequence is MonoBehaviour monoBehaviour)
            {
                Selection.activeObject = monoBehaviour;
            }
            else if (sequence is ScriptableObject scriptableObject)
            {
                Selection.activeObject = scriptableObject;
            }
        }

        EditorGUILayout.EndVertical();
    }

    // Uses reflection to display the properties and fields of the sequence
    private void DrawSequenceValues(ISequence sequence)
    {
        EditorGUI.indentLevel++;

        // Retrieve and display all public properties
        PropertyInfo[] properties = sequence.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            try
            {
                object value = property.GetValue(sequence);
                GUILayout.Label($"{property.Name}: {value}");
            }
            catch (Exception e)
            {
                GUILayout.Label($"{property.Name}: (Error retrieving value: {e.Message})");
            }
        }

        // Retrieve and display all public fields
        FieldInfo[] fields = sequence.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            try
            {
                object value = field.GetValue(sequence);
                GUILayout.Label($"{field.Name}: {value}");
            }
            catch (Exception e)
            {
                GUILayout.Label($"{field.Name}: (Error retrieving value: {e.Message})");
            }
        }

        EditorGUI.indentLevel--;
    }

    // Returns a formatted label for the sequence
    private static string GetSequenceLabel(ISequence sequence)
    {
        if (sequence is MonoBehaviour monoBehaviour)
        {
            return $"Sequence: {monoBehaviour.name}";
        }
        return $"Sequence: {sequence.GetType().Name}";
    }

    // Async method to finish the sequence
    private async void FinishSequence(ISequence sequence)
    {
        if (sequence != null)
        {
            await Sequence.Finish(sequence);
            Debug.Log($"Sequence {sequence.GetType().Name} finished.");
        }
    }

    // Async method to stop the sequence
    private async void StopSequence(ISequence sequence)
    {
        if (sequence != null)
        {
            await Sequence.Stop(sequence);
            Debug.Log($"Sequence {sequence.GetType().Name} stopped.");
        }
    }

    // Async method to stop the sequence
    private async void CompleteSequence(ISequence sequence)
    {
        if (sequence != null)
        {
            await Sequence.Finish(sequence);
            await Sequence.Stop(sequence);
            Debug.Log($"Sequence {sequence.GetType().Name} Completed.");
        }
    }


}
