using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using Toolkit.Sequences;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Toolkit.Sequences
{
    [CreateAssetMenu(fileName = "New Nexus Sequence", menuName = "Toolkit/Sequences/Nexus Sequence")]
    public class NexusSequenceDefinition : ScriptableSequence
    {
        [Title("Sequence Setup")]
        [OnValueChanged(nameof(CheckSubSequences))]
        public List<UnityEngine.Object> subSequences;

        [Title("Debug Info")]
        [ShowInInspector, HideInEditorMode]
        public List<INexusSequence> sequences { get; set; } = new List<INexusSequence>();

#if UNITY_EDITOR
        void CheckSubSequences()
        {
            bool updated = false;
            if (subSequences == null)
            {
                subSequences = new List<UnityEngine.Object>();
            }

            // Loop through all subSequences and check if they are of type INexusSequence
            for (int i = 0; i < subSequences.Count; i++)
            {
                if (!(subSequences[i] is INexusSequence))
                {
                    // Remove the subSequence from the list
                    subSequences.RemoveAt(i);
                    updated = true;
                }
            }

            if (updated)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif

        protected override UniTask Finish()
        {
            // Logic for async finish
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            // Logic for async load
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            // Logic for async unload
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Logic to execute after load
        }

        protected override void OnStart()
        {
            // Logic to execute on start
        }

        protected override void OnFinished()
        {
            // Logic to execute when sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic to execute on sequence unload
        }
    }
}
