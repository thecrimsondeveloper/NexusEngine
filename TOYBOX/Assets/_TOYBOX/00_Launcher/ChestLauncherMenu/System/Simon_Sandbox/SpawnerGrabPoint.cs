using Oculus.Interaction;
using ToyBox.Launcher;
using UnityEngine;

namespace ToyBox
{
    public class SpawnerGrabPoint : MonoBehaviour
    {
        [SerializeField] InteractableUnityEventWrapper interactableEvents;
        [SerializeField] ToySpawner spawner;
        [SerializeField] Vector3 snapPosition;

        public Vector3 SnapPosition { get => snapPosition; set => snapPosition = value; }

        Vector3 originalPosition = Vector3.zero;
        private void Start()
        {
            interactableEvents.WhenSelect.AddListener(OnSelect);
            spawner.AnimationComplete.AddListener(OnAnimationComplete);
        }

        private void OnSelect()
        {
            Debug.Log("Opening Chest");
            spawner.SpawnToy();
        }

        private void OnAnimationComplete()
        {
            originalPosition = transform.position;
        }

        private void Update()
        {
            if (originalPosition != Vector3.zero && transform.position != originalPosition)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * 5);
            }
        }
    }
}
