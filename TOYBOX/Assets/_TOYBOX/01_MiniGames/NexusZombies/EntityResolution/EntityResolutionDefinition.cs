using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.DependencyResolution
{
    [CreateAssetMenu(fileName = "EnittyResolutionDefinition", menuName = "Toolkit/DependencyResolution/EntityResolutionDefinition")]
    public class EntityResolutionDefinition : ScriptableObject
    {
        public string Name;
    }
}
