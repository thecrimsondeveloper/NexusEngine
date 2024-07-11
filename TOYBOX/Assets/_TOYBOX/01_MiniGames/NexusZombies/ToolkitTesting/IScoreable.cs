using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Entity
{
    public interface IScoreable
    {
        public UnityEvent<float> OnScoreEvent { get; set; }


    }
}
