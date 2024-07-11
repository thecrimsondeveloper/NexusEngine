using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;




#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Toolkit.XR
{
    public class XRPlayer : MonoSingleton<XRPlayer>
    {
        [SerializeField] protected Transform head;
        [SerializeField] protected Transform leftHand;
        [SerializeField] protected Transform rightHand;
        [SerializeField] protected Rigidbody lookGazeAnchor;
        [SerializeField] protected OVRHand leftHandOVR;
        [SerializeField] protected OVRHand rightHandOVR;
        [SerializeField] protected XRPlayerVisuals visuals;
        [SerializeField] protected XRPlayerSettings settings;

        public static Pose HeadPose { get; protected set; } = Pose.identity;
        public static Pose NeckPose { get; protected set; } = Pose.identity;
        public static XRPlayerHand LeftHand { get; private set; } = new XRPlayerHand();
        public static XRPlayerHand RightHand { get; private set; } = new XRPlayerHand();
        public static XRPlayerVisuals Visuals { get; private set; } = null;
        public static Vector3 Position => Instance != null ? Instance.transform.position : Vector3.zero;
        public static Quaternion Rotation => Instance != null ? Instance.transform.rotation : Quaternion.identity;

        private void Start()
        {
            InitializeHands();

            if (visuals != null)
            {
                Visuals = visuals;
            }

            //set gaze anchor hover distance
            if (lookGazeAnchor != null)
            {
                lookGazeAnchor.transform.localPosition = new Vector3(0, 0, settings.gazeAnchorHoverDistance);
            }
        }

        private void Internal_MovePlayer(Vector3 position)
        {
            transform.position = position;
        }


        public static void MovePlayer(Vector3 position)
        {
            Instance.Internal_MovePlayer(position);
        }

        [Button]
        public static void SetParent(Transform parent, bool worldPositionStays = true, bool worldRotationStays = true)
        {
            if (Instance == null)
            {
                return;
            }
            Instance.transform.SetParent(parent);

            if (worldPositionStays == false)
            {
                Instance.transform.localPosition = Vector3.zero;
            }
            if (worldRotationStays == false)
            {
                Instance.transform.localRotation = Quaternion.identity;
            }
        }

        public static void SetLocalPosition(Vector3 position)
        {
            Instance.transform.localPosition = position;
        }

        public static void SetPosition(Vector3 position)
        {
            Instance.transform.position = position;
        }

        public static void SetRotation(Quaternion rotation)
        {
            Instance.transform.rotation = rotation;
        }

        public static void EnablePassthrough()
        {
            if (Visuals != null)
            {
                Visuals.EnablePassthrough();
            }
        }

        public static void DisablePassthrough()
        {
            if (Visuals != null)
            {
                Visuals.DisablePassthrough();
            }
        }


        public static XRPlayerHand ClosestHand(Vector3 position)
        {
            float leftDistance = Vector3.Distance(LeftHand.Position, position);
            float rightDistance = Vector3.Distance(RightHand.Position, position);

            if (leftDistance < rightDistance)
            {
                return LeftHand;
            }
            else
            {
                return RightHand;
            }
        }

        public static void SetGazeAnchorDistance(float distance)
        {
            if (Instance.lookGazeAnchor != null)
            {
                Instance.lookGazeAnchor.transform.localPosition = new Vector3(0, 0, distance);
            }
        }




#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            //if the application is not playing, don't draw the gizmos
            if (Application.isPlaying == false)
            {
                return;
            }

            //draw the head
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(HeadPose.position, 0.1f);
            Gizmos.DrawLine(HeadPose.position, HeadPose.position + HeadPose.forward);

            //draw the leftHand
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(LeftHand.Position, 0.01f);

            //draw the rightHand
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(RightHand.Position, 0.01f);

        }
#endif

        [Button("Initialize Hands")]
        void InitializeHands()
        {
            LeftHand.Initialize(Mathf.CeilToInt(settings.timeToTrackAverages / Time.fixedDeltaTime));
            RightHand.Initialize(Mathf.CeilToInt(settings.timeToTrackAverages / Time.fixedDeltaTime));
        }



        private void Update()
        {
            UpdateHand(LeftHand, true);
            UpdateHand(RightHand, false);
        }

        protected virtual void FixedUpdate()
        {
            HeadPose = new Pose(head.position, head.rotation);
            NeckPose = new Pose(HeadPose.position + (Vector3.down * settings.neckHeight), HeadPose.rotation);

            FixedUpdateHand(LeftHand, leftHand, true, leftHandOVR);
            FixedUpdateHand(RightHand, rightHand, false, rightHandOVR);
        }

        protected void UpdateHand(XRPlayerHand xrHand, bool isLeft)
        {
            //punch events will reset the isPunching bool once the player pulls their hand back
            float velocityDot = Vector3.Dot(xrHand.FingerTipDirection, xrHand.averageVelocity.normalized);


            //I feel like this could be written better
            bool isPunchingFastEnough = xrHand.averageVelocity.magnitude > settings.punchSpeed;
            if (isPunchingFastEnough && velocityDot > 0.3f && xrHand.hasResetLastPunch)
            {
                Debug.LogWarning("Punch COME BACK : UpdateHand() in XRPlayer.cs also: " + velocityDot);
                Debug.Log("Punch");
                xrHand.OnPunch.Invoke(xrHand.averageVelocity);
                xrHand.hasResetLastPunch = false;
            }
            else if (xrHand.hasResetLastPunch == false && velocityDot < -0.5f)
            {
                xrHand.hasResetLastPunch = true;
            }
        }

        //create a temp pose to store the hand's position and rotation and only have to create a new pose once
        //this will save memory and garbage collection
        protected void FixedUpdateHand(XRPlayerHand xrHand, Transform handAnchor, bool isLeft = true, OVRHand overHand = null)
        {

            //compare the current pose to the last 
            //if the handanchor is at zero, the hand is not being tracked
            if (handAnchor.position == Vector3.zero)
            {
                return;
            }

            xrHand.fixedDeltaPosition = handAnchor.position - xrHand.Position;
            xrHand.velocity = (handAnchor.position - xrHand.Position) / Time.deltaTime;
            xrHand.speed = xrHand.velocity.magnitude;

            //shift the previous velocities to the right
            for (int i = xrHand.previousVelocities.Length - 1; i > 0; i--)
            {
                xrHand.previousVelocities[i] = xrHand.previousVelocities[i - 1];
            }
            //add the current velocity to the first index
            xrHand.previousVelocities[0] = xrHand.velocity;

            //calculate the average velocity
            xrHand.averageVelocity = Vector3.zero;
            for (int i = 0; i < xrHand.previousVelocities.Length; i++)
            {
                xrHand.averageVelocity += xrHand.previousVelocities[i];
            }
            xrHand.averageVelocity /= xrHand.previousVelocities.Length;


            Pose handAnchorPose = new Pose(handAnchor.position, handAnchor.rotation);
            //add the offset to the hand anchor pose
            Vector3 eulerOffset = (xrHand == LeftHand) ? settings.leftHandOffset : settings.rightHandOffset;
            Vector3 positionOffset = (xrHand == LeftHand) ? settings.leftHandPositionOffset : settings.rightHandPositionOffset;
            handAnchorPose.rotation = handAnchorPose.rotation * Quaternion.Euler(eulerOffset);

            xrHand.PalmDirection = handAnchorPose.forward;
            xrHand.FingerTipDirection = handAnchorPose.up;
            xrHand.ThumbDirection = handAnchorPose.right * (isLeft ? 1 : -1);

            //set the pose
            xrHand.Position = handAnchor.position + handAnchorPose.forward * positionOffset.z + handAnchorPose.right * positionOffset.x + handAnchorPose.up * positionOffset.y;
            xrHand.LocalPosition = handAnchor.InverseTransformPoint(xrHand.Position);
            xrHand.Rotation = handAnchor.rotation;
            xrHand.LocalRotation = handAnchor.localRotation;


            //direction
            Vector3 up = xrHand.PalmDirection;
            float dot = Vector3.Dot(up, Vector3.up);
            if (dot > 0.7f)
            {
                xrHand.direction = HandDirection.PalmUp;
            }
            else if (dot < -0.7f)
            {
                xrHand.direction = HandDirection.PalmDown;
            }
            else
            {
                xrHand.direction = HandDirection.PalmMiddle;
            }


            //pinch
            if (overHand != null)
            {
                xrHand.indexPinchStrength = overHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
                xrHand.middlePinchStrength = overHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
                xrHand.ringPinchStrength = overHand.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
                xrHand.pinkyPinchStrength = overHand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky);
            }


            //distance
            xrHand.distanceFromNeck = Vector3.Distance(xrHand.Position, NeckPose.position);
            xrHand.horizontalDistanceFromNeck = Vector3.Distance(new Vector3(xrHand.Position.x, 0, xrHand.Position.z),
                                                                        new Vector3(NeckPose.position.x, 0, NeckPose.position.z));
            xrHand.verticalDistanceFromNeck = xrHand.Position.y - NeckPose.position.y;

            xrHand.maxHorizontalDistance = Mathf.Max(xrHand.maxHorizontalDistance, xrHand.horizontalDistanceFromNeck);

            //clamp the max distance to 1.1
            if (xrHand.maxHorizontalDistance > 1.1f)
            {
                xrHand.maxHorizontalDistance = 1.1f;
            }
        }

        public class XRPlayerHand
        {
            public void Initialize(int averageDataPointsToTrack)
            {
                previousVelocities = new Vector3[averageDataPointsToTrack];
                for (int i = 0; i < previousVelocities.Length; i++)
                {
                    previousVelocities[i] = Vector3.zero;
                }
            }

            public Vector3 FingerTipDirection { get; internal set; } = Vector3.zero;
            public Vector3 PalmDirection { get; internal set; } = Vector3.zero;
            public Vector3 ThumbDirection { get; internal set; } = Vector3.zero;
            public Vector3 Position { get; internal set; } = Vector3.zero;
            public Vector3 LocalPosition { get; internal set; } = Vector3.zero;
            public Quaternion Rotation { get; internal set; } = Quaternion.identity;
            public Quaternion LocalRotation { get; internal set; } = Quaternion.identity;
            public Vector3 fixedDeltaPosition { get; internal set; } = Vector3.zero;
            public Vector3 velocity { get; internal set; } = Vector3.zero;
            public Vector3 averageVelocity { get; internal set; } = Vector3.zero;
            public Vector3[] previousVelocities { get; internal set; } = new Vector3[15];
            public float speed { get; internal set; } = 0f;

            public float indexPinchStrength { get; internal set; } = 0f;
            public float middlePinchStrength { get; internal set; } = 0f;
            public float ringPinchStrength { get; internal set; } = 0f;
            public float pinkyPinchStrength { get; internal set; } = 0f;

            public float distanceFromNeck { get; internal set; } = 0f;
            public float horizontalDistanceFromNeck { get; internal set; } = 0f;
            public float verticalDistanceFromNeck { get; internal set; } = 0f;
            public float maxHorizontalDistance { get; internal set; } = 0.5f;
            public bool hasResetLastPunch { get; internal set; } = true;
            public UnityEvent<Vector3> OnPunch { get; internal set; } = new UnityEvent<Vector3>();
            public HandDirection direction { get; internal set; } = HandDirection.PalmUp;
        }

        public enum HandDirection
        {
            PalmUp,
            PalmDown,
            PalmMiddle
        }



    }
}
