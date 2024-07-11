using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float gazeHorizontalOffset = 1;
    [SerializeField] float gazeVerticalOffset = 1;
    [SerializeField] Vector3 offset;
    [SerializeField] float moveSpeed = 5f;

    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }


    private void Start()
    {
        transform.SetParent(null);
    }


    Vector3 lookAtPosition => (target.forward * gazeHorizontalOffset) + (target.up * gazeVerticalOffset + target.position);
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position, lookAtPosition);
        Gizmos.DrawSphere(lookAtPosition, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, lookAtPosition);

        if (Application.isPlaying)
            SetPosition();
    }

    private void Update()
    {
        SetPosition();
    }



    void SetPosition()
    {
        // Calculate the offset based on the target's rotation
        Vector3 offsetFromRotation = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0) * offset;

        // Calculate the target position with offset
        Vector3 targetPosition = target.position + offsetFromRotation;

        // Move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Make the camera look at the target
        transform.LookAt(lookAtPosition);
    }

}
