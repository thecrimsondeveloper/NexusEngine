using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace LuminaryLabs.NexusEngine
{

    public class TMPTextModifier : SequenceModifier
    {
        private Text text;
        private string newText;

        protected override UniTask Initialize(SequenceModifierData currentData)
        {
            if (currentData is TMPTextModifierData textData)
            {
                text = textData.text;
                newText = textData.newText;
            }

            return UniTask.CompletedTask;
        }

        protected override void ModifySequence(ISequence sequence)
        {
            text.text = newText;

        }
    }

    public class TMPTextModifierData : SequenceModifierData
    {
        public Text text;
        public string newText;
    }

}