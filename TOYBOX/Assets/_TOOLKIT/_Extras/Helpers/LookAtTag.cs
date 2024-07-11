using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Extras
{
    public class LookAtTag : LookAt
    {

        Transform target;
        protected override Pose targetPose
        {
            get; set;
        }

        [SerializeField] string tag;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag(tag).transform;
        }

        public override void OnDataUpdate()
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag(tag).transform;
                return;
            }
            targetPose = new Pose(target.position, target.rotation);
        }
    }
}
