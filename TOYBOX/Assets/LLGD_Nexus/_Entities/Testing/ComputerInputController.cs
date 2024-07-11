using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class ComputerInputController : MonoBehaviour
    {

        public UnityEvent OnSpacePressed;
        public UnityEvent OnLeftMousePressed;
        public UnityEvent OnRightMousePressed;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpacePressed.Invoke();
            }
        }
    }
}
