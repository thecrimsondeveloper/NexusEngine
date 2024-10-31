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
        // Used for mirror mode
        private Vector3 targetPosition;
        private Vector3 targetRotation;
        private float moveSpeed = 1.0f;
        private float rotateSpeed = 1.0f;
        private float pathFollowSpeed = 1.0f; // Speed for following the spline path
        private PathFollowMode pathFollowMode; // Mode for path following (Clamp, Loop, Mirror)
        private bool orientTowardsMovement = true;  // Flag to check if orientation is needed
        private bool _isReversing = false; // Flag to track the direction of path following

        protected override UniTask Initialize(RigidbodyKinematicMovementData currentData)
        {
            _action = currentData.movementAction;
            if (currentData.kinematicRigidbodies != null)
                kinematicRigidbodies = currentData.kinematicRigidbodies;
            if (currentData.splineHandler != null)
                _splineHandler = currentData.splineHandler;
            targetPosition = currentData.targetPosition;
            targetRotation = currentData.targetRotation;
            moveSpeed = currentData.moveSpeed;
            rotateSpeed = currentData.rotateSpeed;
            pathFollowSpeed = currentData.pathFollowSpeed;
            pathFollowMode = currentData.pathFollowMode;
            orientTowardsMovement = currentData.orientTowardsMovement;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Ensure valid action type before proceeding
            if (_action == null)
            {
                Debug.LogError("MovementAction is not set.");
                return;
            }

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
                    // Ensure the spline handler is valid before proceeding
                    if (_splineHandler != null)
                    {
                        FollowPath().Forget(); 
                    }
                    else
                    {
                        Debug.LogError("SplineHandler is null, cannot follow path.");
                    }
                    break;
                default:
                    Debug.LogError("Unknown MovementAction specified.");
                    break;
            }
        }

        private async UniTask MoveToPosition()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                if (rb != null)
                {
                    rb.MovePosition(targetPosition);
                    await UniTask.Yield();
                }
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private async UniTask RotateToRotation()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                if (rb != null)
                {
                    rb.MoveRotation(Quaternion.Euler(targetRotation));
                    await UniTask.Yield();
                }
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        private async UniTask LerpPosition()
        {
            foreach (var rb in kinematicRigidbodies)
            {
                while (Vector3.Distance(rb.position, targetPosition) > 0.01f)
                {
                    rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.deltaTime * moveSpeed));
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
                while (Quaternion.Angle(rb.rotation, Quaternion.Euler(targetRotation)) > 0.1f)
                {
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotateSpeed));
                    await UniTask.Yield();
                }
            }
            Sequence.Finish(this);
            Sequence.Stop(this);
        }

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
            int lastPausedControlPoint = -1; // Track the last control point we paused at

            while (!isPathComplete)
            {
                foreach (var rb in kinematicRigidbodies)
                {
                    if (rb == null)
                    {
                        Debug.LogError("Rigidbody is null in kinematicRigidbodies list.");
                        continue;
                    }

                    // Normalize t to be between 0 and 1 for each segment
                    int segmentIndex = Mathf.FloorToInt(t * totalSegments);
                    float segmentT = (t * totalSegments) - segmentIndex;

                    Vector3 targetPosition = _splineHandler.GetPointAtSegment(segmentIndex, segmentT);
                    Vector3 movementDirection = (targetPosition - rb.position).normalized;
                    rb.MovePosition(targetPosition);

                    if (orientTowardsMovement)
                        OrientTowardsMovement(rb, movementDirection);

                    // Check if we should pause at the control point and ensure we only pause once per control point
                    if (segmentIndex != lastPausedControlPoint && await ShouldPauseAtControlPoint(segmentIndex))
                    {
                        Debug.Log($"Paused at control point {segmentIndex} for the specified duration.");
                        lastPausedControlPoint = segmentIndex; // Update to prevent re-pausing at the same point
                        await UniTask.Yield();
                        continue; // Move to the next control point after the pause
                    }

                    // Handle different path follow modes
                    switch (pathFollowMode)
                    {
                        case PathFollowMode.Clamp:
                            t += Time.deltaTime * pathFollowSpeed / totalSegments;
                            if (t >= 1f)
                            {
                                t = 1f;
                                isPathComplete = true;
                            }
                            break;

                        case PathFollowMode.Loop:
                            t += Time.deltaTime * pathFollowSpeed / totalSegments;
                            if (t >= 1f)
                            {
                                t = 0f; // Reset t to smoothly start from the beginning again
                            }
                            break;

                        case PathFollowMode.Mirror:
                            // Increment or decrement t depending on the direction (forward or reverse)
                            t += (_isReversing ? -1 : 1) * Time.deltaTime * pathFollowSpeed / totalSegments;

                            // If t reaches the end (1.0), reverse direction
                            if (t >= 1f)
                            {
                                t = 1f; // Clamp t to 1
                                _isReversing = true; // Reverse direction
                                lastPausedControlPoint = -1; // Reset pause tracking when reversing
                            }
                            // If t reaches the beginning (0.0), reverse direction again
                            else if (t <= 0f)
                            {
                                t = 0f; // Clamp t to 0
                                _isReversing = false; // Change direction back to forward
                                lastPausedControlPoint = -1; // Reset pause tracking when reversing
                            }
                            break;
                    }

                    await UniTask.Yield();
                }
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }


        // Ensure proper orientation of the rigidbody toward the movement direction
        private void OrientTowardsMovement(Rigidbody rb, Vector3 movementDirection)
        {
            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotateSpeed));
            }
        }

        // Check if we should pause at a control point and handle the pause duration
        private async UniTask<bool> ShouldPauseAtControlPoint(int segmentIndex)
        {
            bool shouldPause = await _splineHandler.CheckForPauseAtControlPoint(segmentIndex);
            if (shouldPause)
            {
                // Get pause duration from the spline handler and delay
                float pauseDuration = _splineHandler.pauseDurations[_splineHandler.pauseAtControlPoints.IndexOf(segmentIndex)];
                Debug.Log($"Pausing for {pauseDuration} seconds at control point {segmentIndex}");
                await UniTask.Delay((int)(pauseDuration * 1000)); // Pause for the specified duration
                return true;
            }
            return false;
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
        public bool orientTowardsMovement = true;  // Flag to check if orientation is needed
    }
}
