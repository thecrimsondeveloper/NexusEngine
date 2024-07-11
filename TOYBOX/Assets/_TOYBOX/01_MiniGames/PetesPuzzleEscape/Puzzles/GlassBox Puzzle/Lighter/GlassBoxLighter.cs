using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox
{
    public class GlassBoxLighter : MonoBehaviour
    {
        [SerializeField] VisualEffect flameVFX;
        [SerializeField] float targetFlickVelocity = 0.5f;
        [SerializeField] Animation anim;
        [SerializeField] PointableUnityEventWrapper events;
        [SerializeField] Rigidbody rb;

        private bool isOpened = false;
        private bool isGrabbed = false;

        private void Start()
        {
            events.WhenMove.AddListener(OnMove);
        }

        private void OnMove(PointerEvent args)
        {
            if (rb.velocity.z > targetFlickVelocity && !isOpened)
            {
                OpenLighter();
            }
            else if (rb.velocity.z < -targetFlickVelocity && isOpened)
            {
                CloseLighter();
            }
        }

        private void OpenLighter()
        {
            anim.Play("FlickOpen");
            isOpened = true;
        }

        private void CloseLighter()
        {
            anim.Play("FlickClose");
            isOpened = false;
        }








    }
}
