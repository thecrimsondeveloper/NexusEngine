using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PositionHandling
{
    internal abstract class PositionSettings<T> : PositionSettings where T : PositionSetter
    {
        public override void AddPositionSetter(GameObject target)
        {
            T setter = target.AddComponent<T>();
        }
    }

    public abstract class PositionSettings : ScriptableObject
    {
        public PositionType positionType = PositionType.LeftHand;
        public bool snapToPose = false;
        public Pose offset = Pose.identity;
        public float positionalLerpSpeed = 1;
        public float rotationalLerpSpeed = 1;
        public float distanceThreshold = 1;
        public AnimationCurve distanceCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public abstract void AddPositionSetter(GameObject target);
    }
    public enum PositionType
    {
        Identity,
        Playspace,
        LeftHand,
        RightHand,
        Chest,
        Gaze,
    }
}
