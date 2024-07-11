using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Entity;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox
{
    public class ExampleClickToComplete : MonoBehaviour, ICompletable
    {
        [ShowInInspector] public bool IsComplete { get; set; } = false;

        public UnityEvent OnComplete { get; } = new UnityEvent();
        public UnityEvent OnReset { get; } = new UnityEvent();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space key pressed");
                (this as ICompletable).Complete();
            }

        }

        public void Internal_OnComplete()
        {
            Debug.Log("Internal_OnComplete");
        }

        private void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
            (this as ICompletable).Complete();
        }

        public UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void Internal_OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}
