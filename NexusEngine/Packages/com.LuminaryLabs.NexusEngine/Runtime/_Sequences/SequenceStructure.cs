using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
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
