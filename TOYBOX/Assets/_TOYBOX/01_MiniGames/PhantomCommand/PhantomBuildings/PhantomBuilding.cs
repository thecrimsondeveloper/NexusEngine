using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomBuilding : MonoBehaviour
    {
        [Title("Building Dependencies")]

        [Sirenix.OdinInspector.Optional]
        public PointableUnityEventWrapper buildingEvents;
        [Title("Building Events")]
        public UnityEvent OnFocus = new UnityEvent();
        public UnityEvent OnUnfocus = new UnityEvent();
        [Title("Building Debug")]
        [HideInEditorMode] public PhantomPlayer owner;


        public void Initialize(PhantomPlayer owner)
        {
            this.owner = owner;
        }

        private void Start()
        {
            if (buildingEvents) buildingEvents.WhenHover.AddListener(WhenHover);
            if (buildingEvents) buildingEvents.WhenUnhover.AddListener(WhenUnhover);
        }

        void WhenHover(PointerEvent eventData)
        {
            Focus();
        }

        void WhenUnhover(PointerEvent eventData)
        {
            Unfocus();
        }




        public void Focus()
        {
            OnFocus.Invoke();
        }

        public void Unfocus()
        {
            OnUnfocus.Invoke();
        }

        public void Damage(float amount)
        {

        }
    }
}
