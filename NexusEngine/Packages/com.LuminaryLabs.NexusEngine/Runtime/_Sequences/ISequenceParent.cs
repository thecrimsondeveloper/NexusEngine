using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LuminaryLabs.Sequences
{
    public interface ISequenceParent : ISequence
    {
        public ISequence[] subSequences { get; set; }

        public void FindSubSequences()
        {
            if (this is MonoBehaviour monoBehaviour)
            {
                subSequences = monoBehaviour.GetComponentsInChildren<ISequence>().Where(s => s != this).ToArray();
            }
        }
    }

}

