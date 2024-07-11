using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class Spin : MonoBehaviour
    {
        [SerializeField] Vector3 spin = new Vector3(0, 0, 1);

        // Update is called once per frame
        void FixedUpdate()
        {
            //transform.Rotate(spin * Time.deltaTime);
            transform.localRotation *= Quaternion.Euler(spin * Time.deltaTime);
        }
    }
}
