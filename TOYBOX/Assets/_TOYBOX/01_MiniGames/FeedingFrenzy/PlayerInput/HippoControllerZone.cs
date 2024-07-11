using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox.Minigames.FeedingFrenzy
{
    public class HippoControllerZone : MonoBehaviour
    {
        [SerializeField] FFHippo hippo;
        [SerializeField] float speed = 10f;
        public HippoPlayerHand leftHand;
        public HippoPlayerHand rightHand;


        public bool TryGetHand(out HippoPlayerHand hand)
        {
            //get the closest hand to the hippo
            if (leftHand != null && rightHand != null)
            {
                if (Vector3.Distance(leftHand.transform.position, hippo.transform.position) < Vector3.Distance(rightHand.transform.position, hippo.transform.position))
                {
                    hand = leftHand;
                    return true;
                }
                else
                {
                    hand = rightHand;
                    return true;
                }
            }
            else if (leftHand != null)
            {
                hand = leftHand;
                return true;
            }
            else if (rightHand != null)
            {
                hand = rightHand;
                return true;
            }
            else
            {
                hand = null;
                return false;
            }
        }



        private void Update()
        {
            if (TryGetHand(out HippoPlayerHand hand))
            {

                //move horizontally towards the hand
                Vector2 handPosition = new Vector2(hand.transform.position.x, hand.transform.position.z);
                Vector2 hippoPosition = new Vector2(hippo.transform.position.x, hippo.transform.position.z);
                Vector2 direction = handPosition - hippoPosition;

                hippo.Velocity = new Vector3(direction.x, 0, direction.y) * speed;
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HippoPlayerHand hippoHand))
            {
                if (hippoHand.IsLeft)
                {
                    leftHand = hippoHand;
                }
                else
                {
                    rightHand = hippoHand;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out HippoPlayerHand hippoHand))
            {
                if (hippoHand.IsLeft)
                {
                    leftHand = null;
                }
                else
                {
                    rightHand = null;
                }
            }
        }
    }
}
