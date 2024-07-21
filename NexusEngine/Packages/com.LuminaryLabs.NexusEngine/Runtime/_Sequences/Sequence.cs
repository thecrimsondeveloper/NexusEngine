using System;
using System.Collections.Generic;
using System.Reflection;
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

        public static void RegisterSequence(IBaseSequence sequence)
        {
            if (sequence.guid == Guid.Empty)
            {
                sequence.guid = Guid.NewGuid();
            }

            if (runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence already running");
                return;
            }

            runningSequences.Add(sequence.guid, sequence);
        }

        public static void UnregisterSequence(IBaseSequence sequence)
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
            Debug.Log("Running sequence: " + sequence.GetType().Name);
            if (IsRunning(sequence))
            {
                Debug.LogWarning("Sequence already running");
                return;
            }

            if (runData?.Replace != null && IsRunning(runData.Replace))
            {
                await Stop(runData.Replace);
            }

            RegisterSequence(sequence);

            if (sequence is IAsyncSequence aSequence)
            {
                await aSequence.LoadSequence();
            }
            else if (sequence is IAsyncDataSequence aDataSequence)
            {
                await aDataSequence.LoadSequence(runData?.InitializationData);
            }

            if (sequence is ISequence standardSequence)
            {
                standardSequence.Load();
            }
            else if (sequence is IDataSequence dataSequence)
            {
                dataSequence.Load(runData?.InitializationData);
            }

            sequence.SequenceStart();
        }

        public static async UniTask Stop(IBaseSequence sequence)
        {
            if (!IsRunning(sequence))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

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
            if (!IsRunning(sequence))
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

        public static void ForEach(Action<IBaseSequence> action)
        {
            foreach (var sequence in runningSequences.Values)
            {
                action(sequence);
            }
        }

        public static string GetSequenceTree()
        {
            string tree = "";
            foreach (var sequence in runningSequences.Values)
            {
                tree += sequence.GetType().Name + "\n";
            }
            return tree;
        }

        public static List<SequenceDetails> GetSequenceDetails()
        {
            var details = new List<SequenceDetails>();

            foreach (var sequence in runningSequences.Values)
            {
                if (sequence is ISequence seq)
                {
                    var sequenceDetail = new SequenceDetails
                    {
                        SequenceType = seq.GetType().Name,
                        Fields = new List<FieldDetails>()
                    };

                    var fields = seq.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        sequenceDetail.Fields.Add(new FieldDetails
                        {
                            FieldName = field.Name,
                            FieldType = field.FieldType.Name,
                            FieldValue = field.GetValue(seq)?.ToString()
                        });
                    }

                    var properties = seq.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var property in properties)
                    {
                        if (property.CanRead)
                        {
                            sequenceDetail.Fields.Add(new FieldDetails
                            {
                                FieldName = property.Name,
                                FieldType = property.PropertyType.Name,
                                FieldValue = property.GetValue(seq)?.ToString()
                            });
                        }
                    }

                    details.Add(sequenceDetail);
                }
            }

            return details;
        }
    }

    public class SequenceRunData
    {
        public IBaseSequence Replace { get; set; }
        public IBaseSequence SuperSequence { get; set; }
        public object InitializationData { get; set; }
    }

    public class SequenceDetails
    {
        public string SequenceType { get; set; }
        public List<FieldDetails> Fields { get; set; }
    }

    public class FieldDetails
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldValue { get; set; }
    }
}
