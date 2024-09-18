using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace LuminaryLabs.NexusEngine
{
    public class Sequence : MonoBehaviour
    {
#if ODIN_INSPECTOR  
        [Title("Sequence View")]
        [ShowInInspector]
#endif

        List<SequenceStructure> sequenceStructures => SequenceStructure.BuildSequenceStructures(GetAll());
#if ODIN_INSPECTOR  
        [Title("Sequence Data")]
        [ShowInInspector]
#endif
        private static readonly Dictionary<Guid, ISequence> runningSequences = new Dictionary<Guid, ISequence>();

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif

        private static readonly Dictionary<Guid, SequenceEvents> sequenceEvents = new Dictionary<Guid, SequenceEvents>();

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
                if (runData.onUnload != null) events.RegisterEvent(runData.onUnload, SequenceEventType.OnUnload);
                if (runData.onUnloaded != null) events.RegisterEvent(runData.onUnloaded, SequenceEventType.OnUnloaded);
                return events;
            }
            else if (sequenceEvents.TryGetValue(sequence.guid, out var sequenceEvent))
            {
                if (runData.onInitialize != null) sequenceEvent.RegisterEvent(runData.onInitialize, SequenceEventType.OnInitialize);
                if (runData.onBegin != null) sequenceEvent.RegisterEvent(runData.onBegin, SequenceEventType.OnBegin);
                if (runData.onFinished != null) sequenceEvent.RegisterEvent(runData.onFinished, SequenceEventType.OnFinished);
                if (runData.onUnload != null) sequenceEvent.RegisterEvent(runData.onUnload, SequenceEventType.OnUnload);
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

            string name = sequence.GetType().Name;
            Debug.Log("Running Sequence: " + name);
            bool hasData = runData != null;
            Debug.Log("Has Data: " + hasData);

            if (IsRunning(sequence))
            {
                if (sequenceEvents.TryGetValue(sequence.guid, out var runningEvents))
                    sequenceObject.events = runningEvents;
                return sequenceObject;
            }

            if (runData == null)
            {
                runData = new SequenceRunData();
            }

            sequence = HandleInstantiation(sequence, runData);

            //handle anything to do with Unity Objects
            if (sequence is MonoBehaviour monoBehaviour)
            {
                HandleMonoBehaviour(monoBehaviour, runData);
            }


            SequenceEvents events = RegisterSequence(sequence, runData);
            // Start the sequence and store the running task
            UniTask runningTask = RunSequence(sequence, events, runData);
            sequenceObject.SetTask(runningTask);
            sequenceObject.events = events;

            return sequenceObject;
        }

        static void HandleMonoBehaviour(MonoBehaviour sequence, SequenceRunData runData = null)
        {
            if (runData.parent != null)
            {
                sequence.transform.SetParent(runData.parent);
            }

            //set the position and rotation
            if (runData.useLocalPosition)
                sequence.transform.localPosition = runData.spawnPosition;
            else sequence.transform.position = runData.spawnPosition;

            if (runData.useLocalRotation)
                sequence.transform.localRotation = runData.spawnRotation;
            else sequence.transform.rotation = runData.spawnRotation;
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

            if (runData.superSequence != null)
            {
                sequence.superSequence = runData.superSequence;
            }
            await sequence.InitializeSequence(runData.sequenceData);


            if (events != null) events.InvokeEvent(SequenceEventType.OnInitialize, sequence); // OnBegin
            sequence.OnBeginSequence();
            if (events != null) events.InvokeEvent(SequenceEventType.OnBegin, sequence); // OnBegin
        }

        public static async UniTask Stop(ISequence sequence)
        {
            if (!IsRunning(sequence))
            {
                Debug.LogWarning("Sequence not running");
            }

            await sequence.UnloadSequence();
            if (sequenceEvents.TryGetValue(sequence.guid, out var events))
                events.InvokeEvent(SequenceEventType.OnUnloaded, sequence); // OnUnloaded
            UnregisterSequence(sequence);
        }

        public static async UniTask Finish(ISequence sequence)
        {
            if (!IsRunning(sequence))
            {
                return;
            }

            await sequence.FinishSequence();
            if (sequenceEvents.TryGetValue(sequence.guid, out var events))
                events.InvokeEvent(SequenceEventType.OnFinished, sequence); // OnFinished
            UnregisterSequence(sequence);
        }

        public static void ForEach(Action<ISequence> action)
        {
            foreach (var sequence in runningSequences.Values)
                action(sequence);
        }
        private static ISequence HandleInstantiation(ISequence sequence, SequenceRunData runData = null)
        {
            if (sequence is ScriptableObject scriptableObject)
            {
                ScriptableObject scriptableInstance = Instantiate(scriptableObject);
                runData.onGenerated?.Invoke(scriptableInstance);
                return scriptableInstance as ISequence;
            }

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

        public static List<SequenceStructure> GetSequenceStructures()
        {
            return SequenceStructure.BuildSequenceStructures(GetAll());
        }



    }

}

