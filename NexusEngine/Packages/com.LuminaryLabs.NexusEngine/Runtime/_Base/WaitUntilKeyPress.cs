using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;


namespace LuminaryLabs.NexusEngine
{
    public class WaitUntilKeyPress : BaseSequence<WaitUntilKeyPressData>
    {
        private KeyCode keyToPress;

        protected override UniTask Initialize(WaitUntilKeyPressData currentData)
        {
            keyToPress = currentData.keyToPress;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Wait until the key is pressed
            WaitForKeyPress();
        }

        private void WaitForKeyPress()
        {
            UniTask.WaitUntil(() => Input.GetKeyDown(keyToPress)).ContinueWith(KeyPressed);
        }

        void KeyPressed()
        {

            Nexus.Log("Key Pressed");

            // Key was pressed
            this.Complete();
        }
    }

    [System.Serializable]
    public class WaitUntilKeyPressData : BaseSequenceData
    {
        public KeyCode keyToPress;

        // Data for the sequence
    }
}
