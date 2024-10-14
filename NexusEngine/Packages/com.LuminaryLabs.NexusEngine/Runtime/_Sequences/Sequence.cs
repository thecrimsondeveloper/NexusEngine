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
                SequenceEvents events = new SequenceEvents(sequence);
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


        public static SequenceRunResult Run<T>(SequenceRunData runData = null) where T : UnityEngine.MonoBehaviour, ISequence
        {
            GameObject gameObject = new GameObject();
            T monoSequence = gameObject.AddComponent<T>();
            gameObject.name = monoSequence.GetType().ToString();
            return Run(monoSequence, runData); 
        }

        public static SequenceRunResult Run(ISequence sequence, SequenceRunData runData = null)
        {

            SequenceRunResult sequenceObject = new SequenceRunResult();
            sequenceObject.sequence = sequence;

            string name = sequence.GetType().Name;
            bool hasRunData = runData != null;
            bool hasSequenceData = hasRunData ? runData.sequenceData != null : false;
            Nexus.Log("Running:" + name);

            if (IsRunning(sequence))
            {
                if(sequence is MonoBehaviour mono)
                {
                    name = mono.name;
                }
                Nexus.Log(name + " is already running, will instead subscribe the events again and return the current run result");
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
            else if (runData.superSequence != null)
            {
                sequence.transform.SetParent(runData.superSequence.GetTransform());
            }

            if (runData.spawnPosition.HasValue == false)
            {
                runData.spawnPosition = sequence.transform.position;
            }
            else if (runData.useLocalPosition)
            {
                sequence.transform.localPosition = runData.spawnPosition.Value;
            }
            else
            {
                sequence.transform.position = runData.spawnPosition.Value;
            }


            if (runData.spawnRotation.HasValue == false)
            {
                runData.spawnRotation = sequence.transform.rotation;
            }
            else if (runData.useLocalRotation)
            {
                sequence.transform.localRotation = runData.spawnRotation.Value;
            }
            else
            {
                sequence.transform.rotation = runData.spawnRotation.Value;
            }

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

            //setup sequence heirarchy
            if (runData.superSequence != null)
            {
                sequence.superSequence = runData.superSequence;
            }

            //set data
            //if data is passed in, set it
            if (runData.sequenceData != null)
            {
                sequence.currentData = runData.sequenceData;
            }
            else //if no data is passed in, set the current data to the sequence's data
            {
                runData.sequenceData = sequence.currentData;
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
                Nexus.LogWarning("Stop-Sequence: "+sequence.GetType()+" is not running with GUID: "+ sequence.guid);
            }

            await sequence.UnloadSequence();
            if (sequenceEvents.TryGetValue(sequence.guid, out var events))
                events.InvokeEvent(SequenceEventType.OnUnloaded, sequence); // OnUnloaded
            UnregisterSequence(sequence);

            sequence.OnUnloadSequence();
        }

        public static async UniTask Finish(ISequence sequence)
        {
            if (!IsRunning(sequence))
            {
                string mustRunError = sequence.GetType() + " is not currently running. You must Run a Sequence in order for it to Finish";

                if(sequence is UnityEngine.Object obj)
                Nexus.LogError(obj.name + ": " + mustRunError, obj);
                else
                Nexus.LogError(mustRunError);

                return;
            }


            await sequence.FinishSequence();
            Debug.Log("Sequence Finished: " + sequence.GetType());
            if (sequenceEvents.TryGetValue(sequence.guid, out var events))
            {
                Debug.Log("Invoking Sequence Events for OnFinished");
                events.InvokeEvent(SequenceEventType.OnFinished, sequence); // OnFinished
            }

            sequence.OnFinishSequence();
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
            //     ScriptableObject scriptableInstance = Instantiate(scriptableObject);
            //     runData.onGenerated?.Invoke(scriptableInstance);
            //     return scriptableInstance as ISequence;
            // }



            if (sequence is MonoBehaviour monoBehaviour)
            {
                Nexus.Log("(NEXUS) Handling GameObject: " + monoBehaviour.gameObject.name);
                bool isPrefab = monoBehaviour.gameObject.scene.name == null;
                if (isPrefab == false) { return sequence; }
                else
                {
                    Nexus.Log("Is Prefab: " + isPrefab);
                }

                MonoBehaviour monoInstance = Instantiate(monoBehaviour);
                if (runData != null)
                {
                    runData.onGenerated?.Invoke(monoInstance);
                }

                return monoInstance as ISequence;
            }
            return sequence;
        }

        public static List<SequenceStructure> GetSequenceStructures()
        {
            return SequenceStructure.BuildSequenceStructures(GetAll());
        }



    }

}

