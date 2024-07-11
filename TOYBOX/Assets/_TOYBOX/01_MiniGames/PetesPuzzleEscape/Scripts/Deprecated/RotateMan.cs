using Oculus.Interaction;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMan : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isGrabbed = false;
    [SerializeField, FoldoutGroup("Dependencies")] PointableUnityEventWrapper events;
   // private Quaternion initialRotation;

    void Start()
    {

        initialPosition = transform.position;
        //initialRotation = transform.rotation;

        events.WhenUnselect.AddListener(OnRelease);
        events.WhenSelect.AddListener(OnGrab);

    }

    void Update()
    {
        if (isGrabbed)
        {
            // Get the current position of the cube
            Vector3 currentPosition = transform.position;
            //transform.rotation = initialRotation;

            // Restrict movement to the z-axis
            currentPosition.x = initialPosition.x;
            currentPosition.y = initialPosition.y;
            currentPosition.z = initialPosition.z;

            // Update the position of the cube
          //  transform.position = currentPosition;
        }
    }

    public void OnGrab(PointerEvent args)
    {
        isGrabbed = true;

    }

    public void OnRelease(PointerEvent args)
    {
        isGrabbed = false;
    }
}
