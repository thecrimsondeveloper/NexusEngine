using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class NexusPrefabFactory : NexusFactoryPane
    {
        protected override void OnDraw()
        {
            // Your existing drawing code for the editor window
            // add a GUI button
        }

        [MenuItem("GameObject/Create Mono Sequence", false, 0)]
        private static void CreateMonoSequence()
        {
            // Create a new GameObject and add a MonoSequence component
            GameObject monoSequencePrefab = SequenceEditorSettings.MonoSequencePrefab;

            GameObject monoSequence = PrefabUtility.InstantiatePrefab(monoSequencePrefab) as GameObject;

            if(Selection.activeGameObject is GameObject gobj && gobj.scene != null)
            {
                monoSequence.transform.parent = gobj.transform;
            }


            // Register the creation in the undo system for easy revert
            Undo.RegisterCreatedObjectUndo(monoSequence, "Create MonoSequence");

            // Optionally, select the created object in the hierarchy
            Selection.activeGameObject = monoSequence.gameObject;

            // Trigger the rename in the editor
            EditorApplication.delayCall += () =>
            {
                Selection.activeGameObject = monoSequence.gameObject;

                // Use SceneView to focus on the object and start renaming
                SceneView.FrameLastActiveSceneView();
                EditorApplication.ExecuteMenuItem("Edit/Rename");
            };
        }

        [MenuItem("GameObject/Create Runner Sequence", false, 0)]
        private static void CreateRunnerSequence()
        {
            // Create a new GameObject and add a MonoSequence component
            MonoSequence monoSequencePrefab = SequenceEditorSettings.RunnerSequencePrefab;

            MonoSequence monoSequence = PrefabUtility.InstantiatePrefab(monoSequencePrefab) as MonoSequence;

             if(Selection.activeGameObject is GameObject gobj && gobj.scene != null)
            {
                monoSequence.transform.parent = gobj.transform;
            }

            // Register the creation in the undo system for easy revert
            Undo.RegisterCreatedObjectUndo(monoSequence, "Create MonoSequence");

            // Optionally, select the created object in the aject;

            EditorApplication.delayCall += () =>
            {
                Selection.activeGameObject = monoSequence.gameObject;

                // Use SceneView to focus on the object and start renaming
                SceneView.FrameLastActiveSceneView();
                EditorApplication.ExecuteMenuItem("Edit/Rename");
            };
        }
    }
}
