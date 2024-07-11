using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "NexusCondition", menuName = "Nexus/Conditions/Base Condition")]
    public class NexusCondition : NexusObject
    {

        [SerializeField] List<NexusCondition> conditions = new List<NexusCondition>();

        [SerializeField] bool useForcedCondition = false;
        [SerializeField, ShowIf(nameof(useForcedCondition))] bool forcedCondition = false;

        public virtual bool IsConditionMet(MonoBehaviour mono = null)
        {
            foreach (var condition in conditions)
            {
                if (condition.IsConditionMet(mono) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public void ToggleForcedCondition(bool conditionValue)
        {
            useForcedCondition = !useForcedCondition;
            forcedCondition = conditionValue;
        }

        public void ToggleForcedCondition()
        {
            useForcedCondition = !useForcedCondition;
        }

        public void SetForcedCondition(bool conditionValue)
        {
            forcedCondition = conditionValue;
        }

    }
}
