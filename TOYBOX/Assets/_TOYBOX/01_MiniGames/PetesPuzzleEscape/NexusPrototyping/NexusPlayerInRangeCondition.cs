using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.XR;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "InRangeCondition", menuName = "Nexus/Conditions/InRangeCondition")]
    public class InRangeCondition : NexusCondition
    {
        public enum RangeType
        {
            Sphere,
            Box
        }

        public enum ComparisonTarget
        {
            Player,
            Tag
        }
        public RangeType rangeType = RangeType.Sphere;
        public ComparisonTarget comparisonTarget = ComparisonTarget.Player;

        [ShowIf(nameof(rangeType), RangeType.Box)]
        public float width = 1.0f;

        [ShowIf(nameof(rangeType), RangeType.Sphere)]
        public float radius = 1.0f;

        [ShowIf(nameof(comparisonTarget), ComparisonTarget.Tag)]
        public string tag = "Player";

        public override bool IsConditionMet(MonoBehaviour sourceEntity = null)
        {
            if (sourceEntity == null)
            {
                return false;
            }
            switch (rangeType)
            {
                case RangeType.Sphere:
                    return InSphereRange(sourceEntity);
                case RangeType.Box:
                    return InBoxRange(sourceEntity);
            }
            return false;
        }

        bool InSphereRange(MonoBehaviour sourceEntity)
        {
            if (comparisonTarget == ComparisonTarget.Player)
            {
                return Vector3.Distance(XRPlayer.HeadPose.position, sourceEntity.transform.position) <= radius;
            }
            else if (comparisonTarget == ComparisonTarget.Tag)
            {
                return Vector3.Distance(GameObject.FindWithTag(tag).transform.position, sourceEntity.transform.position) <= radius;
            }

            return false;
        }

        bool InBoxRange(MonoBehaviour sourceEntity)
        {
            Bounds bounds = new Bounds(sourceEntity.transform.position, Vector3.one * width);
            if (comparisonTarget == ComparisonTarget.Player)
            {
                return bounds.Contains(XRPlayer.HeadPose.position);
            }
            else if (comparisonTarget == ComparisonTarget.Tag)
            {
                return bounds.Contains(GameObject.FindWithTag(tag).transform.position);
            }

            return false;
        }
    }
}
