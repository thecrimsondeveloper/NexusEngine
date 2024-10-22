using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class SplineHandler : EntitySequence<SplineHandlerData>
    {
        public Transform[] controlPoints;
        private Vector3[] _gizmoPoints;

        protected override UniTask Initialize(SplineHandlerData currentData)
        {
            controlPoints = currentData.controlPoints;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // Calculate gizmo points for visualization
            _gizmoPoints = CalculateBezierPoints(controlPoints, currentData.resolution);
            Sequence.Finish(this);
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        private Vector3[] CalculateBezierPoints(Transform[] points, int resolution)
        {
            Vector3[] bezierPoints = new Vector3[resolution + 1];
            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                bezierPoints[i] = GetBezierPoint(points[0].position, points[1].position, points[2].position, points[3].position, t);
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

        private void OnDrawGizmos()
        {
            if (controlPoints == null || controlPoints.Length != 4)
                return;

            if (_gizmoPoints != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < _gizmoPoints.Length - 1; i++)
                {
                    Gizmos.DrawLine(_gizmoPoints[i], _gizmoPoints[i + 1]);
                }
            }
        }

        public Vector3 GetPointAt(float t)
        {
            return GetBezierPoint(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);
        }
    }

    [System.Serializable]
    public class SplineHandlerData : SequenceData
    {
        public Transform[] controlPoints;
        public int resolution = 20; // Number of points to calculate for smoothness
    }
}
