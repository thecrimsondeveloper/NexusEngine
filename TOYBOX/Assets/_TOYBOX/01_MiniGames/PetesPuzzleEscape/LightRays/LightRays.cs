using System.Collections;
using System.Collections.Generic;
using Fusion.Addons.TestSuit;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox
{
    public class LightRays : MonoBehaviour
    {

        [SerializeField] Transform rayPoint;
        [SerializeField] GameObject startRay;
        [SerializeField] GameObject reflectionRay;

        void OnDrawGizmos()
        {
            Update();
        }

        private void Update()
        {
            if (Physics.Raycast(rayPoint.position, rayPoint.forward, out RaycastHit hit))
            {

                reflectionRay.SetActive(true);
                Vector3 reflectDirection = Vector3.Reflect(rayPoint.forward, hit.normal);

                startRay.transform.localScale = new Vector3(hit.distance, 1, 1);

                reflectionRay.transform.position = hit.point;
                reflectionRay.transform.forward = reflectDirection;
            }
            else
            {
                reflectionRay.SetActive(false);
            }

        }



    }
}
