using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class PPERayTeleport : MonoBehaviour
    {
        [SerializeField] private RayInteractor rightRay;
        [SerializeField] private RayInteractor leftRay;

        private void Start()
        {
            // Subscribe to the WhenUnselected events
            rightRay.WhenInteractableUnselected.Action += OnRightRayUnselected;
            leftRay.WhenInteractableUnselected.Action += OnLeftRayUnselected;
        }

        private void OnRightRayUnselected(RayInteractable rayInteractable)
        {
            // Handle right ray unselected event
            Debug.Log($"Right ray unselected: {rayInteractable.name}");
            TeleportPlayerToPosition(rightRay.End);
        }

        private void OnLeftRayUnselected(RayInteractable rayInteractable)
        {
            // Handle left ray unselected event
            Debug.Log($"Left ray unselected: {rayInteractable.name}");
            // Teleport the player to the endpoint of the left ray
            TeleportPlayerToPosition(leftRay.End);
        }

        public void TeleportPlayerToPosition(Vector3 position)
        {
            XRPlayer.SetPosition(position);
        }

        private void OnDestroy()
        {
            // Unsubscribe from the WhenUnselected events
            rightRay.WhenInteractableUnselected.Action -= OnRightRayUnselected;
            leftRay.WhenInteractableUnselected.Action -= OnLeftRayUnselected;
        }
    }
}