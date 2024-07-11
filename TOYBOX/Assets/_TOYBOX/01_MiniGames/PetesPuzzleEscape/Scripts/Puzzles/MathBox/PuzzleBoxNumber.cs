using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;

namespace ToyBox
{
    public abstract class PuzzleBoxNumber : MonoBehaviour
    {
        [SerializeField] protected int currentNumber = 0;
        [SerializeField] protected int startingNumber = 0;
        [SerializeField] protected int targetNumber = 0;
        [SerializeField] protected int incrementStep = 1;
        [SerializeField] public TextMeshPro numberText;
        [SerializeField] protected GameObject button;


        public PointableUnityEventWrapper events;

        public UnityEvent onPress;
        public UnityEvent onRelease;

        public int TargetNumber => targetNumber;
        public int CurrentNumber => currentNumber;


        bool isPressed = false;


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

        protected virtual void OnButtonHeld()
        {

        }

        protected virtual void OnButtonDown(PointerEvent arg)
        {

            isPressed = true;
            onPress.Invoke();


        }

        protected virtual void OnButtonRelease(PointerEvent arg)
        {
            isPressed = false;
            onRelease.Invoke();

        }


        protected virtual void UpdateText()
        {
            if (numberText != null)
            {
                numberText.text = currentNumber.ToString();
            }
        }










    }
}
