using System;
using System.Collections.Generic;
using Toolkit.Sequences;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Toolkit.NexusEngine
{
    public interface INexusSequence : ISequence
    {
        public Guid guid { get; set; }
        List<INexusSequence> sequences { get; set; }
        new public virtual void Load()
        {
            Debug.Log("NexusSequence Load");
            (this as ISequence).Load();
        }
    }
}
