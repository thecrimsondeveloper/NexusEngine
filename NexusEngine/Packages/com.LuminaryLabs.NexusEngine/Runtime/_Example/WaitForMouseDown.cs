using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // Assuming you are using UniTask for async operations
using LuminaryLabs.NexusEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug; // Replace with your actual namespace if different

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
            Stopwatch stopwatch = new Stopwatch(); // Create a stopwatch instance
            stopwatch.Start(); // Start the timer
            
            if (Sequence.IsRunning(this))
            {
                stopwatch.Stop(); // Stop the timer
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds; // Get the elapsed time in milliseconds
                
                Debug.Log("Time taken for Sequence.IsRunning: " + elapsedMilliseconds + " ms");
                
                Complete(); // Complete the sequence
            }
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
