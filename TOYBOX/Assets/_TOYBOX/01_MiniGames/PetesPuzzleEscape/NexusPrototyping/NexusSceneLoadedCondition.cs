using System.Collections;
using System.Collections.Generic;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{

    [CreateAssetMenu(fileName = "Nexus Scene Loaded", menuName = "Nexus/Conditions/OVR Scene Loaded")]
    public class NexusSceneLoadedCondition : NexusCondition
    {
        public override bool IsConditionMet(MonoBehaviour mono = null)
        {
            return XRPlayspace.IsSceneLoaded;
        }
    }
}
