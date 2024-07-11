using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

using Toolkit.Playspace;
using Toolkit.Extras;
using Toolkit.DependencyResolution;
using UnityEditor;
using ToyBox;
using Sirenix.Utilities;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace Toolkit.XR
{
    public class XRPlayspace : OVRSceneModelLoader
    {
        public bool IsInitialized { get; private set; }
        [ShowInInspector] public static OVRSceneRoom room;
        public static UnityEvent<OVRSceneRoom> OnLoadSceneRoom = new UnityEvent<OVRSceneRoom>();

        public static XRPlayspaceInfo Info { get; private set; } = new XRPlayspaceInfo();

        public static bool IsSceneLoaded => room != null;

        public class XRPlayspaceInfo
        {
            public Vector3 FloorPosition { get; set; } = Vector3.zero;
            public Vector3 WallPosition { get; set; } = Vector3.zero;
            public Vector3 CeilingPosition { get; set; } = Vector3.zero;
            internal List<Pose> WallPoses { get; set; } = new List<Pose>();
            internal List<Vector3> TopCornerPositions { get; set; } = new List<Vector3>();
            internal List<Vector3> BottomCornerPositions { get; set; } = new List<Vector3>();
            public int TopCornerCount => TopCornerPositions.Count;
            public int BottomCornerCount => BottomCornerPositions.Count;


            public Vector3 RandomWallPosition => WallPoses[Random.Range(0, WallPoses.Count - 1)].position;

            public bool TryGetWallPose(int index, out Pose pose)
            {
                if (index < WallPoses.Count)
                {
                    pose = WallPoses[index];
                    return true;
                }
                pose = Pose.identity;
                return false;
            }

            public bool TryGetTopCornerPosition(int index, out Vector3 position)
            {
                if (index < TopCornerPositions.Count)
                {
                    position = TopCornerPositions[index];
                    return true;
                }
                position = Vector3.zero;
                return false;
            }

            public bool TryGetBottomCornerPosition(int index, out Vector3 position)
            {
                if (index < BottomCornerPositions.Count)
                {
                    position = BottomCornerPositions[index];
                    return true;
                }
                position = Vector3.zero;
                return false;
            }

            public Vector3 GetFarthestTopCornerFrom(Vector3 target)
            {
                Vector3 farthestCorner = Vector3.zero;
                float farthestDistance = 0;

                for (int i = 0; i < TopCornerCount; i++)
                {
                    if (TryGetTopCornerPosition(i, out var cornerPos))
                    {
                        float distance = Vector3.Distance(target, cornerPos);
                        if (distance > farthestDistance)
                        {
                            farthestDistance = distance;
                            farthestCorner = cornerPos;
                        }
                    }
                }

                return farthestCorner;
            }

            public Vector3 GetFarthestBottomCornerFrom(Vector3 target)
            {
                Vector3 farthestCorner = Vector3.zero;
                float farthestDistance = 0;

                for (int i = 0; i < BottomCornerCount; i++)
                {
                    if (TryGetBottomCornerPosition(i, out var cornerPos))
                    {
                        float distance = Vector3.Distance(target, cornerPos);
                        if (distance > farthestDistance)
                        {
                            farthestDistance = distance;
                            farthestCorner = cornerPos;
                        }
                    }
                }

                return farthestCorner;
            }

            public Vector3 GetClosestTopCornerFrom(Vector3 target)
            {
                Vector3 closestCorner = Vector3.zero;
                float closestDistance = float.MaxValue;

                for (int i = 0; i < TopCornerCount; i++)
                {
                    if (TryGetTopCornerPosition(i, out var cornerPos))
                    {
                        float distance = Vector3.Distance(target, cornerPos);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestCorner = cornerPos;
                        }
                    }
                }

                return closestCorner;
            }

            public Vector3 GetClosestBottomCornerFrom(Vector3 target)
            {
                Vector3 closestCorner = Vector3.zero;
                float closestDistance = float.MaxValue;

                for (int i = 0; i < BottomCornerCount; i++)
                {
                    if (TryGetBottomCornerPosition(i, out var cornerPos))
                    {
                        float distance = Vector3.Distance(target, cornerPos);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestCorner = cornerPos;
                        }
                    }
                }

                return closestCorner;
            }

            public Pose GetClosestWallPose(Vector3 target)
            {
                Pose closestPose = Pose.identity;
                float closestDistance = float.MaxValue;

                for (int i = 0; i < WallPoses.Count; i++)
                {
                    var pose = WallPoses[i];
                    float distance = Vector3.Distance(target, pose.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPose = pose;
                    }
                }

                return closestPose;
            }

            public Pose GetFarthestWallPose(Vector3 target)
            {
                Pose farthestPose = Pose.identity;
                float farthestDistance = 0;

                for (int i = 0; i < WallPoses.Count; i++)
                {
                    var pose = WallPoses[i];
                    float distance = Vector3.Distance(target, pose.position);
                    if (distance > farthestDistance)
                    {
                        farthestDistance = distance;
                        farthestPose = pose;
                    }
                }

                return farthestPose;
            }
        }
        void Awake()
        {
            Info = new XRPlayspaceInfo();
        }

        float lastUpdate = 0;

        void Update()
        {
            if (Time.time - lastUpdate < 0.1)
                return;

            if (room)
            {
                Info.FloorPosition = room.Floor.transform.position;
                Info.CeilingPosition = room.Ceiling.transform.position;

                Info.WallPoses.Clear();
                foreach (var wall in room.Walls)
                {
                    Info.WallPoses.Add(new Pose(wall.transform.position, wall.transform.rotation));
                }

                Info.TopCornerPositions.Clear();
                foreach (var position in room.Ceiling.Boundary)
                {
                    Vector3 worldPoint = room.Ceiling.transform.TransformPoint(position);
                    Info.TopCornerPositions.Add(worldPoint);
                }

                Info.BottomCornerPositions.Clear();
                foreach (var position in room.Floor.Boundary)
                {
                    Vector3 worldPoint = room.Floor.transform.TransformPoint(position);
                    Info.BottomCornerPositions.Add(worldPoint);
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
        }


        protected override void OnSceneModelLoadedSuccessfully()
        {
            base.OnSceneModelLoadedSuccessfully();
            OnInitializeScene();
        }

        protected override void OnSceneCaptureReturnedWithoutError()
        {
            base.OnSceneCaptureReturnedWithoutError();
            OnInitializeScene();
        }

        protected override void OnNoSceneModelToLoad()
        {
            base.OnNoSceneModelToLoad();
        }


        void OnInitializeScene()
        {
            room = FindObjectOfType<OVRSceneRoom>();
            if (room)
            {
                OnLoadSceneRoom.Invoke(room);
            }

            IsInitialized = true;
        }


        OVRSemanticClassification currentClassification;
        Transform workingScematicAnchor;
        Pose workingScematicPose;


    }
}

