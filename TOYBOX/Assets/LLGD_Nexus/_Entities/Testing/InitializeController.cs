using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class InitializeController : NexusBlock
    {
        public UnityEvent OnInitialize = new UnityEvent();
        private void Start()
        {
            OnInitialize.Invoke();
        }
    }
}
