using Oculus.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Minigames.EscapeRoom
{
    public class Ring : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Dependencies")] PointableUnityEventWrapper events;
        public RingBase currentRingBase;
        public RingBase hoverRingBase;

        public int size;

        void Start()
        {
            events.WhenRelease.AddListener(WhenReleaseRing);
            events.WhenSelect.AddListener(OnGrab);
        }

        private void OnGrab(PointerEvent args)
        {
            if (currentRingBase != null)
            {
                currentRingBase.RemoveRing(this);
                currentRingBase = null;
            }
        }

        private void WhenReleaseRing(PointerEvent args)
        {
            Debug.Log("Checking snap");
            if (hoverRingBase != null)
            {
                currentRingBase = hoverRingBase;
                currentRingBase.PlaceRing(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out hoverRingBase))
            {

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (hoverRingBase && other.gameObject == hoverRingBase.gameObject)
            {
                hoverRingBase = null;
            }
        }
    }
}
