using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Toolkit.Extras
{
    public abstract class LookAt : MonoBehaviour
    {
        protected abstract Pose targetPose { get; set; }

        public abstract void OnDataUpdate();
        [SerializeField] Vector3 rotationOffset;
        [SerializeField, Range(0, 1)] float dampening;
        public enum LockedAxis
        {
            X,
            Y,
            Z
        }

        [SerializeField] LockedAxis lockedAxis = LockedAxis.Y;

        Vector3 startLocalRotation = Vector3.zero;



        private void Start()
        {
            startLocalRotation = transform.localRotation.eulerAngles;

        }

        private void OnEnable()
        {
            DataUpdate();
            SnapToTarget();
        }
        bool isDeactivated = false;

        Quaternion targetRotation = Quaternion.identity;

        private void Update()
        {
            if (isDeactivated)
                return;

            DataUpdate();
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * (1 / dampening));
            //
            if (lockedAxis == LockedAxis.X)
            {
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            else if (lockedAxis == LockedAxis.Y)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            }
            else if (lockedAxis == LockedAxis.Z)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            }
        }

        void DataUpdate()
        {
            OnDataUpdate();
            Vector3 dir = targetPose.position - transform.position;
            targetRotation = (dir == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(dir.normalized))
                            * Quaternion.Euler(rotationOffset);

            if (dampening <= 0)
            {
                transform.rotation = targetRotation;
                return;
            }
        }



        public void Deactivate()
        {
            isDeactivated = true;
            transform.DOLocalRotate(startLocalRotation, 1).OnComplete(() =>
            {
                enabled = false;
            });
        }

        public void Activate()
        {
            enabled = true;
        }

        public void SnapToTarget()
        {
            transform.rotation = targetRotation;
        }
    }


}
