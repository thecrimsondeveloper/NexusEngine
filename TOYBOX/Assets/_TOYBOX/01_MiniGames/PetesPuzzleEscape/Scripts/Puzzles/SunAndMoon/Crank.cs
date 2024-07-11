using Oculus.Interaction;
using Toolkit.XR;
using UnityEngine;
using UnityEngine.Events;


namespace ToyBox.Minigames.EscapeRoom
{
    public class Crank : MonoBehaviour
    {
        [SerializeField] private PointableUnityEventWrapper events;

        public OneGrabRotateTransformer oneGrabRotateTransformer;

        public bool isRotating = false;

        private void Start()
        {
            events.WhenSelect.AddListener(OnSelected);
            events.WhenUnselect.AddListener(StopRotate);
        }

        private void OnSelected(PointerEvent args)
        {
            if (isRotating) return;

            isRotating = true;
        }

        public void StopRotate(PointerEvent args)
        {
            isRotating = false;
        }
    }
}
