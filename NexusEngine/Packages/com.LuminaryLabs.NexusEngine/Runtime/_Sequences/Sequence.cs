using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sequences
{
    [Serializable]
    public class SequenceEvent : UnityEvent<IBaseSequence> { }

    public class Sequence : MonoBehaviour
    {
        private static readonly Dictionary<Guid, IBaseSequence> runningSequences = new Dictionary<Guid, IBaseSequence>();

        private static void RegisterSequence(IBaseSequence sequence)
        {
            if (runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence already running");
                return;
            }
            runningSequences.Add(sequence.guid, sequence);
        }

        private static void UnregisterSequence(IBaseSequence sequence)
        {
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }
            runningSequences.Remove(sequence.guid);
        }

        public static bool IsRunning(IBaseSequence sequence)
        {
            return runningSequences.ContainsKey(sequence.guid);
        }

        public static async UniTask Run(IBaseSequence sequence, SequenceRunData runData = null)
        {
            // Prerequisites
            Debug.Log("Running sequence: " + sequence.GetType().Name);
            if (sequence.guid != Guid.Empty && runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence already running");
                return;
            }

            if (runData != null && runData.Replace != null && runData.Replace.guid != Guid.Empty && runningSequences.ContainsKey(runData.Replace.guid))
            {
                if (sequence is IAsyncSequence || sequence is IAsyncDataSequence)
                {
                    await Stop(runData.Replace);
                }
                else
                {
                    Stop(runData.Replace);
                }
            }

            // Setup and run logic
            sequence.guid = Guid.NewGuid();
            RegisterSequence(sequence);

            // Async load if the sequence has async functions
            if (sequence is IAsyncSequence aSequence)
            {
                await aSequence.LoadSequence();
            }
            else if (sequence is IAsyncDataSequence aDataSequence)
            {
                await aDataSequence.LoadSequence(runData?.InitializationData);
            }

            // Post load logic
            if (sequence is ISequence standardSequence)
            {
                standardSequence.Load();
            }
            else if (sequence is IDataSequence dataSequence)
            {
                dataSequence.Load(runData?.InitializationData);
            }

            if (runData != null && runData.SuperSequence != null)
            {
                sequence.superSequence = runData.SuperSequence;
            }

            // Start the sequence
            sequence.SequenceStart();
        }


        public static async UniTask Stop(IBaseSequence sequence)
        {
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

            // If has async functions, await
            if (sequence is IAsyncSequence aSequence)
            {
                await aSequence.UnloadSequence();
            }

            if (sequence is ISequence iSequence)
            {
                iSequence.Unload();
            }
            else if (sequence is IDataSequence dataSequence)
            {
                dataSequence.Unload();
            }

            UnregisterSequence(sequence);
        }


        public static async UniTask Finish(IBaseSequence sequence)
        {
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

            if (sequence is IAsyncSequence aSequence)
            {
                await aSequence.FinishSequence();
            }

            if (sequence is ISequence iSequence)
            {
                iSequence.SequenceFinished();
            }
            else if (sequence is IDataSequence dataSequence)
            {
                dataSequence.SequenceFinished();
            }

            UnregisterSequence(sequence);
        }
    }

    public class SequenceRunData
    {
        public IBaseSequence SuperSequence { get; set; }
        public IBaseSequence Replace { get; set; }
        public object InitializationData { get; set; }
    }
}
