using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.Sequences
{
    public class Sequence : MonoBehaviour
    {
        private static readonly Dictionary<Guid, ISequence> runningSequences = new Dictionary<Guid, ISequence>();

        private static void RegisterSequence(ISequence sequence)
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
            if (runData != null) Debug.Log("Data: " + runData.ToString());

            // Prerequisites
            Debug.Log("Running sequence: " + sequence.GetType().Name);
            if (IsRunning(sequence))
            {
                Debug.LogWarning("Sequence already running");
                return;
            }


            if (runData != null && runData.replace != null && runData.replace.guid != Guid.Empty && runningSequences.ContainsKey(runData.replace.guid))
            {
                await Stop(runData.replace);
            }


            // Instantiate if the sequence is a MonoBehaviour and a prefab
            if (sequence is MonoBehaviour monoBehaviour)
            {
                GameObject sequenceObject = monoBehaviour.gameObject;
                if (sequenceObject.scene.name == null)
                {
                    GameObject instance = Instantiate(sequenceObject);
                    sequence = instance.GetComponent<ISequence>();
                    if (sequence == null)
                    {
                        Debug.LogError("Instantiated object does not contain an ISequence component.");
                        Destroy(instance);
                        return;
                    }
                }
            }


            // Setup and run logic
            sequence.guid = Guid.NewGuid();
            RegisterSequence(sequence);

            if (runData != null && runData.superSequence != null)
            {
                sequence.superSequence = runData.superSequence;
                if (sequence is MonoBehaviour mono)
                {
                    mono.transform.SetParent(runData.superSequence.GetTransform());
                    mono.transform.localPosition = runData.spawnPosition;
                    mono.transform.localRotation = runData.spawnRotation;
                }
            }


            if (runData != null)
            {
                if (runData.sequenceData != null)
                {
                    sequence.currentData = runData.sequenceData;
                    await sequence.InitializeSequence(sequence.currentData);
                }
                else
                {
                    await sequence.InitializeSequence();
                }


            }

            // Begin the sequence
            sequence.OnBeginSequence();
        }

        public static async UniTask Stop(ISequence sequence)
        {
            Debug.Log("Stopping sequence: " + sequence.GetType().Name);
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

            await sequence.UnloadSequence();
            UnregisterSequence(sequence);
        }

        public static async UniTask Finish(ISequence sequence)
        {
            if (!IsRunning(sequence))
            {
                Debug.LogWarning("Sequence not running");
                return;
            }

            await sequence.FinishSequence();

            UnregisterSequence(sequence);
        }

        public static void ForEach(Action<ISequence> action)
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

    public interface ISequence
    {
        ISequence superSequence { get; set; }
        Guid guid { get; set; }
        object currentData { get; set; }

        UniTask InitializeSequence(object currentData = null);
        void OnBeginSequence();
        UniTask FinishSequence();
        UniTask UnloadSequence();
        void OnFinishedSequence();
        void OnUnloadedSequence();

        public Transform GetTransform()
        {
            if (this is MonoBehaviour monoBehaviour)
            {
                return monoBehaviour.transform;
            }
            return superSequence != null ? superSequence.GetTransform() : null;
        }
    }


    public class SequenceRunData
    {
        public ISequence superSequence { get; set; }
        public ISequence replace { get; set; }
        public object sequenceData { get; set; }
        public Vector3 spawnPosition { get; set; }
        public Quaternion spawnRotation { get; set; }
        public override string ToString()
        {
            return "SequenceRunData: " + sequenceData + "\n" +
                    "SuperSequence: " + (superSequence != null ? superSequence.ToString() : "null") + "\n" +
                    "Replace: " + (replace != null ? replace.ToString() : "null") + "\n" +
                    "SpawnPosition: " + spawnPosition + "\n" +
                    "SpawnRotation: " + spawnRotation;
        }
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
