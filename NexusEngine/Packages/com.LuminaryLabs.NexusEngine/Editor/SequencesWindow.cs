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

    [MenuItem("LuminaryLabs/Sequence Viewer")]
    public static void ShowWindow()
    {
        GetWindow<SequencesWindow>("Sequences");
    }

    private void OnEnable()
    {
        // Get all sequences in the scene

        Debug.Log("SequencesWindow enabled");
        // Get all sequences in the scene
        sequences = Sequence.GetAll();
        rootSequences = BuildSequenceHierarchy(sequences);
    }



    private void OnGUI()
    {

        if (Application.isPlaying == false)
        {
            GUILayout.Label("Please enter play mode to view running sequences.", EditorStyles.label);

            if (GUILayout.Button("Enter Player Mode"))
            {
                EditorApplication.isPlaying = true;
            }
            return;
        }

        if (GUILayout.Button("Refresh Sequences", GUILayout.Width(200)))
        {
            // Get all sequences in the scene
            sequences = Sequence.GetAll();
            rootSequences = BuildSequenceHierarchy(sequences);
        }

        if (sequences == null || sequences.Count == 0)
        {
            GUILayout.Label("No sequences are currently running.", EditorStyles.label);
            return;
        }

        GUILayout.Label("Running Sequences", EditorStyles.boldLabel);

        // Start a scroll view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);

        // Build and display the sequence hierarchy

        foreach (var rootSequence in rootSequences)
        {
            rootSequence.Draw();
        }

        EditorGUILayout.EndScrollView();
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

        public void Draw()
        {
            if (sequence == null)
            {
                return;
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
                    child.Draw();
                }

                EditorGUI.indentLevel--;
            }
        }

        private string GetSequenceLabel()
        {
            if (sequence is MonoBehaviour monoBehaviour)
            {
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
