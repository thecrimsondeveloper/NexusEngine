using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction.Input;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox.Minigames.FeedingFrenzy
{
    public class HippoPlayerHand : MonoBehaviour
    {
        public enum HandType
        {
            Left,
            Right
        }
        [SerializeField] HandType hand;

        public HandType Hand => hand;
        public Rigidbody handRigidbody;
        public Transform handTransform;

        public float speed = 10f;

        public bool IsLeft => hand == HandType.Left;

        private void Update()
        {
            Vector3 targetPosition = hand == HandType.Left ? XRPlayer.LeftHand.Position : XRPlayer.RightHand.Position;

            //set the velocity of the rigidbody to move towards the target position
            handRigidbody.velocity = (targetPosition - handTransform.position) * speed;
            handTransform.transform.rotation = hand == HandType.Left ? XRPlayer.LeftHand.Rotation : XRPlayer.RightHand.Rotation;
        }





    }
}
