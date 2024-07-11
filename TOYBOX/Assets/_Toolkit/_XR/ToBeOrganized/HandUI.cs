using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.DependencyResolution;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace MediTrainer
{


    public class HandUI : MonoBehaviour
    {
        const string LEFT_HAND_TAG = "LeftHand";
        const string RIGHT_HAND_TAG = "RightHand";

        internal enum HandType { Left, Right }

        internal enum DisplayState { Hidden, Visible }
        internal enum DisplayMode { Static, Reactive }
        [SerializeField, FoldoutGroup("Hand UI")] DependencyResolver sceneDependencies;
        [SerializeField, FoldoutGroup("Hand UI")] DisplayMode displayMode = DisplayMode.Static;
        [SerializeField, FoldoutGroup("Hand UI")] DisplayState startDisplayState = DisplayState.Hidden;
        [SerializeField, FoldoutGroup("Hand UI")] HandType handType = HandType.Left;
        [SerializeField, FoldoutGroup("Hand UI"), HideInEditorMode] DisplayState displayState = DisplayState.Hidden;
        [SerializeField, FoldoutGroup("Hand UI")] Transform contentParent;
        [SerializeField, FoldoutGroup("Hand UI")] HandDisplaySettings displaySettings;

        Transform handTransform;
        Camera head;
        private async void Awake()
        {
            await sceneDependencies.Resolve();
            string tag = handType == HandType.Left ? LEFT_HAND_TAG : RIGHT_HAND_TAG;
            GameObject handGameObject = GameObject.FindGameObjectWithTag(tag);
            if (handGameObject == null)
            {
                Debug.LogError($"No object with tag {tag} found.");
                return;
            }
            handTransform = handGameObject.transform;
            SetDisplayState(startDisplayState, force: true);
        }
        private void Start()
        {
        }

        void Update()
        {
            if (head == null)
            {
                head = Camera.main;
                return;
            }

            Vector3 handForward = handTransform.forward;
            Vector3 cameraForward = head.transform.forward;

            if (displayState == DisplayState.Visible)
            {
                OnHandDisplay();
            }

            if (displayMode == DisplayMode.Static)
            {
                return;
            }

            bool shouldDisplay = displaySettings.ShouldDisplay(handTransform, head.transform);
            if (shouldDisplay)
            {
                SetDisplayState(DisplayState.Visible);
            }
            else
            {
                SetDisplayState(DisplayState.Hidden);
            }
        }

        void SetDisplayState(DisplayState newState, bool force = false)
        {
            if (newState == displayState && force == false)
            {
                return;
            }
            displayState = newState;
            switch (newState)
            {
                case DisplayState.Hidden:
                    break;
                case DisplayState.Visible:
                    break;
            }
        }

        void OnHandDisplay()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, handTransform.rotation, Time.deltaTime * 10);
            transform.position = Vector3.Lerp(transform.position, handTransform.position, Time.deltaTime * 30);
        }
    }

    [System.Serializable]
    public class HandDisplaySettings
    {
        [SerializeField, Range(0, 1)] float dotThresholdForDisplay = 0.5f;

        public float DotThresholdForDisplay => dotThresholdForDisplay;
        public bool ShouldDisplay(Transform hand, Transform head)
        {
            Vector3 handForward = hand.forward;
            Vector3 headForward = head.forward;

            float dot = Vector3.Dot(handForward, headForward);
            float negativeRatio = Mathf.InverseLerp(1, -1, dot);

            // Debug.DrawRay(head.position, headForward * 5, Color.blue);
            // Debug.DrawRay(hand.position, handForward * 5, Color.red);
            // Debug.Log($"Dot: {dot}");
            // Debug.Log($"Ratio: {negativeRatio}");

            //if the hand is facing the opposite direction of the head, return true
            return negativeRatio >= dotThresholdForDisplay;
        }
    }



}