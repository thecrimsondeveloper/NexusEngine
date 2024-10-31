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
            Nexus.Log("Registering Sequence: " + sequence.name);
            if (sequence.guid == Guid.Empty)
            {
                sequence.guid = Guid.NewGuid();
                Nexus.Log("Generated new GUID for sequence: " + sequence.name);
            }

            if (!runningSequences.ContainsKey(sequence.guid))
            {
                Nexus.Log("Adding New Sequence: " + sequence.name);
                SequenceEvents events = new SequenceEvents(sequence);
                runningSequences.Add(sequence.guid, sequence);
                sequenceEvents.Add(sequence.guid, events);

                if (runData.onInitialize != null) events.RegisterEvent(runData.onInitialize, SequenceEventType.OnInitialize);
                else Nexus.Log("onInitialize event is null for sequence: " + sequence.name);

                if (runData.onBegin != null) events.RegisterEvent(runData.onBegin, SequenceEventType.OnBegin);
                else Nexus.Log("onBegin event is null for sequence: " + sequence.name);

                if (runData.onFinished != null) events.RegisterEvent(runData.onFinished, SequenceEventType.OnFinished);
                else Nexus.Log("onFinished event is null for sequence: " + sequence.name);

                if (runData.onUnload != null) events.RegisterEvent(runData.onUnload, SequenceEventType.OnUnload);
                else Nexus.Log("onUnload event is null for sequence: " + sequence.name);

                if (runData.onUnloaded != null) events.RegisterEvent(runData.onUnloaded, SequenceEventType.OnUnloaded);
                else Nexus.Log("onUnloaded event is null for sequence: " + sequence.name);

                return events;
            }
            else if (sequenceEvents.TryGetValue(sequence.guid, out var sequenceEvent))
            {
                Nexus.Log("Updating Existing Sequence: " + sequence.name);

                if (runData.onInitialize != null) sequenceEvent.RegisterEvent(runData.onInitialize, SequenceEventType.OnInitialize);
                else Nexus.Log("onInitialize event is null for sequence: " + sequence.name);

                if (runData.onBegin != null) sequenceEvent.RegisterEvent(runData.onBegin, SequenceEventType.OnBegin);
                else Nexus.Log("onBegin event is null for sequence: " + sequence.name);

                if (runData.onFinished != null) sequenceEvent.RegisterEvent(runData.onFinished, SequenceEventType.OnFinished);
                else Nexus.Log("onFinished event is null for sequence: " + sequence.name);

                if (runData.onUnload != null) sequenceEvent.RegisterEvent(runData.onUnload, SequenceEventType.OnUnload);
                else Nexus.Log("onUnload event is null for sequence: " + sequence.name);

                if (runData.onUnloaded != null) sequenceEvent.RegisterEvent(runData.onUnloaded, SequenceEventType.OnUnloaded);
                else Nexus.Log("onUnloaded event is null for sequence: " + sequence.name);

                return sequenceEvent;
            }

            Nexus.LogError("Failed to register or update sequence: " + sequence.name);
            return null;
        }


        private static void UnregisterSequence(ISequence sequence)
        {
            if (runningSequences.ContainsKey(sequence.guid))
            {
                Nexus.Log("Removing " + sequence.name + " from the sequence list");
                runningSequences.Remove(sequence.guid);
            }
            else
            {
                Nexus.Log(sequence.name + " not found when trying to UnRegister from sequence list");
            }

            if (sequenceEvents.ContainsKey(sequence.guid))
            {
                Nexus.Log("Removing " + sequence.name + " from the events list");
                sequenceEvents.Remove(sequence.guid);
            }
            else
            {
                Nexus.Log(sequence.name + " not found when trying to UnRegister from events list");
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

            Nexus.Log("PreRegister Sequence: " + sequence.name);

            SequenceEvents events = RegisterSequence(sequence, runData);

            Nexus.Log("Sequence Ready To Run "+ sequence.name);

            // Start the sequence and store the running task
            UniTask runningTask = RunSequence(sequence, events, runData);
            sequenceObject.SetTask(runningTask);
            sequenceObject.events = events;

            return sequenceObject;
        }

       public static void HandleMonoBehaviour(MonoBehaviour sequence, SequenceRunData runData)
        {
            if (sequence == null || runData == null)
            {
                Debug.LogError("Sequence or runData is null.");
                return;
            }

            if (runData.parent != null)
            {
                sequence.transform.SetParent(runData.parent);
            }
            else if (runData.superSequence != null)
            {
                sequence.transform.SetParent(runData.superSequence.GetTransform());
            }


            if (runData.spawnPosition.HasValue)
            {
                if (runData.spawnSpace == Space.Self)
                {
                    // Set local position if specified
                    sequence.transform.localPosition = runData.spawnPosition.Value;
                }
                else
                {
                    // Otherwise, set world position
                    sequence.transform.position = runData.spawnPosition.Value;
                }
            }


            if (runData.spawnRotation.HasValue)
            {
                // If no rotation was specified, use the current rotation
                if (runData.spawnSpace == Space.Self)
                {
                    // Set local rotation if specified
                    sequence.transform.localRotation = runData.spawnRotation.Value;
                }
                else
                {
                    // Otherwise, set world rotation
                    sequence.transform.rotation = runData.spawnRotation.Value;
                }
            }
        }

        static async UniTask RunSequence(ISequence sequence, SequenceEvents events, SequenceRunData runData)
        {
            Nexus.Log("Async Run "+ sequence.name);
            // Cleanup for replacement sequence
            bool hasReplacement = runData.replace != null;
            if (hasReplacement && IsRunning(runData.replace))
            {
                await Stop(runData.replace);
            }
            await UniTask.NextFrame();

            // Setup sequence hierarchy
            if (runData.superSequence != null)
            {
                sequence.superSequence = runData.superSequence;
            }

            // Handle data assignment
            if (runData.sequenceData == null)
            {
                // If no sequenceData is provided, use currentData of the sequence
                if (sequence.currentData != null)
                {
                    Nexus.Log($"Using existing data for sequence: {sequence.name}");
                    runData.sequenceData = sequence.currentData;
                }
                else
                {
                    // If sequence also has no currentData, create a new instance
                    runData.sequenceData = default;
                }
            }

            // Assign the data to the sequence
            sequence.currentData = runData.sequenceData;

            sequence.phase = Phase.Initialization;

            // Initialize sequence with the provided data
            await sequence.InitializeSequence(runData.sequenceData);
            if (events != null) events.InvokeEvent(SequenceEventType.OnInitialize, sequence);

            // Proceed to Begin phase
            sequence.phase = Phase.Begin;
            sequence.OnBeginSequence();
            if (events != null) events.InvokeEvent(SequenceEventType.OnBegin, sequence);

            // Set sequence to Run phase
            sequence.phase = Phase.Run;
            Nexus.Log("Async Run Function Complete "+ sequence.name);
        }


        public static async UniTask Stop(ISequence sequence)
        {
            if(sequence.phase == Phase.Begin)
            {
                await UniTask.NextFrame();
            }
            sequence.phase = Phase.Unloading;

            if (!IsRunning(sequence))
            {
                Nexus.LogWarning("Stop-Sequence: "+sequence.GetType()+" is not running with GUID: "+ sequence.guid);
            }

            if(sequenceEvents.TryGetValue(sequence.guid, out SequenceEvents evts) == false)
            {
                Nexus.Log(sequence.name + " was not foundin the events Dictionary.");
            }


            if (evts != null)
            {
                evts.InvokeEvent(SequenceEventType.OnUnload, sequence); // OnUnloaded
            }
            await sequence.UnloadSequence();

            if (evts != null)
            {
                evts.InvokeEvent(SequenceEventType.OnUnloaded, sequence); // OnUnloaded
            }
            
            UnregisterSequence(sequence);
            sequence.OnUnloadedSequence();

            sequence.phase = Phase.Idle;
        }

        public static async UniTask Finish(ISequence sequence)
        {
            if(sequence.phase == Phase.Begin)
            {
                await UniTask.NextFrame();
            }
            
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
            Nexus.Log("Sequence Finished: "  + sequence.name);
            if (sequenceEvents.TryGetValue(sequence.guid, out var events))
            {
                Nexus.Log("Invoking Sequence Events for OnFinished");
                events.InvokeEvent(SequenceEventType.OnFinished, sequence); // OnFinished
            }
            else
            {
                Nexus.Log("FINISHED EVENTS NOT FOUND WHEN FINISHING");
            }
            sequence.phase = Phase.Finished;
            sequence.OnFinishedSequence();
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
                Nexus.Log("Handling GameObject: " + monoBehaviour.gameObject.name);
                bool isPrefab = monoBehaviour.gameObject.scene.name == null;

                //if there is no spawnPosition and this is a prefab, set the spawn position to the current local position
                if(runData.spawnPosition.HasValue == false && isPrefab)
                {
                    runData.spawnPosition = monoBehaviour.transform.localPosition;
                }
                if(runData.spawnRotation.HasValue == false && isPrefab)
                {
                    runData.spawnRotation = monoBehaviour.transform.rotation;
                }

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
