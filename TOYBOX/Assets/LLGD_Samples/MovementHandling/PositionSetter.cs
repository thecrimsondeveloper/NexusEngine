using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PositionHandling
{
    public abstract class PositionSetter : MonoBehaviour
    {


        [Title("Settings")]

        [Tooltip("The number of times at which the transform tion is updated per second. 0 means it is updated every frame.")]
        [SerializeField, FoldoutGroup("Position Setter"), Range(0, 90)] float refreshRate = 10;
        [SerializeField, FoldoutGroup("Position Setter")] private PositionSettings currentSettings;

        [Title("Debug")]
        [SerializeField, FoldoutGroup("Position Setter"), ReadOnly] Pose targetPose;

        public PositionType Type => currentSettings == null ? PositionType.Identity : currentSettings.positionType;

        public PositionSettings Settings
        {
            get
            {
                if (currentSettings == null)
                {
                    currentSettings = ScriptableObject.CreateInstance<PositionSettings>();
                }
                return currentSettings;
            }
            set
            {
                currentSettings = value;
                UpdateSettings(currentSettings);
            }
        }


        float timer = 0;

        private void Update()
        {
            if (refreshRate <= 0)
            {
                Refresh();
            }
            else
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Refresh();
                    timer = 1 / refreshRate;
                }
            }
        }

        private void Refresh()
        {
            targetPose.position = GetPosition();

            if (currentSettings.snapToPose)
            {
                transform.position = targetPose.position;
                return;
            }

            transform.position = Vector3.Lerp(transform.position, targetPose.position, Time.deltaTime * currentSettings.positionalLerpSpeed);
        }



        public bool IsCorrectType(PositionType positionType)
        {
            if (currentSettings == null)
            {
                return false;
            }
            return currentSettings.positionType == positionType;
        }

        protected abstract Vector3 GetPosition();
        protected abstract Quaternion GetRotation();

        protected float GetSpeedFromCurve()
        {
            if (currentSettings == null) return 0;
            float distance = Vector3.Distance(transform.position, targetPose.position);
            float distanceRatio = Mathf.Clamp01(distance / currentSettings.distanceThreshold);
            float curveValue = currentSettings.distanceCurve.Evaluate(distanceRatio);
            return curveValue;
        }

        public void UpdateSettings(PositionSettings settings)
        {
            OnUpdateSettings(settings);
            this.currentSettings = settings;
        }

        protected abstract void OnUpdateSettings(PositionSettings settings);
    }
}
