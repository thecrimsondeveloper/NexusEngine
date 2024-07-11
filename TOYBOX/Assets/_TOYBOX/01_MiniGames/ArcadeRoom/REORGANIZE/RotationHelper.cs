using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox
{
    public class RotationHelper : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 10f;
        public enum RotationAxis
        {
            X,
            Y,
            Z
        }

        public RotationAxis rotationAxis = RotationAxis.X;

        public enum RotationMode
        {
            Free,
            Constant,
            Eased,
            Random
        }

        [SerializeField, ShowIf("rotationMode", RotationMode.Random)] private float changeInterval = 3f;
        [SerializeField, ShowIf("rotationMode", RotationMode.Random)] private bool isMovingToTarget = false;

        public enum AxisMode
        {
            TwoD,
            ThreeD
        }

        public RotationMode rotationMode = RotationMode.Free;
        public AxisMode axisMode = AxisMode.ThreeD;

        private void Update()
        {
            switch (rotationMode)
            {
                case RotationMode.Free:
                    //RotateFree();
                    break;
                case RotationMode.Constant:
                    RotateConstant();
                    break;
                case RotationMode.Eased:
                    //RotateEased();
                    break;
                case RotationMode.Random:
                    if (isMovingToTarget)
                    {
                        return;
                    }
                    RotateRandom();
                    break;
            }
        }

        private void RotateConstant()
        {
            switch (axisMode)
            {
                case AxisMode.TwoD:
                    RotateConstant2D();
                    break;
                case AxisMode.ThreeD:
                    RotateConstant3D();
                    break;
            }
        }

        private void RotateConstant2D()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        private void RotateConstant3D()
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
                    break;
                case RotationAxis.Y:
                    transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                    break;
                case RotationAxis.Z:
                    transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                    break;
            }
        }

        private void RotateRandom()
        {
            switch (axisMode)
            {
                case AxisMode.TwoD:
                    RotateRandom2D();
                    break;
                case AxisMode.ThreeD:
                    RotateRandom3D();
                    break;
            }
        }

        private UniTask RotateRandom2D()
        {
            isMovingToTarget = true;
            //pick a target rotation amount 
            float targetRotation = Random.Range(0, 360);
            //rotate towards the target rotation over the change interval
            float time = 0;
            while (time < changeInterval)
            {
                time += Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetRotation), rotationSpeed * Time.deltaTime);
                UniTask.Yield(); //wait for the next frame
            }

            isMovingToTarget = false;

            return UniTask.CompletedTask;
        }

        private UniTask RotateRandom3D()
        {
            isMovingToTarget = true;
            //pick a target rotation amount 
            Quaternion targetRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Vector3 targetAxis;
            //pick the required axis
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    targetAxis = Vector3.right;
                    break;
                case RotationAxis.Y:
                    targetAxis = Vector3.up;
                    break;
                case RotationAxis.Z:
                    targetAxis = Vector3.forward;
                    break;
            }

            //rotate towards the target rotation over the change interval
            float time = 0;
            while (time < changeInterval)
            {
                time += Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                UniTask.Yield(); //wait for the next frame
            }

            isMovingToTarget = false;

            return UniTask.CompletedTask;
        }
    }
}
