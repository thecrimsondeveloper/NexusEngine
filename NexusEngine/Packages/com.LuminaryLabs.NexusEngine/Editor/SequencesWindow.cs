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

    // Retrieve the list of all running sequences
    public List<ISequence> sequences = new List<ISequence>();

    [MenuItem("Luminary Labs/Sequence Viewer")]
    public static void ShowWindow()
    {
        GetWindow<SequencesWindow>("Sequences");
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
    }

    private List<SerializableSequence> BuildSequenceHierarchy(List<ISequence> sequences)
    {
        var sequenceDict = new Dictionary<ISequence, SerializableSequence>();
        var rootSequences = new List<SerializableSequence>();

        // Create SerializableSequence instances and store them in a dictionary
        foreach (var sequence in sequences)
        {
            var serializableSequence = new SerializableSequence(sequence);
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

    class SerializableSequence
    {
        public ISequence sequence;
        public List<SerializableSequence> children;
        private Dictionary<Guid, bool> foldoutStates = new Dictionary<Guid, bool>();

        public SerializableSequence(ISequence sequence)
        {
            this.sequence = sequence;
            children = new List<SerializableSequence>();
        }

        public enum DrawResult
        {
            Success,
            Error
        }

        public DrawResult Draw()
        {
            if (sequence == null)
            {
                return DrawResult.Error;
            }

            // Handle the foldout for the sequence
            Guid key = sequence.guid;
            bool isExpanded = GetFoldoutState(key);

            string label = GetSequenceLabel();
            isExpanded = EditorGUILayout.Foldout(isExpanded, label, true);

            SetFoldoutState(key, isExpanded);

            if (isExpanded)
            {
                EditorGUI.indentLevel++;

                // Show the information of the current sequence
                DrawSequenceInfo();

                GUILayout.Space(10);

                // Draw children
                foreach (var child in children)
                {
                    if (child != null)
                    {
                        child.Draw();
                    }
                    else
                    {

                        return DrawResult.Error;
                    }
                }

                EditorGUI.indentLevel--;
            }

            return DrawResult.Success;
        }

        private string GetSequenceLabel()
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

        private void DrawSequenceInfo()
        {
            EditorGUI.indentLevel++;

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label($"GUID: {sequence.guid}", EditorStyles.boldLabel);
            GUILayout.Label($"Current Data: {sequence.currentData ?? "No Data"}", EditorStyles.label);
            GUILayout.Label($"Super Sequence: {(sequence.superSequence != null ? sequence.superSequence.guid.ToString() : "None")}", EditorStyles.label);

            GUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }
    }
}
