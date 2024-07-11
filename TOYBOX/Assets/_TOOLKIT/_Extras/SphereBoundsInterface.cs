using System.Collections;
using System.Collections.Generic;
using Toolkit.Extras;
using UnityEngine;

namespace ToyBox
{
    public class SphereBoundsInterface : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public struct SphereBounds
    {
        public Vector3 center;
        public float radius;

        public SphereBounds(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
        public Vector3 CalculateRandomPoint()
        {
            return Random.insideUnitSphere * radius + center;
        }
        public Vector3 CalculateRandomPoint(Vector3[] avoidPoints, float avoidDistance = 0)
        {
            Vector3 point = Vector3.zero;
            float distance = 0;

            do
            {
                point = Random.insideUnitSphere * radius + center;
                distance = Vector3.Distance(center, point);
            } while (distance > radius || !IsPointValid(point, avoidPoints, avoidDistance));


            return point;
        }

        bool IsPointValid(Vector3 point, Vector3[] avoidPoints, float avoidDistance)
        {
            foreach (Vector3 avoidPoint in avoidPoints)
            {
                if (Vector3.Distance(avoidPoint, point) < avoidDistance)
                {
                    return false;
                }
            }

            return true;
        }


        public bool Contains(Vector3 point)
        {
            return Vector3.Distance(center, point) <= radius;
        }

        public bool Intersects(SphereBounds other)
        {
            return Vector3.Distance(center, other.center) <= radius + other.radius;
        }
    }
}
