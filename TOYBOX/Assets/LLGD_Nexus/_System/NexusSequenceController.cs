using System.Collections;
using System.Collections.Generic;
using Toolkit.Sequences;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusSequenceController : SequenceController
    {
        protected override void BeforeRun(ISequence sequence)
        {
            if (sequence is NexusSequenceDefinition)
            {
                NexusSequenceDefinition nexusSequence = sequence as NexusSequenceDefinition;
                nexusSequence.OnSequenceStart();
            }
        }
    }
}
