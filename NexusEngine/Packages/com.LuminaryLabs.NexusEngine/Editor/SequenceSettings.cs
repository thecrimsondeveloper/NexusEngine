using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    [CreateAssetMenu(fileName = "SequenceEditorSettings", menuName = "LuminaryLabs/NexusEngine/SequenceEditorSettings")]
    public class SequenceEditorSettings : ScriptableObject
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        const string settingsPath = @"Packages\com.LuminaryLabs.NexusEngine\Editor\Settings\";
        static SequenceEditorSettings instance;

        static SequenceEditorSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<SequenceEditorSettings>(settingsPath + "SequenceEditorSettings.asset");
                }

                return instance;
            }
        }

        [SerializeField] MonoScript sequenceTemplate;
        [SerializeField] GameObject monoSequencePrefab;
        [SerializeField] MonoSequence runnerSequencePrefab;

        public static MonoSequence RunnerSequencePrefab
        {
            get
            {
                var settings = Instance;
                return settings ? settings.runnerSequencePrefab : null;
            }
        }

        public static GameObject MonoSequencePrefab
        {
            get
            {
                var settings = Instance;
                return settings ? settings.monoSequencePrefab : null;
            }
        }



        public static MonoScript GetSequenceTemplate()
        {
            SequenceEditorSettings settings = Instance;

            if (settings == null)
            {
                return null;
            }

            return settings.sequenceTemplate;
        }
    }
}
