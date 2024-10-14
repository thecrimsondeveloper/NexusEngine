using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // Assuming you are using UniTask for async operations
using LuminaryLabs.NexusEngine; // Replace with your actual namespace if different

namespace LuminaryLabs.Example
{

    public class WaitForMouseDown : MonoSequence
    {
        protected override UniTask Initialize(object currentData)
        {
            // Any initialization logic can go here
            return UniTask.CompletedTask;
        }
        protected override void OnBegin()
        {

        }

    
        void OnMouseDown()
        {
            Debug.Log("Mouse clicked on " + gameObject.name);
            Complete(); // Complete the sequence
        }

        private async void Complete()
        {
            await Sequence.Finish(this); // Signal that the sequence is finished
            await Sequence.Stop(this); // Cleanup or stop the sequence as needed
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

      
    }

}
