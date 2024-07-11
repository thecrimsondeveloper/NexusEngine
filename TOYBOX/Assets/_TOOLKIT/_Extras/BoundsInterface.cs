using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;

using UnityEngine;


namespace Extras
{
    public static class BoundsInterface
    {
        public static Vector3 GetRandomPointInBounds(Bounds bounds)
        {
            // Create a single instance of System.Random to ensure better randomness
            System.Random random = new System.Random();

            return bounds.center + new Vector3(
                (float)random.NextDouble() * bounds.size.x + bounds.min.x,
                (float)random.NextDouble() * bounds.size.y + bounds.min.y,
                (float)random.NextDouble() * bounds.size.z + bounds.min.z
            );
        }


        public static Vector3 GetRandomPointInBounds(Bounds[] bounds)
        {
            // Create a single instance of System.Random to ensure better randomness
            System.Random random = new System.Random();

            // Generate a random index within the bounds array
            int randomIndex = random.Next(0, bounds.Length);

            // Return a random point within the selected Bounds
            return GetRandomPointInBounds(bounds[randomIndex]);
        }


        public static Vector3[] GetRandomPointsInBounds(Bounds bounds, int count, float minDistance = 0.0f)
        {
            // Array to store the generated points
            Vector3[] points = new Vector3[count];
            Vector3 newPoint = Vector3.zero;

            // Loop to generate each point
            for (int pointIndex = 0; pointIndex < count; pointIndex++)
            {
                // Attempt counter to prevent infinite loops
                int attemptCount = 0;

                newPoint = GetRandomPointInBounds(bounds);

                //while the distance between the new point and any of the other points is less than the minimum distance
                while (Vector3.Distance(newPoint, points[pointIndex]) < minDistance)
                {
                    newPoint = GetRandomPointInBounds(bounds);
                    attemptCount++;
                    if (attemptCount > 100)
                    {
                        Debug.LogError("Failed to find a point in bounds after 100 attempts");
                        break;
                    }
                }
            }
            return points;
        }

        public static async UniTask<Vector3[]> GetRandomPointsInBounds_Async(Bounds bounds, int count, float minDistance)
        {
            //await a threadpool task to get a random point in the bounds
            return await UniTask.RunOnThreadPool(() => GetRandomPointsInBounds(bounds, count, minDistance));
        }



    }
}
