using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusEntityDependencyDefinition<T> : NexusBlockDependencyDefinition where T : NexusEntity
    {
        public T entityPrefab;
    }
}
