using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LuminaryLabs.NexusEngine;

public class SequencesWindow : EditorWindow
{
    // private List<SequenceState> loadedStates = new List<SequenceState>();
    private Vector2 scrollPos;

    public List<ISequence> sequences => Sequence.GetAll();

    [MenuItem("Tools/Sequences")]
    public static void ShowWindow()
    {
        GetWindow<SequencesWindow>("Sequences");
    }

    private void OnGUI()
    {

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

}
