using UnityEngine;

namespace ToyBox
{
    public class RaycastDotDrawer : MonoBehaviour
    {
        public ComputeShader CS;
        public MeshRenderer MR;
        private RenderTexture RT;

        public Collider detectionSurface;
        public ThumbTack thumbTack;

        void Start()
        {
            RT = new RenderTexture(1024, 1024, 0);
            RT.enableRandomWrite = true;
            RT.Create();
            MR.material.SetTexture("_DotSurface", RT);

            // Set the render texture to white
            Graphics.SetRenderTarget(RT);
            GL.Clear(true, true, Color.white);
            Graphics.SetRenderTarget(null);
        }

        // Create a public function to add a dot to the render texture at a specific position
        public void AddDot(Vector2 position, float size)
        {
            CS.SetVector("target", position);
            CS.SetFloat("distanceThreshold", size);
            CS.SetTexture(0, "surface", RT);
            CS.Dispatch(0, RT.width / 8, RT.height / 8, 1);
        }

        private Vector3 closestPoint;
        private Vector2 uv;
        private float dotSize;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ThumbTack _thumbTack))
            {
                _thumbTack.isPinned = true;
                thumbTack = _thumbTack;
                // Get the closest point on the detection surface to the thumbtack
                Vector3 _closestPoint = detectionSurface.ClosestPoint(_thumbTack.transform.position);
                closestPoint = _closestPoint;

                // Convert the closest point to local space of the detection surface
                Vector3 localPoint = detectionSurface.transform.InverseTransformPoint(closestPoint);

                // Assuming the detection surface is a Plane, get its local size
                Vector3 size = detectionSurface.bounds.size;

                // Assuming the detection surface is axis-aligned
                Vector3 localMin = detectionSurface.transform.InverseTransformPoint(detectionSurface.bounds.min);
                Vector3 localMax = detectionSurface.transform.InverseTransformPoint(detectionSurface.bounds.max);

                // Map the local position to UV coordinates
                Vector2 _uv = new Vector2(
                    Mathf.InverseLerp(localMin.x, localMax.x, localPoint.x),
                    Mathf.InverseLerp(localMin.z, localMax.z, localPoint.z)
                );

                uv = _uv;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ThumbTack _thumbTack))
            {
                // Calculate the distance between the closest point and the thumbtackIntersectionPoint
                float distance = Vector3.Distance(closestPoint, _thumbTack.InterSectionPoint.position);

                if (thumbTack.isPinned)
                {
                    dotSize = Mathf.Clamp(distance * 0.2f, 0.001f, 0.01f); // Example scaling, adjust as necessary
                }
                else
                {
                    dotSize = 0.01f;
                }
                // Use the distance to determine the size of the dot (you can scale this as needed)


                // Add a dot to the render texture at the UV coordinates with the calculated size
                AddDot(uv, dotSize);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ThumbTack _thumbTack))
            {
                _thumbTack.isPinned = false;
            }
        }
    }
}
