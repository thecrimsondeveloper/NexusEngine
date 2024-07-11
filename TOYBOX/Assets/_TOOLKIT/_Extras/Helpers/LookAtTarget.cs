using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Extras
{
    public class LookAtTarget : LookAt
    {
        [SerializeField] Transform target;

        protected override Pose targetPose
        {
            get => new Pose(target.position, target.rotation);
            set
            {
                target.position = value.position;
                target.rotation = value.rotation;
            }
        }

        public override void OnDataUpdate()
        {
        }

        public void SetTarget(Transform transform)
        {
            target = transform;
        }
    }
}
