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

        public enum PathFollowMode
        {
            Clamp,   // Stop at the end of the path
            Loop,    // Loop back to the start
            Mirror   // Reverse direction upon reaching the end
        }

        private MovementAction _action;
        public List<Rigidbody> kinematicRigidbodies;
        private SplineHandler _splineHandler; // Reference to the spline handler for following a path
        private bool _isReversing; // Used for mirror mode

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

        // New method to follow a spline path with different path follow modes
        private async UniTask FollowPath()
        {
            if (_splineHandler == null)
            {
                Debug.LogError("SplineHandler is null. Cannot follow path.");
                Sequence.Finish(this);
                return;
            }

            // Ensure that the spline is properly initialized and ready to provide points
            Sequence.Run(_splineHandler, new SequenceRunData { sequenceData = _splineHandler.currentData }); // Run the spline handler
            await UniTask.Delay(100); // Wait for spline to initialize

            float t = 0f; // Time factor for traveling along the path
            int totalSegments = _splineHandler.GetSegmentCount(); // Get the number of segments in the spline
            bool isPathComplete = false;

            while (!isPathComplete)
            {
                foreach (var rb in kinematicRigidbodies)
                {
                    // Normalize t to be between 0 and 1 for each segment
                    int segmentIndex = Mathf.FloorToInt(t * totalSegments);
                    float segmentT = (t * totalSegments) - segmentIndex;

                    Vector3 targetPosition = _splineHandler.GetPointAtSegment(segmentIndex, segmentT);
                    rb.MovePosition(targetPosition);
                }

                // Handle different path follow modes
                switch (currentData.pathFollowMode)
                {
                    case PathFollowMode.Clamp:
                        t += Time.deltaTime * currentData.pathFollowSpeed / totalSegments;
                        if (t >= 1f)
                        {
                            t = 1f;
                            isPathComplete = true;
                        }
                        break;

                    case PathFollowMode.Loop:
                        t += Time.deltaTime * currentData.pathFollowSpeed / totalSegments;
                        if (t >= 1f)
                        {
                            t = 0f; // Reset t to smoothly start from the beginning again
                        }
                        break;

                    case PathFollowMode.Mirror:
                        t += (_isReversing ? -1 : 1) * Time.deltaTime * currentData.pathFollowSpeed / totalSegments;
                        if (t >= 1f)
                        {
                            t = 1f;
                            _isReversing = true; // Reverse direction
                        }
                        else if (t <= 0f)
                        {
                            t = 0f;
                            _isReversing = false; // Reverse back to forward
                        }
                        break;
                }

                await UniTask.Yield();
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
        public RigidbodyKinematicMovementHandler.PathFollowMode pathFollowMode; // Mode for path following (Clamp, Loop, Mirror)
    }
}
