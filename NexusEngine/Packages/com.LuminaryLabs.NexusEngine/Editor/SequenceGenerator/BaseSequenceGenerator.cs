using LuminaryLabs.NexusEngine;
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.Editor
{
    public static class BaseSequenceGenerator
    {
        [MenuItem("Assets/Create/#C Script (Base Sequence)", false, 2)]
        private static void CreateBaseSequenceScript()
        {
            MonoScript baseSequenceTemplate = SequenceEditorSettings.GetBaseSequenceTemplate();
            SequenceGenerator.CreateScriptFromTemplate(baseSequenceTemplate, "NewBaseSequence", "Save Base Sequence Script");
        }
    }
}
