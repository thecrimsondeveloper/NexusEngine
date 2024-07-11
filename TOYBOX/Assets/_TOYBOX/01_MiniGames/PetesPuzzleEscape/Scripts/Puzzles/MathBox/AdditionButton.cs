using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{

    public class AdditionButton : PuzzleBoxNumber
    {

        
        protected override void OnButtonDown(PointerEvent arg)
        {
            base.OnButtonDown(arg);
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            Debug.Log("On button down");
            NumberIncrementer();
        }

        protected override void OnButtonRelease(PointerEvent arg)
        {
            base.OnButtonRelease(arg);
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            Debug.Log("On button release");
        }

        public void NumberIncrementer()
        {
            currentNumber = (currentNumber + incrementStep) % 10;
            UpdateText();
            Debug.Log(currentNumber);

        }

        protected override void UpdateText()
        {
            base.UpdateText();
        }
    }
}
