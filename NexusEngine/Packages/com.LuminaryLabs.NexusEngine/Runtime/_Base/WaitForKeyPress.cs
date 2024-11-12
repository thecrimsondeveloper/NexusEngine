using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class WaitForKeyPress : BaseSequence<WaitForKeyPressData>
{
    private KeyCode keyToPress;

        protected override UniTask Initialize(WaitForKeyPressData currentData)
        {
            keyToPress = currentData.keyToPress;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Wait until the key is pressed
            WaitUntilKeyPress();
        }

        private void WaitUntilKeyPress()
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

public class WaitForKeyPressData : BaseSequenceData
{
      public KeyCode keyToPress;

        // Data for the sequence
}
