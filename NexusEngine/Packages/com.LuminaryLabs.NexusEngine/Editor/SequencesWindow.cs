using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LuminaryLabs.NexusEngine;
using System;

public class SequencesWindow : EditorWindow
{
    private Vector2 scrollPos;
    private Dictionary<Guid, bool> foldoutStates = new Dictionary<Guid, bool>();
    List<SerializableSequence> rootSequences = new List<SerializableSequence>();

    string sequenceHeirarchyJSON = "";

    // Retrieve the list of all running sequences
    public List<ISequence> sequences = new List<ISequence>();

    [MenuItem("Luminary Labs/Sequence Viewer")]
    public static void ShowWindow()
    {
        GetWindow<SequencesWindow>("Sequence Viewer");
    }

    float timeLastRefresh = 0;
    private void OnGUI()
    {

        if (Application.isPlaying == false)
        {
            GUILayout.Label("Please enter play mode to view running sequences.", EditorStyles.label);

            if (GUILayout.Button("Enter Play Mode"))
            {
                EditorApplication.isPlaying = true;
            }
            return;
        }


        if (GUILayout.Button("Copy JSON"))
        {
            EditorGUIUtility.systemCopyBuffer = sequenceHeirarchyJSON;
        }

        RefreshSequences();
        // Start a scroll view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);

        // Build and display the sequence hierarchy

        foreach (var rootSequence in rootSequences)
        {

            if (rootSequence != null)
            {
                SerializableSequence.DrawResult result = rootSequence.Draw();
                if (result == SerializableSequence.DrawResult.Error)
                {
                    Debug.LogError("Error drawing sequence.");
                    RefreshSequences();
                    break;
                }
            }
            else
            {
                RefreshSequences();
                break;
            }
        }

        EditorGUILayout.EndScrollView();
    }

    float refreshRate = 10;

    //refresh rate is the amount per second that the window will refresh
    //cooldown is the total time that must pass before the window will refresh again
    float coolDown => refreshRate == 0 ? 0 : 1 / refreshRate;
    void RefreshSequences()
    {
        bool hasLowEnoughRefreshRateToMatter = refreshRate < 20;
        bool hasEnoughTimePassedSinceLastRefresh = Time.time - timeLastRefresh > coolDown;
        if (hasLowEnoughRefreshRateToMatter && hasEnoughTimePassedSinceLastRefresh)
        {
            return;
        }
        timeLastRefresh = Time.time;
        // Get all sequences in the scene
        sequences = Sequence.GetAll();
        rootSequences = BuildSequenceHierarchy(sequences);


        SerializableSequence outputSequence = new SerializableSequence(null, foldoutStates);
        outputSequence.children = rootSequences;
        sequenceHeirarchyJSON = JsonUtility.ToJson(outputSequence, true);

        Repaint();
    }

    private List<SerializableSequence> BuildSequenceHierarchy(List<ISequence> sequences)
    {
        var sequenceDict = new Dictionary<ISequence, SerializableSequence>();
        var rootSequences = new List<SerializableSequence>();

        // Create SerializableSequence instances and store them in a dictionary
        foreach (var sequence in sequences)
        {
            var serializableSequence = new SerializableSequence(sequence, foldoutStates);
            sequenceDict[sequence] = serializableSequence;
        }

        Debug.Log($"Sequence dict count: {sequenceDict.Count}");
        Debug.Log($"Sequences count: {sequences.Count}");

        // Build the hierarchy by assigning children to their respective parents
        foreach (var sequence in sequences)
        {
            var serializableSequence = sequenceDict[sequence];
            bool isRootSequence = sequence.superSequence == null;
            bool isChildSequence = !isRootSequence && sequenceDict.ContainsKey(sequence.superSequence);

            if (isRootSequence)
            {
                rootSequences.Add(serializableSequence);
            }
            else if (isChildSequence)
            {
                var parentSequence = sequenceDict[sequence.superSequence];
                parentSequence.children.Add(serializableSequence);
            }
        }

        Debug.Log($"Root sequences count: {rootSequences.Count}");
        return rootSequences;
    }

    [Serializable]
    class SerializableSequence
    {
        public ISequence sequence;
        public string label;
        public List<SerializableSequence> children;
        private Dictionary<Guid, bool> foldoutStates;

        public SerializableSequence(ISequence sequence, Dictionary<Guid, bool> foldoutStates)
        {
            this.sequence = sequence;

            if (sequence != null)
            {
                label = GetSequenceLabel(sequence);
            }

            this.foldoutStates = foldoutStates;
            children = new List<SerializableSequence>();
        }

        public enum DrawResult
        {
            Success,
            Error
        }

        public DrawResult Draw(int depth = 0)
        {
            if (sequence == null)
            {
                return DrawResult.Error;
            }

            Guid key = sequence.guid;
            bool isExpanded = GetFoldoutState(key);

            // Start a vertical section with a help box style to include the button and label
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();

            // Create an empty label to create the indentation effect
            GUILayout.Label("", GUILayout.Width(depth * 10));

            // Draw the foldout button
            if (GUILayout.Button(isExpanded ? "▼" : "▶", GUILayout.Width(20)))
            {
                isExpanded = !isExpanded;
            }

            // Draw the label
            GUILayout.Label(GetSequenceLabel(sequence));

            EditorGUILayout.EndHorizontal();

            SetFoldoutState(key, isExpanded);

            if (isExpanded)
            {
                EditorGUI.indentLevel++;

                // Show the information of the current sequence inside the box
                DrawSequenceBody();

                GUILayout.Space(10);

                // Draw children
                foreach (var child in children)
                {
                    if (child != null)
                    {
                        DrawResult childResult = child.Draw(depth + 1); // Increase depth for children
                        if (childResult == DrawResult.Error)
                        {
                            return DrawResult.Error;
                        }
                    }
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical(); // End the vertical section with the box style

            return DrawResult.Success;
        }


        private static string GetSequenceLabel(ISequence sequence)
        {
            if (sequence is MonoBehaviour monoBehaviour)
            {
                if (monoBehaviour == null)
                {
                    return "MonoSequence: null";
                }
                return $"Sequence: {monoBehaviour.name}";
            }
            else
            {
                return $"Sequence: {sequence.GetType().Name}";
            }
        }

        private bool GetFoldoutState(Guid superSequenceKey)
        {
            if (!foldoutStates.ContainsKey(superSequenceKey))
                foldoutStates[superSequenceKey] = true;
            return foldoutStates[superSequenceKey];
        }

        private void SetFoldoutState(Guid superSequenceKey, bool state)
        {
            foldoutStates[superSequenceKey] = state;
        }

        private void DrawSequenceBody()
        {
            EditorGUI.indentLevel++; // Increase indent level

            // Draw the super sequence label with indentation
            GUILayout.Label($"Super Sequence: {(sequence.superSequence != null ? label : "None")}", EditorStyles.label);

            // Conditionally display whether the sequence has data
            if (sequence.currentData != null)
            {
                GUILayout.Label("HAS DATA", EditorStyles.label);
            }

            //add a button to select the sequence
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

            EditorGUI.indentLevel--; // Restore the previous indent level
        }

    }
}
