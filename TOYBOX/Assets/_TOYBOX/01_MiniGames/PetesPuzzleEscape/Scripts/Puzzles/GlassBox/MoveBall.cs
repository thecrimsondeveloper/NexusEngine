using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;

namespace ToyBox
{
    public class MoveBall : MonoBehaviour
    {

        public PointableUnityEventWrapper events;

        public UnityEvent onPress;
        public UnityEvent onRelease;

        bool isPressed = false;

        [SerializeField] protected GameObject ball;
        [SerializeField] protected GameObject button;

        public float moveSpeed = 5f; // Adjust this value to control the speed of movement
        public float maxY = 10f; // Adjust this value to set the maximum height
        public float minY = 0f; // Adjust this value to set the minimum height

        void Start()
        {
            //currentNumber = startingNumber;
            events.WhenSelect.AddListener(OnButtonDown);
            events.WhenUnselect.AddListener(OnButtonRelease);

        }

        void Update()
        {
            if (isPressed)
            {
                OnButtonHeld();
            }
        }

        protected void OnButtonHeld()
        {
            MoveBallY();
        }

        protected void OnButtonDown(PointerEvent arg)
        {

            isPressed = true;
            onPress.Invoke();
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            MoveBallY();

        }

        protected void OnButtonRelease(PointerEvent arg)
        {
            isPressed = false;
            onRelease.Invoke();
            button.transform.localPosition = new Vector3(0, 0.015f, 0);

        }

        protected void MoveBallY()
        {
            ball.GetComponent<Rigidbody>().AddForce(Vector3.up * moveSpeed);

        }

    }
}
