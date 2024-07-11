using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomBuildingUI : MonoBehaviour
    {
        public PointableUnityEventWrapper interactionEvents;
        public Transform contentParent;
        private void Start()
        {
            interactionEvents.WhenHover.AddListener(WhenHover);
            interactionEvents.WhenUnhover.AddListener(WhenUnhover);
        }
        void WhenHover(PointerEvent ptr)
        {
            contentParent.gameObject.SetActive(true);
        }
        void WhenUnhover(PointerEvent ptr)
        {
            contentParent.gameObject.SetActive(false);
        }
    }
}
