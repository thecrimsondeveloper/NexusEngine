using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Sequences
{
    public class Sequence : MonoBehaviour
    {
        private static readonly Dictionary<Guid, ISequence> runningSequences = new Dictionary<Guid, ISequence>();

        private static void RegisterSequence(ISequence sequence)
        {
            if (runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence already running");
                return;
            }
            runningSequences.Add(sequence.guid, sequence);
        }

        private static void UnregisterSequence(ISequence sequence)
        {
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }
            runningSequences.Remove(sequence.guid);
        }

        public static bool IsRunning(ISequence sequence)
        {
            return runningSequences.ContainsKey(sequence.guid);
        }

        public static async UniTask Run(ISequence sequence, SequenceRunData runData = null)
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
                await Stop(runData.Replace);
            }

            // Setup and run logic
            sequence.guid = Guid.NewGuid();
            RegisterSequence(sequence);

            if (runData != null && runData.SuperSequence != null)
            {
                sequence.superSequence = runData.SuperSequence;
            }

            if (runData != null)
            {
                if (runData.InitializationData != null)
                {
                    sequence.currentData = runData.InitializationData;
                    await sequence.Initialize(sequence.currentData);
                }
                else
                {
                    await sequence.Initialize();
                }
            }

            // Begin the sequence
            sequence.OnBegin();
        }

        public static async UniTask Stop(ISequence sequence)
        {
            Debug.Log("Stopping sequence: " + sequence.GetType().Name);
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

            await sequence.Unload();

            UnregisterSequence(sequence);
        }

        public static async UniTask Finish(ISequence sequence)
        {
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

            await sequence.Finish();

            UnregisterSequence(sequence);
        }
    }

    public class SequenceRunData
    {
        public ISequence SuperSequence { get; set; }
        public ISequence Replace { get; set; }
        public object InitializationData { get; set; }
    }
}
