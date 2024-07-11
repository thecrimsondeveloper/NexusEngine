using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox
{
    public class Vector3Binder : MonoBehaviour
    {
        [SerializeField] VisualEffect visualEffect = null;
        [SerializeField] string vector3Property = "Vector3Property";


        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            Update();
        }

        [SerializeField] Vector3 positionLastFrame = Vector3.zero;
        [SerializeField] Vector3 targetPositionDelta = Vector3.zero;
        // Update is called once per frame
        void Update()
        {
            Vector3 positionDelta = transform.position - positionLastFrame;
            targetPositionDelta = Vector3.Lerp(targetPositionDelta, positionDelta, Time.deltaTime * 100);

            visualEffect.SetVector3(vector3Property, targetPositionDelta * 10);
            positionLastFrame = transform.position;
        }


    }
}
