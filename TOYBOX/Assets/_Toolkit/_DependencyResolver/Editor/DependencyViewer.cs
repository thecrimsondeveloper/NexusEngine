using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    public class DependencyViewer : EditorWindow
    {
        [MenuItem("LLGD/Dependency Manager/View Dependencies")]
        public static void ShowWindow()
        {
            var window = GetWindow<DependencyViewer>();
            window.titleContent = new GUIContent("Dependency Viewer");
            window.Show();
        }


        Vector2 scrollPosition = Vector2.zero;
        private void OnGUI()
        {
            var dependencies = DependencyManager.singleDependencies;
            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollView.scrollPosition;
                using (var viewer = new DictionaryViewer(dependencies))
                {
                    viewer.Draw();
                }
            }
        }


        class DictionaryViewer : IDisposable
        {
            public DictionaryViewer(Dictionary<DependencyType, DependencyDefinition> dictionary)
            {
                this.dictionary = dictionary;
            }

            private Dictionary<DependencyType, DependencyDefinition> dictionary;

            public void Draw()
            {
                //draw a label showing the number of dependencies
                EditorGUILayout.LabelField($"Number of Dictionary Items: {dictionary.Count}");
                foreach (var dependency in dictionary)
                {
                    EditorGUILayout.LabelField(dependency.Key.ToString());
                    EditorGUILayout.LabelField(dependency.Value.ToString());
                }
            }

            public void Dispose()
            {
                dictionary = null;
            }
        }
    }
}
