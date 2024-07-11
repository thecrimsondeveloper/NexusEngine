using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class WheelChair : MonoBehaviour
    {
        Vector3 positionLastFrame;
        // Update is called once per frame


        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
            {
                Update();
            }
        }
        void Update()
        {

            Vector3 dir = transform.position - positionLastFrame;

            transform.rotation = Quaternion.LookRotation(dir);
            //zero out the x and z rotations
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


            positionLastFrame = transform.position;
        }
    }
}
