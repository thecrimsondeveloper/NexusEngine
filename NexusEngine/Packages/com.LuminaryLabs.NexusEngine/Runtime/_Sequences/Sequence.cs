using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
    public class Sequence : MonoBehaviour
    {
        [Title("Sequence View")]
        [ShowInInspector] List<SequenceStructure> sequenceStructures => SequenceStructure.BuildSequenceStructures(GetAll());
        [Title("Sequence Data")]
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
            if (runData.superSequence != null)
            {
                sequence.superSequence = runData.superSequence;
            }

            await sequence.InitializeSequence(runData.sequenceData);
            if (sequence is MonoSequence monoSequence)
            {
                if (runData.parent != null)
                {
                    monoSequence.transform.SetParent(runData.parent);
                }


                if (runData.useLocalPosition)
                    monoSequence.transform.localPosition = runData.spawnPosition;
                else monoSequence.transform.position = runData.spawnPosition;

                if (runData.useLocalRotation)
                    monoSequence.transform.localRotation = runData.spawnRotation;
                else monoSequence.transform.rotation = runData.spawnRotation;
            }

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

        public static List<SequenceStructure> GetSequenceStructures()
        {
            return SequenceStructure.BuildSequenceStructures(GetAll());
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
        /// <summary>
        /// The sequence that is running this sequence.
        /// If both the superSequence and the given sequence are MonoBehaviours, the given sequence will be parented to the superSequence.
        /// </summary>
        public ISequence superSequence { get; set; }

        /// <summary>
        /// This specificies a sequences that will be stopped before the new sequence is started.
        /// </summary>
        /// /// ///
        public ISequence replace { get; set; }

        /// <summary>
        /// The data that is passed to the sequence when it is spawned.
        /// This can be anything that needs to be passed into the sequence for it to run. 
        /// May be a second data class.
        /// </summary>
        public object sequenceData { get; set; }

        /// <summary>
        /// The position of the sequence when it is spawned. Uses world space position by default.
        /// </summary>
        /// /// ///
        public Vector3 spawnPosition { get; set; }

        /// <summary>
        /// The rotation of the sequence when it is spawned. Uses world space rotation by default.
        /// </summary>
        /// /// ///
        public Quaternion spawnRotation { get; set; }

        /// <summary>
        /// Sets the position locally after it set's the parent.
        /// </summary>
        /// /// ///
        public bool useLocalPosition { get; set; } = true;

        /// <summary>
        /// Sets the rotation locally after it set's the parent.
        /// </summary>
        /// /// ///
        public bool useLocalRotation { get; set; } = true;

        /// <summary>
        /// Sets the parent of the sequence if it is associated with a MonoBehaviour. Overwrites the superSequence parent.
        /// </summary>
        /// /// ///
        public Transform parent { get; set; }

        /// <summary>
        /// If the sequence was generated by the Sequence class.
        /// </summary>
        /// /// ///
        public bool wasGenerated { get; set; } = false;

        /// <summary>
        /// EVENT: Called when any Sequence is initialized by the Sequence class.\
        /// This is called before the sequence is started. Use this to set any dependancies.
        /// </summary>
        /// /// ///
        public UnityAction<ISequence> onInitialize { get; set; }

        /// <summary>
        /// EVENT: Called when the sequence is started by the Sequence class.
        /// Called directly after the sequence is initialized.
        /// This is where the sequence should start running any sub sequences and logic.
        /// </summary>
        /// /// ///
        public UnityAction<ISequence> onBegin { get; set; }

        /// <summary>
        /// EVENT: Called when the sequence is finished by the Sequence class.
        /// This should be reserved for the completion of the sequence. The sequence is still running when this is called.
        /// </summary>
        /// /// ///
        public UnityAction<ISequence> onFinished { get; set; }

        /// <summary>
        /// EVENT: Called when a sequence is stopped by the Sequence class.
        /// </summary>
        /// /// ///
        public UnityAction<ISequence> onUnloaded { get; set; }

        /// <summary>
        /// EVENT: Called when a MonoBehaviour is generated by the Sequence class.Gets called when a prefab sequence is ran.
        /// </summary>
        /// /// ///
        public SequenceAction<MonoBehaviour> onGenerated { get; set; }

        public override string ToString() => $"SequenceRunData: {sequenceData}\nSuperSequence: {superSequence}\nReplace: {replace}\nSpawnPosition: {spawnPosition}\nSpawnRotation: {spawnRotation}\nParent: {parent}";
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

    [System.Serializable]
    public class SequenceStructure
    {
        public ISequence sequence;
        public UnityEngine.Object sequenceObject;
        public string name;
        public string parent;

        public List<SequenceStructure> subSequences = new List<SequenceStructure>();

        // public DebugSequence[] subSequences;
        public SequenceStructure(ISequence sequence)
        {
            this.sequence = sequence;
            this.sequenceObject = sequence is UnityEngine.Object ? sequence as UnityEngine.Object : null;
            this.name = sequence.GetType().Name;
            this.parent = sequence.superSequence?.GetType().Name;
        }

        public static List<SequenceStructure> BuildSequenceStructures(List<ISequence> sequences)
        {
            Dictionary<Guid, SequenceStructure> sequenceMap = new Dictionary<Guid, SequenceStructure>();
            List<SequenceStructure> debugSequences = new List<SequenceStructure>();

            foreach (var sequence in sequences)
            {
                SequenceStructure debugSequence = new SequenceStructure(sequence);
                sequenceMap.Add(sequence.guid, debugSequence);
            }

            foreach (var sequence in sequences)
            {
                if (sequence.superSequence != null && sequenceMap.TryGetValue(sequence.superSequence.guid, out var parent))
                {
                    parent.subSequences.Add(sequenceMap[sequence.guid]);
                }
                else
                {
                    debugSequences.Add(sequenceMap[sequence.guid]);
                }
            }
            return debugSequences;
        }

    }
}

