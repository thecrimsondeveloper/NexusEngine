using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class WaitForKeyPressEntity : EntitySequence<WaitForKeyPressEntityData>
    {
        private List<KeyCode> keysToWaitFor;

        protected override UniTask Initialize(WaitForKeyPressEntityData currentData)
        {
            keysToWaitFor = new List<KeyCode>(currentData.keysToWaitFor); // Create a copy of the keys to wait for.
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Start checking for key presses
            if (keysToWaitFor == null || keysToWaitFor.Count == 0)
            {
                Complete().Forget();
            }
        }

        void Update()
        {
            if (keysToWaitFor == null || keysToWaitFor.Count == 0)
                return;

            // Iterate through the keys and remove any that have been pressed.
            for (int i = keysToWaitFor.Count - 1; i >= 0; i--)
            {
                if (Input.GetKeyDown(keysToWaitFor[i]))
                {
                    keysToWaitFor.RemoveAt(i);
                }
            }

            // If the list is empty, all keys have been pressed.
            if (keysToWaitFor.Count == 0)
            {
                Complete().Forget();
            }
        }

        private async UniTaskVoid Complete()
        {
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            // Clean up by destroying the GameObject when the sequence is done
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class WaitForKeyPressEntityData : SequenceData
    {
        public List<KeyCode> keysToWaitFor = new List<KeyCode>();
    }
}
