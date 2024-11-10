using LuminaryLabs.NexusEngine;
using UnityEditor;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.Editor
{
    public static class EntitySequenceGenerator
    {
        [MenuItem("Assets/Create/#C Script (Entity Sequence)", false, 3)]
        private static void CreateEntitySequenceScript()
        {
            MonoScript entitySequenceTemplate = SequenceEditorSettings.GetEntitySequenceTemplate();
            SequenceGenerator.CreateScriptFromTemplate(entitySequenceTemplate, "NewEntitySequence", "Save Entity Sequence Script");
        }
    }
}

