using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class SplineHandler : EntitySequence<SplineHandlerData>
    {
        public Transform[] controlPoints;
        private List<Vector3> _gizmoPoints;

        protected override UniTask Initialize(SplineHandlerData currentData)
        {
            controlPoints = currentData.controlPoints;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            _gizmoPoints = CalculateBezierPoints(controlPoints, currentData.resolution, currentData.closedLoop);
            Sequence.Finish(this);
        }

        // Get total number of segments in the spline
        public int GetSegmentCount()
        {
            return (controlPoints.Length - 1) / 3;
        }

        // Get the point at a specific segment and t value
        public Vector3 GetPointAtSegment(int segmentIndex, float t)
        {
            int i = segmentIndex * 3;
            if (i + 3 >= controlPoints.Length)
                return controlPoints[controlPoints.Length - 1].position;

            return GetBezierPoint(controlPoints[i].position, controlPoints[i + 1].position, controlPoints[i + 2].position, controlPoints[i + 3].position, t);
        }

        private List<Vector3> CalculateBezierPoints(Transform[] points, int resolution, bool closedLoop)
        {
            List<Vector3> bezierPoints = new List<Vector3>();
            int segmentCount = (closedLoop) ? points.Length : points.Length - 3;

            for (int i = 0; i < segmentCount; i++)
            {
                Transform p0 = points[i];
                Transform p1 = points[(i + 1) % points.Length];
                Transform p2 = points[(i + 2) % points.Length];
                Transform p3 = points[(i + 3) % points.Length];

                for (int j = 0; j <= resolution; j++)
                {
                    float t = j / (float)resolution;
                    Vector3 bezierPoint = GetBezierPoint(p0.position, p1.position, p2.position, p3.position, t);
                    bezierPoints.Add(bezierPoint);
                }
            }

            return bezierPoints;
        }

        private Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }


    [System.Serializable]
    public class SplineHandlerData : SequenceData
    {
        public Transform[] controlPoints;
        public int resolution = 20; // Number of points to calculate for smoothness
        public bool closedLoop = false; // Option to create a closed loop
    }
}
