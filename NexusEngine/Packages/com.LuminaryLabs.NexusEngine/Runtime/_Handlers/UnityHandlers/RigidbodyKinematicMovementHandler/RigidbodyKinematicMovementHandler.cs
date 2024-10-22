using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class RigidbodyKinematicMovementHandler : EntitySequence<RigidbodyKinematicMovementData>
    {
        public enum MovementAction
        {
            MoveToPosition,
            RotateToRotation,
            LerpPosition,
            SlerpRotation,
            FollowPath // New movement option for following a spline path
        }

        private MovementAction _action;
        public List<Rigidbody> kinematicRigidbodies;
        private SplineHandler _splineHandler; // Reference to the spline handler for following a path

        protected override UniTask Initialize(RigidbodyKinematicMovementData currentData)
        {
            _action = currentData.movementAction;
            if (currentData.kinematicRigidbodies != null)
                kinematicRigidbodies = currentData.kinematicRigidbodies;

            if (currentData.splineHandler != null)
                _splineHandler = currentData.splineHandler; // Assign spline handler for path-following

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            switch (_action)
            {
                case MovementAction.MoveToPosition:
                    MoveToPosition().Forget();
                    break;
                case MovementAction.RotateToRotation:
                    RotateToRotation().Forget();
                    break;
                case MovementAction.LerpPosition:
                    LerpPosition().Forget();
                    break;
                case MovementAction.SlerpRotation:
                    SlerpRotation().Forget();
                    break;
                case MovementAction.FollowPath:
                    FollowPath().Forget(); // New follow path action
                    break;
            }
        }

        private async UniTask MoveToPosition()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                rb.MovePosition(currentData.targetPosition);
                await UniTask.Yield();
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private async UniTask RotateToRotation()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                rb.MoveRotation(Quaternion.Euler(currentData.targetRotation));
                await UniTask.Yield();
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private async UniTask LerpPosition()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                while (Vector3.Distance(rb.position, currentData.targetPosition) > 0.01f)
                {
                    rb.MovePosition(Vector3.Lerp(rb.position, currentData.targetPosition, Time.deltaTime * currentData.moveSpeed));
                    await UniTask.Yield();
                }
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private async UniTask SlerpRotation()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                while (Quaternion.Angle(rb.rotation, Quaternion.Euler(currentData.targetRotation)) > 0.1f)
                {
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(currentData.targetRotation), Time.deltaTime * currentData.rotateSpeed));
                    await UniTask.Yield();
                }
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        // New method to follow a spline path
        private async UniTask FollowPath()
        {
            if (_splineHandler == null)
            {
                Sequence.Finish(this);
                return;
            }

            // Ensure the spline handler is running to generate points
            Sequence.Run(_splineHandler);

            foreach (var rb in kinematicRigidbodies)
            {
                float t = 0f; // Time factor for traveling along the path
                float totalDuration = 1f / currentData.pathFollowSpeed; // Adjust total duration based on speed

                while (t <= 1f)
                {
                    Vector3 targetPosition = _splineHandler.GetPointAt(t);
                    rb.MovePosition(targetPosition);
                    t += Time.deltaTime / totalDuration; // Use a normalized step size based on the follow speed
                    await UniTask.Yield();
                }
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }


        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class RigidbodyKinematicMovementData : SequenceData
    {
        public RigidbodyKinematicMovementHandler.MovementAction movementAction;
        public List<Rigidbody> kinematicRigidbodies;
        public Vector3 targetPosition;
        public Vector3 targetRotation;
        public float moveSpeed = 1.0f;
        public float rotateSpeed = 1.0f;
        public float pathFollowSpeed = 1.0f; // Speed for following the spline path
        public SplineHandler splineHandler; // Reference to the SplineHandler for path following
    }
}
