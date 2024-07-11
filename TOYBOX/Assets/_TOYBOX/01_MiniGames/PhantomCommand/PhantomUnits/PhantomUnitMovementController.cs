using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomUnitMovementController : MonoBehaviour
    {
        [SerializeField] PhantomUnitMovementHandler[] movementHandler;
        [SerializeField] PhantomUnitMovementHandler unitToFollow;
        [SerializeField] Transform targetPosition;

        [Button]
        public void MoveTo(Vector3 targetPosition)
        {
            foreach (var handler in movementHandler)
            {
                handler.SetTargetPosition(targetPosition);
            }
        }
        [Button]
        public void FollowUnit(PhantomUnitMovementHandler unitToFollow)
        {
            foreach (var handler in movementHandler)
            {
                handler.SetUnitToFollow(unitToFollow);
            }
        }
        [Button]
        public void MoveToTargetPosition()
        {
            foreach (var handler in movementHandler)
            {
                handler.SetTargetPosition(targetPosition.position);
            }
        }
        [Button]
        public void FollowUnit()
        {
            foreach (var handler in movementHandler)
            {
                handler.SetUnitToFollow(unitToFollow);
            }
        }

        [Button]
        public void StopFollowing()
        {
            foreach (var handler in movementHandler)
            {
                handler.StopFollowing();
            }
        }
    }
}
