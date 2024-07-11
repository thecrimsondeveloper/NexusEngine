using UnityEngine;

namespace ToyBox
{
    public class FloppyBirdCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public Vector3 rotationOffset;
        public AnimationCurve smoothCurve;
        public float rotationSpeed = 5f;

        private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothCurve.Evaluate(Time.time));
                transform.position = smoothedPosition;

                Quaternion desiredRotation = Quaternion.Euler(rotationOffset);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothCurve.Evaluate(Time.time));
            }
        }

        public void ResetCameraToOffset()
        {
            transform.position = target.position + offset;
        }
    }
}
