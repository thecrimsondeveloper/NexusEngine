using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform head;
    [SerializeField] float maxHorizontalLookAngle = 20f;
    [SerializeField] float maxVerticalLookAngle = 20f;

    Vector3 startPosition;
    Quaternion startRotation;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        //lock the mouse
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, rigidbody.velocity.normalized * 2f);
    }


    // Update is called once per frame
    void Update()
    {
        //look up and down
        MouseRotation();
        KeyMovement();
    }

    void KeyMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float yVelocity = rigidbody.velocity.y;
        //forward direction
        Vector3 movementDirection = transform.forward * vertical + transform.right * horizontal;

        Vector3 targetVelocity = new Vector3(movementDirection.x, 0, movementDirection.z) * moveSpeed;
        targetVelocity.y = yVelocity;

        rigidbody.velocity = targetVelocity;

        //reset the player if it falls off the map
        if (transform.position.y < -5)
        {
            transform.position = startPosition;
            rigidbody.velocity = Vector3.zero;
        }
    }

    float targetRotationAngle = 0;
    float targetVerticalAngle = 0;
    void MouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //rotate the player head in the direction of the mouse
        targetRotationAngle += mouseX * rotateSpeed;
        targetRotationAngle = Mathf.Clamp(targetRotationAngle, -maxHorizontalLookAngle, maxHorizontalLookAngle);

        targetVerticalAngle += mouseY * rotateSpeed;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, -maxVerticalLookAngle, maxVerticalLookAngle);

        //clamp the rotation angle
        // head.localRotation = Quaternion.Euler(0, targetRotationAngle, 0);

        // rotate the body if the horizontal angle is too far
        transform.Rotate(Vector3.up, mouseX * rotateSpeed * 100 * Time.deltaTime);

        head.eulerAngles = new Vector3(-targetVerticalAngle, head.eulerAngles.y, 0);
    }
}
