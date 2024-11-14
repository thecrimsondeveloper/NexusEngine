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
                DrawSequencePane(sequence);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    // Refreshes the list of running sequences
    void RefreshSequences()
    {
        sequences = Sequence.GetAll();

    }

    void DrawSequence(ISequence sequence, string prefix = "")
    {


        // Display a reference to the sequence
        if (sequence is UnityEngine.Object uniObject)
        {
            
            // Display the reference to the GameObject if it's a MonoBehaviour
            if (sequence is MonoBehaviour monoBehaviour)
            {
                EditorGUILayout.ObjectField(prefix + "GameObject", monoBehaviour.gameObject, typeof(GameObject), true);
            }
            else
            {
                EditorGUILayout.PropertyField(new SerializedObject(uniObject).FindProperty("m_Script"));
            }
        }
        else
        {
            GUILayout.Label($"Sequence: {sequence.GetType().Name}");
        }
    }



   private void DrawSequencePane(ISequence sequence)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        //make the sequence name bold

        // Add Complete button for the sequence
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name: " + sequence.name, EditorStyles.boldLabel);
         if (GUILayout.Button("Complete"))
        {
            CompleteSequence(sequence);
        }

        EditorGUILayout.EndHorizontal();

        DrawSequence(sequence);


        //draw a read only enum field for the sequence state
        EditorGUILayout.EnumPopup("Phase: ", sequence.phase);//, EditorStyles.miniLabel);

        //draw a serialize field for the object data
        

        if (sequence.superSequence != null)
        {
            DrawSequence(sequence.superSequence, "Super ");
        }




        DrawSequenceDetails(sequence);
       
        EditorGUILayout.EndVertical();
    }

    void DrawSequenceDetails(ISequence sequence)
    {
        NexusFactory.BeginContentArea();
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

        NexusFactory.EndContentArea();
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
                if(value is UnityEngine.Object uniObject)
                {
                    EditorGUILayout.ObjectField("Property: " + property.Name, uniObject, typeof(UnityEngine.Object), true);
                }
                else
                {
                    GUILayout.Label($"Property: {property.Name}: {value}");
                }
            }
            catch (Exception e)
            {
                // GUILayout.Label($"{property.Name}: (Error retrieving value: {e.Message})");
            }
        }

        

        // Retrieve and display all public fields
        FieldInfo[] fields = sequence.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            try
            {
                object value = field.GetValue(sequence);

                if(value is UnityEngine.Object uniObject)
                {
                    EditorGUILayout.ObjectField("Field: " + field.Name, uniObject, typeof(UnityEngine.Object), true);
                }
                else
                {
                    GUILayout.Label($"Field: {field.Name}: {value}");
                }
            }
            catch (Exception e)
            {
                // GUILayout.Label($"{field.Name}: (Error retrieving value: {e.Message})");
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
