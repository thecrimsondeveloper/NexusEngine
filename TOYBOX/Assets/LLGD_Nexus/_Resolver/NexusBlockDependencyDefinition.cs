using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusBlockDependencyDefinition : ScriptableObject
    {
        string guid = "";
        public NexusBlockDependencyDefinition[] subBlocks;

        public NexusComponentDependencyDefinition[] components;
    }
}
