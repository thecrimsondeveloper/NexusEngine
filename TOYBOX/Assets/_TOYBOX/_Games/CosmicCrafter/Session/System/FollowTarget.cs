using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] bool followPosition = true;
        [SerializeField] bool followRotation = true;
        [SerializeField] Transform target = null;

        [Button("Refresh")]
        private void Update()
        {
            if (target)
            {
                if (followPosition)
                {
                    transform.position = target.position;
                }

                if (followRotation)
                {
                    transform.rotation = target.rotation;
                }
            }
        }
    }
}
