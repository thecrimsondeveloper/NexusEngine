using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Sequences
{
    [Serializable]

    [CreateAssetMenu(fileName = "SequenceState", menuName = "Toolkit/Sequences/Sequence State")]
    public class SequenceState : ScriptableObject
    {
        public int ID;
        public SequenceState[] SubSequenceIDs;
    }
}
