using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ToyBox;
using Unity.VisualScripting;
using UnityEngine;

namespace Toolkit.DependencyResolution.Examples
{
    [CreateAssetMenu(fileName = "WASDTutorialDependency", menuName = "LLGD/Dependencies/WASD Tutorial Dependency")]
    public class KeyPressTutorialDependency : TutorialDependency
    {
        protected override void OnPrerequisiteComplete()
        {

        }
    }
}
