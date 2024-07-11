using System;
using System.Collections;
using System.Collections.Generic;
using ToyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class NexusLookAtEvents : NexusBlock
    {

        public NexusRay[] targets;
        [Range(-1, 1)] public float dotThreshold = 0f;


        public NexusEventBlock OnLookedAt;
        public NexusEventBlock OnLookedAway;

        #region Working Variables, do not want to reallocate memory every frame
        Vector3 largestDirToThis = Vector3.zero;
        Vector3 largestDotOrigin = Vector3.zero;
        Vector3 largestDotDirection = Vector3.zero;
        Vector3 currentDir = Vector3.zero;
        Vector3 currentDirection = Vector3.zero;

        #endregion

        bool currentlyLookedAt = false;
        bool lookedAtLastFrame = false;

        public void Update()
        {
            float largestDot = -1;
            float currdot = -1;
            NexusRay target = null;
            for (int i = 0; i < targets.Length; i++)
            {
                target = targets[i];
                currentDir = (transform.position - target.origin).normalized;
                currentDirection = target.direction;
                currdot = Vector3.Dot(currentDir, currentDirection);
                if (currdot > largestDot)
                {
                    largestDot = currdot;
                    largestDirToThis = currentDir;
                    largestDotDirection = currentDirection;
                    largestDotOrigin = target.origin;
                }
            }


            float dot = Vector3.Dot(largestDirToThis, largestDotDirection);
            currentlyLookedAt = dot > dotThreshold;

            if (currentlyLookedAt)
            {
                Debug.DrawRay(transform.position, largestDirToThis, Color.red, 0.2f);
                Debug.DrawRay(largestDotOrigin, largestDotDirection, Color.blue, 0.2f);
            }


            bool lookedAway = lookedAtLastFrame && currentlyLookedAt == false;
            bool lookedAt = lookedAtLastFrame == false && currentlyLookedAt;

            if (lookedAway)
            {
                OnLookedAway.InvokeBlock();
            }
            if (lookedAt)
            {
                OnLookedAt.InvokeBlock();
            }




            lookedAtLastFrame = dot > dotThreshold;
        }


    }
}
