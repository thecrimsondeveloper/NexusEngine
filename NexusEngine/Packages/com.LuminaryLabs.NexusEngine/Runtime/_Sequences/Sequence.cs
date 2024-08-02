using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Reflection.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.Sequences
{
    public class Sequence : MonoBehaviour
    {
        [ShowInInspector] private static readonly Dictionary<Guid, ISequence> runningSequences = new Dictionary<Guid, ISequence>();
        [ShowInInspector] private static readonly Dictionary<Guid, SequenceEvents> sequenceEvents = new Dictionary<Guid, SequenceEvents>();


        public static List<ISequence> GetAll() => new List<ISequence>(runningSequences.Values);

        private static SequenceEvents RegisterSequence(ISequence sequence, SequenceRunData runData)
        {
            if (sequence.guid == Guid.Empty)
                sequence.guid = Guid.NewGuid();
            if (!runningSequences.ContainsKey(sequence.guid))
            {
                SequenceEvents events = new SequenceEvents();
                runningSequences.Add(sequence.guid, sequence);
                sequenceEvents.Add(sequence.guid, events);

                if (runData.onInitialize != null) events.RegisterEvent(runData.onInitialize, SequenceEventType.OnInitialize);
                if (runData.onBegin != null) events.RegisterEvent(runData.onBegin, SequenceEventType.OnBegin);
                if (runData.onFinished != null) events.RegisterEvent(runData.onFinished, SequenceEventType.OnFinished);
                if (runData.onUnloaded != null) events.RegisterEvent(runData.onUnloaded, SequenceEventType.OnUnloaded);
                return events;
            }
            else if (sequenceEvents.TryGetValue(sequence.guid, out var sequenceEvent))
            {
                if (runData.onInitialize != null) sequenceEvent.RegisterEvent(runData.onInitialize, SequenceEventType.OnInitialize);
                if (runData.onBegin != null) sequenceEvent.RegisterEvent(runData.onBegin, SequenceEventType.OnBegin);
                if (runData.onFinished != null) sequenceEvent.RegisterEvent(runData.onFinished, SequenceEventType.OnFinished);
                if (runData.onUnloaded != null) sequenceEvent.RegisterEvent(runData.onUnloaded, SequenceEventType.OnUnloaded);
                return sequenceEvent;
            }
            return null;
        }

        private static void UnregisterSequence(ISequence sequence)
        {
            if (runningSequences.ContainsKey(sequence.guid))
            {
                runningSequences.Remove(sequence.guid);

            }

            if (sequenceEvents.ContainsKey(sequence.guid))
            {
                sequenceEvents.Remove(sequence.guid);
            }


        }

        public static bool IsRunning(ISequence sequence) => runningSequences.ContainsKey(sequence.guid);

        public static SequenceRunResult Run(ISequence sequence, SequenceRunData runData = null)
        {
            SequenceRunResult sequenceObject = new SequenceRunResult();
            sequenceObject.sequence = sequence;

            if (IsRunning(sequence))
            {
                if (sequenceEvents.TryGetValue(sequence.guid, out var runningEvents))
                    sequenceObject.events = runningEvents;
                return sequenceObject;
            }
            //setup
            if (runData == null)
            {
                runData = new SequenceRunData();
                runData.wasGenerated = true;
            }

            sequence = HandleInstantiation(sequence, runData);
            SequenceEvents events = RegisterSequence(sequence, runData);
            // Start the sequence and store the running task
            UniTask runningTask = RunSequence(sequence, events, runData);
            sequenceObject.SetTask(runningTask);
            sequenceObject.events = events;

            return sequenceObject;
        }



        static async UniTask RunSequence(ISequence sequence, SequenceEvents events, SequenceRunData runData)
        {
            //cleanup
            bool hasReplacement = runData.replace != null;
            if (hasReplacement && IsRunning(runData.replace))
            {
                await Stop(runData.replace);
            }
            await UniTask.NextFrame();
            Debug.Log("Sequence data: " + sequence.currentData == null);
            Debug.Log("Current data Null: " + (runData.sequenceData == null));
            if (runData.sequenceData != null)
            {
                sequence.currentData = runData.sequenceData;
            }


            events = RegisterSequence(sequence, runData);
            await sequence.InitializeSequence(runData.sequenceData);
            if (sequence is MonoSequence monoSequence)
            {
                if (runData.parent != null)
                {
                    monoSequence.transform.SetParent(runData.parent);
                }


                monoSequence.transform.localPosition = runData.spawnPosition;
                monoSequence.transform.localRotation = runData.spawnRotation;
            }

            if (events != null) events.InvokeEvent(SequenceEventType.OnInitialize); // OnBegin
            sequence.OnBeginSequence();
            if (events != null) events.InvokeEvent(SequenceEventType.OnBegin); // OnBegin
        }

        public static async UniTask Stop(ISequence sequence)
        {
            if (!IsRunning(sequence))
            {
                Debug.LogWarning("Sequence not running");
            }

            await sequence.UnloadSequence();
            sequenceEvents[sequence.guid].InvokeEvent(SequenceEventType.OnUnloaded); // OnUnloaded
            UnregisterSequence(sequence);
        }

        public static async UniTask Finish(ISequence sequence)
        {
            if (!IsRunning(sequence))
            {
                return;
            }

            await sequence.FinishSequence();
            sequenceEvents[sequence.guid].InvokeEvent(SequenceEventType.OnFinished); // OnFinished
            UnregisterSequence(sequence);
        }

        public static void ForEach(Action<ISequence> action)
        {
            foreach (var sequence in runningSequences.Values)
                action(sequence);
        }
        private static ISequence HandleInstantiation(ISequence sequence, SequenceRunData runData = null)
        {
            // if (sequence is ScriptableObject scriptableObject)
            // {
            //     ISequence scriptableInstance = Instantiate(scriptableObject) as ISequence;
            //     return scriptableInstance;
            // }

            if (sequence is MonoBehaviour monoBehaviour && monoBehaviour.gameObject.scene.name == null)
            {
                MonoBehaviour monoInstance = Instantiate(monoBehaviour);

                if (runData != null)
                {
                    bool hasTargetParent = runData.parent != null;
                    bool hasSuperSequence = runData.superSequence != null;
                    if (hasTargetParent == false && hasSuperSequence)
                        monoInstance.transform.SetParent(runData.superSequence.GetTransform());
                    runData.onGenerated?.Invoke(monoInstance);
                }

                if (monoInstance is ISequence sequenceInstance)
                {
                    return sequenceInstance;
                }
            }
            return sequence;
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

        public Transform GetTransform()
        {
            return this is MonoBehaviour monoBehaviour ? monoBehaviour.transform : superSequence?.GetTransform();
        }
    }

    public class SequenceRunData
    {
        public ISequence superSequence { get; set; }
        public ISequence replace { get; set; }
        public object sequenceData { get; set; }
        public Vector3 spawnPosition { get; set; }
        public Quaternion spawnRotation { get; set; }
        public Transform parent { get; set; }
        public bool wasGenerated { get; set; } = false;

        public UnityAction onInitialize { get; set; }
        public UnityAction onBegin { get; set; }
        public UnityAction onFinished { get; set; }
        public UnityAction onUnloaded { get; set; }
        public UnityAction<MonoBehaviour> onGenerated { get; set; }

        public override string ToString() => $"SequenceRunData: {sequenceData}\nSuperSequence: {superSequence}\nReplace: {replace}\nSpawnPosition: {spawnPosition}\nSpawnRotation: {spawnRotation}\nParent: {parent}";
    }

    [Serializable]
    public class SequenceDetails
    {
        public string SequenceType { get; set; }

    }

    public class FieldDetails
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldValue { get; set; }
    }


    public class SequenceRunResult
    {
        public ISequence sequence { get; set; }
        public SequenceEvents events { get; set; }
        private UniTask task { get; set; } = default;

        public void SetTask(UniTask task)
        {
            this.task = task;
        }

        public async UniTask Async()
        {
            if (task.Status == UniTaskStatus.Pending)
                await task;
        }

    }
}
