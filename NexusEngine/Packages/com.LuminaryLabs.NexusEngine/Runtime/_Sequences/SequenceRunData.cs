using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
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
        public Vector3? spawnPosition { get; set; }

        /// <summary>
        /// The rotation of the sequence when it is spawned. Uses world space rotation by default.
        /// </summary>
        /// /// ///
        public Quaternion? spawnRotation { get; set; }

        /// <summary>
        /// Chooses the Spawn Space when setting the position of Running Sequences, Defauls to Self.
        /// </summary>
        /// /// ///
        public Space spawnSpace {get; set;} = Space.Self;

        /// <summary>
        /// Sets the parent of the sequence if it is associated with a MonoBehaviour. Overwrites the superSequence parent.
        /// </summary>
        /// /// ///
        public Transform parent { get; set; }

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
        /// EVENT: Called when a sequence has started to be unloaded by the Sequence class.
        /// </summary>
        /// /// ///
        public UnityAction<ISequence> onUnload { get; set; }

        /// <summary>
        /// EVENT: Called when a sequence is stopped by the Sequence class. The Sequence may be null.
        /// </summary>
        /// /// ///
        public UnityAction<ISequence> onUnloaded { get; set; }

        /// <summary>
        /// EVENT: Called when a MonoBehaviour is generated by the Sequence class.Gets called when a prefab sequence is ran.
        /// </summary>
        /// /// ///
        public SequenceAction<Object> onGenerated { get; set; }
        public bool setupInHeirarchy { get; set;} = true;

        public override string ToString() => $"SequenceRunData: {sequenceData}\nSuperSequence: {superSequence}\nReplace: {replace}\nSpawnPosition: {spawnPosition}\nSpawnRotation: {spawnRotation}\nParent: {parent}";
    }
}