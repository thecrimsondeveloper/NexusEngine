using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class NexusPrefabFactory : NexusFactoryPane
    {
        protected override void WhenDraw()
        {
            // Your existing drawing code for the editor window
            // add a GUI button
        }

        [MenuItem("GameObject/Create Mono Sequence", false, 0)]
        private static void CreateMonoSequence()
        {
            CreateSequence<GameObject>(SequenceEditorSettings.MonoSequencePrefab);
        }
        [MenuItem("GameObject/Create Runner Sequence", false, 0)]
        private static void CreateRunnerSequence()
        {
            CreateSequence<MonoSequence>(SequenceEditorSettings.RunnerSequencePrefab);
        }


        private static void CreateSequence<T>(Object referenceObject) where T : Object
        {
            T sequenceTemplate;
            Transform targetParent = null;
            // Create a new GameObject and add a MonoSequence component
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                // If in Prefab Stage, instantiate within the prefab stage
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                sequenceTemplate = (T)PrefabUtility.InstantiatePrefab(referenceObject, prefabStage.scene);

                if (Selection.activeGameObject is GameObject gobj && 
                    gobj.transform.IsChildOf(prefabStage.prefabContentsRoot.transform))
                {
                    targetParent = gobj.transform;
                }
                else
                {
                    targetParent = prefabStage.prefabContentsRoot.transform;
                }
                Debug.Log("ADDING IN PREFAB");
            }
            else
            {
                // Regular scene instantiation
                sequenceTemplate = PrefabUtility.InstantiatePrefab(referenceObject) as T;
                if (Selection.activeGameObject is GameObject gobj && 
                    gobj.scene != null)
                {
                    targetParent = gobj.transform;
                }
                Debug.Log("ADDING IN SCENE");
            }

            GameObject sequenceGameObject = sequenceTemplate is GameObject gameObject ?  gameObject : 
                                        sequenceTemplate is Behaviour behaviour ? behaviour.gameObject :     
                                        null;

            if(targetParent && sequenceGameObject)
            {
                sequenceGameObject.transform.SetParent(targetParent);
            }

            // Register the creation in the undo system for easy revert
            Undo.RegisterCreatedObjectUndo(sequenceTemplate, "Create MonoSequence");

            // Optionally, select the created object in the hierarchy
            Selection.activeGameObject = sequenceGameObject;

            // Trigger the rename in the editor
            EditorApplication.delayCall += () =>
            {
                Selection.activeGameObject = sequenceGameObject;
                // Use SceneView to focus on the object and start renaming
                SceneView.FrameLastActiveSceneView();
                EditorApplication.ExecuteMenuItem("Edit/Rename");
            };
        }

        [MenuItem("GameObject/Create Runner Sequence", false, 0)]
        private static void CreateMonoSequence<T>() where T : MonoBehaviour
        {
            // Create a new GameObject and add a specified component of type T
            GameObject monoSequencePrefab = SequenceEditorSettings.MonoSequencePrefab;

            GameObject monoSequence;
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                // If in Prefab Stage, instantiate within the prefab stage
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                monoSequence = (GameObject)PrefabUtility.InstantiatePrefab(monoSequencePrefab, prefabStage.scene);

                if (Selection.activeGameObject is GameObject gobj && gobj.transform.IsChildOf(prefabStage.prefabContentsRoot.transform))
                {
                    monoSequence.transform.parent = gobj.transform;
                }
            }
            else
            {
                // Regular scene instantiation
                monoSequence = PrefabUtility.InstantiatePrefab(monoSequencePrefab) as GameObject;
                if (Selection.activeGameObject is GameObject gobj && gobj.scene != null)
                {
                    monoSequence.transform.parent = gobj.transform;
                }
            }

            monoSequence.AddComponent<T>();

            // Register the creation in the undo system for easy revert
            Undo.RegisterCreatedObjectUndo(monoSequence, "Create Typed MonoSequence");

            // Optionally, select the created object in the hierarchy
            Selection.activeGameObject = monoSequence.gameObject;

            // Trigger the rename in the editor
            EditorApplication.delayCall += () =>
            {
                Selection.activeGameObject = monoSequence;
                // Use SceneView to focus on the object and start renaming
                SceneView.FrameLastActiveSceneView();
                EditorApplication.ExecuteMenuItem("Edit/Rename");
            };
        }
    }
}
