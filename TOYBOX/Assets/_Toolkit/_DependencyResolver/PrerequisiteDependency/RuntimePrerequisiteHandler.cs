using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.DependencyResolution
{
    public abstract class RuntimePrerequisiteHandler : MonoBehaviour
    {

        bool isComplete = false;

        public UnityEvent OnPrerequisiteCompleted;


        private void Update()
        {
            if (isComplete == true) return;

            PrerequisiteUpdate();

            if (IsPrerequisiteComplete())
            {
                isComplete = true;
                OnPrerequisiteComplete();
                OnPrerequisiteCompleted?.Invoke();
            }
        }

        protected abstract void OnLoad();
        protected abstract bool IsPrerequisiteComplete();
        protected abstract void PrerequisiteUpdate();
        protected abstract void OnPrerequisiteComplete();




    }
}
