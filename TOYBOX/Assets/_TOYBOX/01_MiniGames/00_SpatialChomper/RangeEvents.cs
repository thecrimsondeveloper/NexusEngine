using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public abstract class RangeEvents : MonoBehaviour
    {
        [SerializeField, Range(0, 1), FoldoutGroup("Range Events")] float openAmount = 0.5f;
        [SerializeField, Range(0, 1), FoldoutGroup("Range Events")] float openThreshold = 0.8f; // Threshold value for opening the hand
        [SerializeField, Range(0, 1), FoldoutGroup("Range Events")] float closeThreshold = 0.2f; // Threshold value for closing the hand
        float lastOpenAmount; // Store the previous openAmount
        bool openedThisFrame; // Flag to track if hand was opened this frame
        bool closedThisFrame; // Flag to track if hand was closed this frame
        [FoldoutGroup("Range Events")] public UnityEvent closedEvent; // Event to be called when openAmount transitions from open to closed
        [FoldoutGroup("Range Events")] public UnityEvent openedEvent; // Event to be called when openAmount transitions from closed to open

        protected void SetOpenAmount(float value)
        {
            openAmount = value;
        }

        private void OnValidate()
        {
            //make sure that the open and close thresholds are valid and that open is greater than close
            if (openThreshold < closeThreshold)
            {
                openThreshold = closeThreshold;
            }

            //make sure that the open and close thresholds are valid and that open is greater than close
            if (closeThreshold > openThreshold)
            {
                closeThreshold = openThreshold;
            }

        }

        protected virtual void FixedUpdate()
        {
            // Set class-level variables at the beginning of FixedUpdate loop
            SetData();

            if (openedThisFrame)
            {
                // Invoke the opened event
                if (openedEvent != null)
                    openedEvent.Invoke();

                // Call the factory method OnOpened
                OnOpened();
            }

            // If the hand was closed last frame and is now open
            else if (closedThisFrame)
            {
                // Invoke the closed event
                if (closedEvent != null)
                    closedEvent.Invoke();

                // Call the factory method OnClosed
                OnClosed();
            }

            // Update lastOpenAmount with the current openAmount
            lastOpenAmount = openAmount;
        }

        // Factory method called when the hand is closed
        protected abstract void OnClosed();

        // Factory method called when the hand is opened
        protected abstract void OnOpened();

        // Function to set class-level variables
        void SetData()
        {
            openedThisFrame = openAmount >= openThreshold && lastOpenAmount < openThreshold;
            closedThisFrame = openAmount < closeThreshold && lastOpenAmount >= closeThreshold;
        }
    }
}