using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace LuminaryLabs.NexusEngine
{

    [CreateAssetMenu(fileName = "SequenceEditorSettings", menuName = "LuminaryLabs/NexusEngine/SequenceEditorSettings")]
    public class SequenceEditorSettings : ScriptableObject
    {
        static SequenceEditorSettings instance;



        static SequenceEditorSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.FindObjectsOfTypeAll<SequenceEditorSettings>()[0];
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
