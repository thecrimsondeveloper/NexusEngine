using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class NexusHand : NexusPart
    {
        // public enum MoveType
        // {
        //     Teleport,
        //     Smooth
        // }

        // public MoveType moveType = MoveType.Smooth;
        public Handedness handedness;
        [SerializeField] float moveSpeed = 10f;
        public UnityEvent<NexusItem> WhenHoverOnItem = new UnityEvent<NexusItem>();
        public UnityEvent<NexusItem> WhenUnhoverOnItem = new UnityEvent<NexusItem>();
        // [SerializeField] Rigidbody rigidbody;

        public NexusItem hoveredItem;
        Transform target;

        private void Start()
        {


        }
        private void Update()
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        protected override void OnPartInitialized(NexusCharacter character)
        {
            base.OnPartInitialized(character);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out NexusItem item))
            {
                Debug.Log("Item found! " + item.name);
                hoveredItem = item;
                WhenHoverOnItem?.Invoke(item);
            }
            else if (other.transform.parent != null && other.transform.parent.TryGetComponent(out NexusItem itemInParent))
            {
                Debug.Log("Item found! " + itemInParent.name);
                hoveredItem = itemInParent;
                WhenHoverOnItem?.Invoke(itemInParent);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (hoveredItem == null) return;
            if (other.gameObject == hoveredItem.gameObject)
            {
                Debug.Log("Item lost! " + hoveredItem.name);
                WhenUnhoverOnItem?.Invoke(hoveredItem);
                hoveredItem = null;
            }
            else if (other.transform.parent != null && other.transform.parent.gameObject == hoveredItem.gameObject)
            {
                Debug.Log("Item lost! " + hoveredItem.name);
                WhenUnhoverOnItem?.Invoke(hoveredItem);
                hoveredItem = null;
            }
        }
    }
}
