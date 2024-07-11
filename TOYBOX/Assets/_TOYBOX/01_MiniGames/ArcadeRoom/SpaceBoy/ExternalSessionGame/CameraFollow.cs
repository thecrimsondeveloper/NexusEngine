using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smoothing speed of camera movement

    [SerializeField] private Vector3 offset; // Offset of the camera from the player

    //add rotational offset
    public Vector3 rotationOffset;

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Smoothly rotate the camera towards the rotation offset
            Quaternion desiredRotation = Quaternion.Euler(rotationOffset);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);


            // Set the position of the camera
            transform.position = smoothedPosition;

        }
    }
}
