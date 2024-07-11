using System.Collections;
using System.Collections.Generic;
using Toolkit.Entity;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class NexusFPSPlayer : NexusPlayer, IAttackable
    {
        public Transform head;
        public Transform leftHandTarget;
        public Transform rightHandTarget;
        public NexusHand leftHand;
        public NexusHand rightHand;

        public NexusInventory leftHandInventory;
        public NexusInventory rightHandInventory;


        protected override void Start()
        {
            base.Start();

            leftHandInventory = ScriptableObject.CreateInstance<NexusInventory>();
            rightHandInventory = ScriptableObject.CreateInstance<NexusInventory>();

            leftHandInventory.maxItems = 1;
            rightHandInventory.maxItems = 1;
            leftHandInventory.itemParent = leftHand.transform;
            rightHandInventory.itemParent = rightHand.transform;

            leftHand.WhenHoverOnItem.AddListener(WhenLeftHandHoverOnItem);
            rightHand.WhenHoverOnItem.AddListener(WhenRightHandHoverOnItem);

            leftHand.WhenUnhoverOnItem.AddListener(WhenLeftHandUnhoverOnItem);
            rightHand.WhenUnhoverOnItem.AddListener(WhenRightHandUnhoverOnItem);

            leftHand.transform.SetParent(transform.parent);
            rightHand.transform.SetParent(transform.parent);

            leftHand.SetTarget(leftHandTarget);
            rightHand.SetTarget(rightHandTarget);


            RegisterInventory(leftHandInventory);
            RegisterInventory(rightHandInventory);
        }



        private void Update()
        {


            if (rightHandHoveredItem && Input.GetKeyDown(KeyCode.E))
            {
                rightHandInventory.Pickup(rightHandHoveredItem);
            }

            if (leftHandHoveredItem && Input.GetKeyDown(KeyCode.Q))
            {
                leftHandInventory.Pickup(leftHandHoveredItem);
            }


            bool mouseDown = Input.GetKeyDown(KeyCode.Mouse0);
            bool leftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
            if (mouseDown)
            {
                rightHandInventory.UseActiveItem(this);
            }

        }

        NexusItem leftHandHoveredItem;
        NexusItem rightHandHoveredItem;

        public UnityEvent<NexusWeapon> WhenAttacked { get; set; } = new UnityEvent<NexusWeapon>();

        private void WhenLeftHandHoverOnItem(NexusItem item)
        {
            leftHandHoveredItem = item;
        }
        private void WhenRightHandHoverOnItem(NexusItem item)
        {
            rightHandHoveredItem = item;
        }

        private void WhenLeftHandUnhoverOnItem(NexusItem item)
        {
            if (leftHandHoveredItem == item)
                leftHandHoveredItem = null;
        }
        private void WhenRightHandUnhoverOnItem(NexusItem item)
        {
            if (rightHandHoveredItem == item)
                rightHandHoveredItem = null;
        }

        public void OnAttacked(NexusWeapon weapon)
        {

        }
    }
}
