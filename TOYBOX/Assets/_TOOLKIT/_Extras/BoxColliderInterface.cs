using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras
{
    public static class BoxColliderInterface
    {
        public static Vector3 GetRandomPoint(this BoxCollider boxCollider)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y),
                Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z)
            );
            return randomPoint;
        }
    }
}
