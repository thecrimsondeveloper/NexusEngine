using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class Numbers : MonoBehaviour
    {

        private int currentNumber = 0;
        public int startingNumber;
        public int targetNumber;
        public int incrementStep = 1;
       
        public Color newColor = Color.black;
        private static List<Numbers> allNumbers = new List<Numbers  >();

        private TextMeshPro textMeshPro;
     //   public TextMeshPro CompletedText;

        public GameObject button;
        public UnityEvent onPress;
        public UnityEvent onRelease;
        GameObject presser;
        bool isPressed;

        private GameObject cube;
        private Renderer cubeRenderer;
        public Color targetColor = Color.black;





        void Start()
        {
            currentNumber = startingNumber;
            textMeshPro = GetComponentInChildren<TextMeshPro>();
            cube = GameObject.Find("Cube");
            if (cube != null)
            {
                cubeRenderer = cube.GetComponent<Renderer>();
                
            }
            else
            {
                Debug.LogError("Cube GameObject not found in the scene.");
            }
            //CompletedText = GetComponentInParent<TextMeshPro>();
            isPressed = false;
            allNumbers.Add(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isPressed)
            {
                button.transform.localPosition = new Vector3(0, 0.003f,0);
                presser = other.gameObject;
                onPress.Invoke();
                isPressed = true;
                
            }
        }
        private void OnTriggerExit(Collider other) 
        {  if (other.gameObject == presser)
            {
               button.transform.localPosition = new Vector3(0, 0.015f, 0);
               onRelease.Invoke();
               isPressed = false;
            } 
        }

        public void NumberIncrementer()
        {
            currentNumber = (currentNumber + incrementStep) % 10;
            UpdateText();
            Debug.Log(currentNumber);
            CheckSuccess();
        }


        public void NumberDecrementer()
        {      
            currentNumber = (currentNumber + 10 - 1) % 10;
            UpdateText();
            Debug.Log(currentNumber);
            CheckSuccess();
        }



        void UpdateText()
        {
            if (textMeshPro != null)
            {
                textMeshPro.text = currentNumber.ToString();
            }
        }

        void CheckSuccess()
        {

            if (currentNumber == targetNumber)
            {
                bool allCompleted = true;
                foreach (var number in allNumbers)
                {
                    if (number.currentNumber != number.targetNumber)
                    {
                        allCompleted = false;
                        break;
                    }
                }

                // If all NumberSquares have reached their targets, log "Completed!"
                if (allCompleted)
                {
                    Debug.Log("Completed!");
                   // CompletedText.text = "Puzzle 1 Completed!";
 
                    cubeRenderer.material.color = targetColor;

                }


            }

        }



    }
}
