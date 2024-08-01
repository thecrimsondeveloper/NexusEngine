using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


namespace LuminaryLabs.NexusEngine
{

    [CreateAssetMenu(fileName = "SequenceEditorSettings", menuName = "LuminaryLabs/NexusEngine/SequenceEditorSettings")]
    public class SequenceEditorSettings : ScriptableObject
    {

        [ShowInInspector]
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
