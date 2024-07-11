using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{

    [CreateAssetMenu(fileName = "NexusEntityDefinition", menuName = "Nexus/Entity Definition")]
    public class NexusEntityDefinition : ScriptableObject
    {
        public NexusEntity entityPrefab;
        public NexusConnection[] connectionData;

    }
}
