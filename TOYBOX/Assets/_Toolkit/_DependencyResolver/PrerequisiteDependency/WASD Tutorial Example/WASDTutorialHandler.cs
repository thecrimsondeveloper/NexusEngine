using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Toolkit.DependencyResolution.Examples
{
    public class WASDTutorialHandler : TutorialHandler
    {
        [SerializeField, ReadOnly] List<KeyCode> pressedKeys = new List<KeyCode>();
        protected override void PrerequisiteUpdate()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (pressedKeys.Contains(KeyCode.W) == false)
                    pressedKeys.Add(KeyCode.W);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (pressedKeys.Contains(KeyCode.A) == false)
                    pressedKeys.Add(KeyCode.A);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (pressedKeys.Contains(KeyCode.S) == false)
                    pressedKeys.Add(KeyCode.S);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (pressedKeys.Contains(KeyCode.D) == false)
                    pressedKeys.Add(KeyCode.D);
            }
        }




        protected override bool IsPrerequisiteComplete()
        {
            if (pressedKeys.Count == 4)
            {
                return true;
            }
            return false;
        }

        protected override void OnLoad()
        {
        }

        protected override void OnPrerequisiteComplete()
        {
        }

    }
}
