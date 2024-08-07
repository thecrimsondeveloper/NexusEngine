using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using LuminaryLabs.NexusEngine;

public class SceneReferencesFinderWindow : EditorWindow
{
    private List<SceneReference> references = new List<SceneReference>();

    [MenuItem("Tools/Find ISequence References")]
    public static void ShowWindow()
    {
        GetWindow<SceneReferencesFinderWindow>("ISequence References Finder");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find ISequence References"))
        {
            LoadAllSceneReferences();
        }

        if (references != null && references.Count > 0)
        {
            GUILayout.Label("ISequence References:", EditorStyles.boldLabel);
            foreach (var reference in references)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("GameObject:", reference.GameObject.name);
                EditorGUILayout.LabelField("Component:", reference.Component.GetType().Name);
                EditorGUILayout.LabelField("Field:", reference.FieldInfo.Name);
                EditorGUILayout.LabelField("Value:", reference.Value != null ? reference.Value.ToString() : "null");
                EditorGUILayout.EndVertical();
            }
        }
    }

    private void LoadAllSceneReferences()
    {
        references = GetAllSceneReferences();
    }

    public static List<SceneReference> GetAllSceneReferences()
    {
        List<SceneReference> sceneReferences = new List<SceneReference>();

        // Get all GameObjects in the scene
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        foreach (var go in allGameObjects)
        {
            // Get all components attached to the GameObject
            Component[] components = go.GetComponents<Component>();

            foreach (var component in components)
            {
                // Use reflection to get all public and serialized fields of the component
                FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    // Check if the field is public or marked with [SerializeField]
                    if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                    {
                        object value = field.GetValue(component);

                        // Check if the field value is of type ISequence
                        if (typeof(ISequence).IsAssignableFrom(field.FieldType))
                        {
                            sceneReferences.Add(new SceneReference(go, component, field, value));
                        }
                    }
                }
            }
        }

        return sceneReferences;
    }
}

public class SceneReference
{
    public GameObject GameObject { get; }
    public Component Component { get; }
    public FieldInfo FieldInfo { get; }
    public object Value { get; }

    public SceneReference(GameObject gameObject, Component component, FieldInfo fieldInfo, object value)
    {
        GameObject = gameObject;
        Component = component;
        FieldInfo = fieldInfo;
        Value = value;
    }
}
