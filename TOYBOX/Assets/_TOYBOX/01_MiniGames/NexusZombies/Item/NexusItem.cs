using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public abstract class NexusItem : MonoBehaviour
    {
        [SerializeField] protected Animation onUseAnimation;
        bool isPickedUp = false;

        public bool IsPickedUp => isPickedUp;

        private void Update()
        {
            bool leftMouse = Input.GetMouseButtonDown(0);
            if (leftMouse)
            {
                Debug.Log("Left Mouse Clicked!");
                Use(null);
            }
        }

        public virtual void Use(NexusPlayer player = null)
        {


            if (onUseAnimation)
            {
                onUseAnimation.Play();
            }

            OnUse(player);

        }

        protected abstract void OnUse(NexusPlayer player = null);
        protected virtual void OnUseWithoutPlayer() { }

        public virtual void Pickup(NexusPlayer player)
        {
            isPickedUp = true;
            OnPickup(player);
        }

        protected virtual void OnPickup(NexusPlayer player) { }

        public virtual void Drop(NexusPlayer player)
        {
            isPickedUp = false;
            OnDrop(player);
        }

        protected virtual void OnDrop(NexusPlayer player) { }


    }
}
