using System.Collections;
using System.Collections.Generic;
using MediTrainer;
using Sirenix.OdinInspector;
using UnityEngine;
using Toolkit.DependencyResolution;
using System.Runtime.CompilerServices;
using System;
using Cysharp.Threading.Tasks;

namespace PositionHandling
{
    public class PositionHandler : MonoBehaviour
    {
        [SerializeField] DependencyDefinition[] dependencies = new DependencyDefinition[0];
        [SerializeField] public PositionType positionType = PositionType.LeftHand;

        [SerializeField] PositionSetter[] trackedSetters;


        public PositionType Type
        {
            get => positionType;
            set
            {
                positionType = value;
                RefreshActivePositionSetter();
            }
        }
        private async void Start()
        {
            await DependencyManager.ResolveDependencies(dependencies);
        }

        void RefreshActivePositionSetter()
        {
            bool found = false;
            foreach (PositionSetter setter in trackedSetters)
            {
                bool isEnabled = false;
                bool isCorrectType = setter.IsCorrectType(positionType);

                if (isCorrectType && found == false)
                {
                    found = true;
                    isEnabled = true;
                }

                setter.enabled = isEnabled;
            }
        }





    }
}
