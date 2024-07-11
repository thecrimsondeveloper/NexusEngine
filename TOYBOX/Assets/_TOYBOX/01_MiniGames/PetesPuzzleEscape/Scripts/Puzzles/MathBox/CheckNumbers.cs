using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox
{
    public class CheckNumbers : MonoBehaviour
    {

        [SerializeField] List<PuzzleBoxNumber> allNumbers = new List<PuzzleBoxNumber>();
        [SerializeField] List<PuzzleBoxNumber> allCorrectNumbers = new List<PuzzleBoxNumber>();
        [SerializeField] MeshRenderer cubeMesh;

        private AdditionButton addButton;
        // Start is called before the first frame update


        void CheckCorrectNumbers()
        {
            foreach (PuzzleBoxNumber number in allNumbers)

            {
               

                if (number.CurrentNumber == number.TargetNumber && !allCorrectNumbers.Contains(number))
                {
                    allCorrectNumbers.Add(number);
                }
                else if (number.CurrentNumber != number.TargetNumber)
                {
                    allCorrectNumbers.Remove(number);
                }
               
                    

            }

            if (allCorrectNumbers.Count == allNumbers.Count)
            {
                cubeMesh.material.color = Color.black;
                    
            }
            else
                cubeMesh.material.color = Color.white;



        }

        void Start()
        {

           
            foreach (PuzzleBoxNumber num in allNumbers)
            {
                num.onRelease.AddListener(CheckCorrectNumbers);
            }         
        }

    }
}
