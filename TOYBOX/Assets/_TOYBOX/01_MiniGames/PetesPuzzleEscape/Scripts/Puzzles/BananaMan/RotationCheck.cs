using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Oculus.Interaction.OneGrabTranslateTransformer;

namespace ToyBox
{
    public class RotationCheck : MonoBehaviour
    {
        public GameObject GrabbableCube;
        [SerializeField] MeshRenderer cubeMesh;
        [SerializeField] float newValue;
        private float originalMinX;


        // Threshold for rotation check (in degrees)
        public float rotationThreshold = 10.0f;

        // Check if the object is rotated 180 degrees

        // Get the current rotation of the object
 

        // Create a rotation representing 180 degrees around the z-axis
        Quaternion targetRotation = Quaternion.Euler(0, 0, 180);

        // Check if the difference in rotation is within the threshold

        private float timer = 0f;
        private float logInterval = 1f; // Log interval set to 1 second

        //  GetComponent().enabled = false;
        // Update is called once per frame

        [SerializeField] float lowerBound = 155f; 
        [SerializeField] float upperBound = 205f;
        private Color orginalColor;


        void Start ()
        {
            orginalColor = cubeMesh.material.color;
            OneGrabTranslateTransformer transformer = GrabbableCube.GetComponent<OneGrabTranslateTransformer>();
            originalMinX = transformer.Constraints.MinX.Value;

        }
        
        
        void Update()
        {
            Quaternion currentRotation = transform.rotation;

            OneGrabTranslateTransformer transformer = GrabbableCube.GetComponent<OneGrabTranslateTransformer>();



            /*            timer += Time.deltaTime;
                        // Check if the timer has exceeded the log interval
                        if (timer >= logInterval)
                        {
                            // Output the log message
                            Debug.Log("Quaternion.Angle(): " + Quaternion.Angle(currentRotation, targetRotation));

                            // Reset the timer to start counting from zero again
                            timer = 0f;
                        }
            */

            // Check if the object is rotated 180 degrees
            if (Quaternion.Angle(currentRotation, targetRotation) >= lowerBound && Quaternion.Angle(currentRotation, targetRotation) <= upperBound) 
            {
              

                transformer.Constraints.MinX.Value = newValue;
                //GrabbableCube.GetComponent<OneGrabTranslateTransformer>().enabled = false;

                cubeMesh.material.color = Color.green; // TODO: activate when cube is pulled out
                Debug.Log("Object is rotated 180 degrees around the z-axis!");
            }
            else
            {
                transformer.Constraints.MinX.Value = originalMinX;
                //reset transform?

                cubeMesh.material.color = orginalColor;
            }
 
        }

    }
}
